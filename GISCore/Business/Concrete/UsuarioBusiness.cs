using GISCore.Business.Abstract;
using GISModel.DTO.Conta;
using GISModel.DTO.Usuario;
using GISModel.Entidades;
using GISModel.Enums;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class UsuarioBusiness : BaseBusiness<Usuario>, IUsuarioBusiness
    {

        #region Inject 

            [Inject]
            public IUsuarioPerfilBusiness UsuarioPerfilBusiness { get; set; }
        
            [Inject]
            public IPerfilBusiness PerfilBusiness { get; set; }

            [Inject]
            public IPerfilMenuBusiness PerfilMenuBusiness { get; set; }

            [Inject]
            public MenuBusiness MenuBusiness { get; set; }

        #endregion

        public UsuarioPerfisMenusViewModel ValidarCredenciais(AutenticacaoModel autenticacaoModel)
        {
            autenticacaoModel.Login = autenticacaoModel.Login.Trim();

            //Buscar usuário sem validar senha, para poder determinar se a validação da senha será com AD ou com a senha interna do GIS
            List<Usuario> lUsuarios = Consulta.Where(u => u.Login.Equals(autenticacaoModel.Login) || 
                                                          u.CPF.Equals(autenticacaoModel.Login) || 
                                                          u.Email.Equals(autenticacaoModel.Login)).ToList();

            if (lUsuarios.Count > 1 || lUsuarios.Count < 1)
            {
                throw new Exception("Não foi possível identificar o seu cadastro.");
            }
            else {

                if (lUsuarios[0].TipoDeAcesso.Equals(0))
                {
                    //Login, validando a senha no AD

                    if (Convert.ToBoolean(ConfigurationManager.AppSettings["AD:DMZ"]))
                    {
                        //Chamar web service para validar a senha no AD
                        return null;
                    }
                    else {

                        using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, lUsuarios[0].Empresa.URL_AD))
                        {
                            if (pc.ValidateCredentials(autenticacaoModel.Login, autenticacaoModel.Senha))
                                return null;
                            else
                                throw new Exception("Login ou senha incorretos.");
                        }
                    }

                }
                else { 
                    //Login, validando a senha interna do GIS
                    string IDUsuario = lUsuarios[0].IDUsuario;

                    Usuario oUsuario = Consulta.FirstOrDefault(p => p.IDUsuario.Equals(IDUsuario) && p.Senha.Equals(autenticacaoModel.Senha));

                    if (oUsuario != null)
                    {
                        UsuarioPerfisMenusViewModel oUPMViewModel = new UsuarioPerfisMenusViewModel();
                        oUPMViewModel.Usuario = oUsuario;

                        var lPerfis = from usuarioperfil in UsuarioPerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                      join perfil in PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usuarioperfil.IDPerfil equals perfil.IDPerfil
                                      where usuarioperfil.IDUsuario.Equals(oUsuario.IDUsuario)
                                      select new Perfil { Nome = perfil.Nome, IDPerfil = perfil.IDPerfil };

                        oUPMViewModel.Perfis = lPerfis.ToList();

                        List<Menu> lMenus = new List<Menu>();

                        foreach (Perfil iPerfil in lPerfis.ToList()) {
                            var listaMenus = from perfilmenu in PerfilMenuBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.IDPerfil.Equals(iPerfil.IDPerfil)).ToList()
                                             join menu in MenuBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on perfilmenu.IDMenu equals menu.IDMenu
                                             select new Menu { Nome = menu.Nome, 
                                                               IDMenu = menu.IDMenu, 
                                                               Ordem = menu.Ordem, 
                                                               Controller = menu.Controller, 
                                                               Action = menu.Action, 
                                                               Icone = menu.Icone,
                                                               MenuSuperior = string.IsNullOrEmpty(menu.IDMenuSuperior) ? null : new Menu { IDMenu = menu.IDMenuSuperior, Nome = MenuBusiness.Consulta.FirstOrDefault(i => i.IDMenu.Equals(menu.IDMenuSuperior)).Nome } };

                            lMenus.AddRange(listaMenus.ToList());
                        }

                        lMenus = (from mci in lMenus select mci).Distinct().ToList();
                        oUPMViewModel.Menus = lMenus;

                        return oUPMViewModel;
                    }
                    else {
                        throw new Exception("Login ou senha incorretos.");
                    }
                }
            }
            
        }

        public byte[] RecuperarFotoPerfil(string idUsuario)
        {
            if (idUsuario == null) 
                return null;
            
            idUsuario = idUsuario.Trim();

            List<Usuario> lUsuarios = Consulta.Where(u => u.IDUsuario.Equals(idUsuario)).ToList();
            if (lUsuarios.Count > 1 || lUsuarios.Count < 1)
            {
                return null;
            }
            else
            {
                var dirAvatar = Path.GetFullPath(ConfigurationManager.AppSettings["DiretorioRaiz"] + "\\Content\\Ace\\avatars\\" + lUsuarios[0].Empresa.CNPJ + "\\" + idUsuario + ".jpg");
                if (File.Exists(dirAvatar))
                {
                    byte[] imageBytesArray = File.ReadAllBytes(dirAvatar);
                    return imageBytesArray;
                }
                else
                {
                    return null;
                }
            }
                  
        }

        public override void Inserir(Usuario usuario)
        {
            if (Consulta.Any(u => u.Login.Equals(usuario.Login)))
                throw new InvalidOperationException("Não é possível inserir usuário com o mesmo login.");

            usuario.IDUsuario = Guid.NewGuid().ToString();

            base.Inserir(usuario);


            if (usuario.TipoDeAcesso.Equals(TipoDeAcesso.AD))
            {
                EnviarEmailParaUsuarioRecemCriadoAD(usuario);
            }
            else {
                EnviarEmailParaUsuarioRecemCriadoSistema(usuario);
            }
            
        }

        private void EnviarEmailParaUsuarioRecemCriadoSistema(Usuario usuario)
        {
            MailMessage mail = new MailMessage("johnnathanalmeida22@gmail.com", usuario.Email);
            mail.Subject = "GiS - Seja bem-vindo!";
            mail.Body = "<html style=\"font-family: Verdana; font-size: 11pt;\"><body>Prezado(a) " + usuario.Nome;
            mail.Body += "<br /><br />";

            string NomeUsuarioInclusao = usuario.UsuarioInclusao;
            Usuario uInclusao = Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Login.Equals(usuario.UsuarioInclusao));
            if (uInclusao != null && !string.IsNullOrEmpty(uInclusao.Nome))
                NomeUsuarioInclusao = uInclusao.Nome;


            string sLink = "http://localhost:26717/Conta/DefinirNovaSenha/" + WebUtility.UrlEncode(GISHelpers.Utils.Criptografador.Criptografar(usuario.IDUsuario + "#" + DateTime.Now.ToString("yyyyMMdd"), 1)).Replace("%", "_@");

            mail.Body += "Você foi cadastrado no sistema GiS - Gestão Inteligente da Segurança pelo " + NomeUsuarioInclusao + ".";
            mail.Body += "<br /><br />";
            mail.Body += "Clique <a href=\"" + sLink + "\">aqui</a> para ativar sua conta.";
            mail.Body += "<br /><br />";
            mail.Body += "Atenciosamente,";
            mail.Body += "<br /><br />";
            mail.Body += "<span style=\"color: #ccc; font-style: italic;\">Mensagem enviada automaticamente, favor não responder este email.</span>";
            mail.Body += "<br /><br />";
            mail.Body += "<strong>Gestão Inteligente da Segurança - GiS</strong>";
            mail.Body += "</body></html>";

            mail.IsBodyHtml = true;
            mail.BodyEncoding = Encoding.UTF8;
            
            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "johnnathanalmeida22@gmail.com",
                Password = "jrpalmeidaasdf0422"
            };

            smtpClient.EnableSsl = true;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };

            smtpClient.Send(mail);

        }

        private void EnviarEmailParaUsuarioRecemCriadoAD(Usuario usuario)
        {
            MailMessage mail = new MailMessage("johnnathanalmeida22@gmail.com", usuario.Email);
            mail.Subject = "GiS - Seja bem-vindo!";
            mail.Body = "<html style=\"font-family: Verdana; font-size: 11pt;\"><body>Prezado(a) " + usuario.Nome;
            mail.Body += "<br /><br />";

            string NomeUsuarioInclusao = usuario.UsuarioInclusao;
            Usuario uInclusao = Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Login.Equals(usuario.UsuarioInclusao));
            if (uInclusao != null && !string.IsNullOrEmpty(uInclusao.Nome))
                NomeUsuarioInclusao = uInclusao.Nome;


            string sLink = "http://localhost:26717/Conta/DefinirNovaSenha/" + WebUtility.UrlEncode(GISHelpers.Utils.Criptografador.Criptografar(usuario.IDUsuario + "#" + DateTime.Now.ToString("yyyyMMdd"), 1)).Replace("%", "_@");

            mail.Body += "Você foi cadastrado no sistema GiS - Gestão Inteligente da Segurança pelo " + NomeUsuarioInclusao + ".";
            mail.Body += "<br /><br />";
            mail.Body += "Clique <a href=\"" + sLink + "\">aqui</a> para ativar sua conta.";
            mail.Body += "<br /><br />";
            mail.Body += "Atenciosamente,";
            mail.Body += "<br /><br />";
            mail.Body += "<span style=\"color: #ccc; font-style: italic;\">Mensagem enviada automaticamente, favor não responder este email.</span>";
            mail.Body += "<br /><br />";
            mail.Body += "<strong>Gestão Inteligente da Segurança - GiS</strong>";
            mail.Body += "</body></html>";

            mail.IsBodyHtml = true;
            mail.BodyEncoding = Encoding.UTF8;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

            smtpClient.Credentials = new System.Net.NetworkCredential()
            {
                UserName = "johnnathanalmeida22@gmail.com",
                Password = "jrpalmeidaasdf0422"
            };

            smtpClient.EnableSsl = true;
            System.Net.ServicePointManager.ServerCertificateValidationCallback = delegate(object s,
                    System.Security.Cryptography.X509Certificates.X509Certificate certificate,
                    System.Security.Cryptography.X509Certificates.X509Chain chain,
                    System.Net.Security.SslPolicyErrors sslPolicyErrors)
            {
                return true;
            };

            smtpClient.Send(mail);

        }

        [ComVisible(false)]
        private string CreateHashFromPassword(string pstrOriginalPassword)
        {
            if (string.IsNullOrEmpty(pstrOriginalPassword))
                return string.Empty;

            string str3 = ConvertToHashedString(pstrOriginalPassword).Substring(0, 5);
            byte[] bytes = Encoding.UTF8.GetBytes(pstrOriginalPassword + str3);
            HashAlgorithm lobjHash = new MD5CryptoServiceProvider();
            return Convert.ToBase64String(lobjHash.ComputeHash(bytes));
        }

        [ComVisible(false)]
        private string ConvertToHashedString(string pstrOriginal)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(pstrOriginal);
            HashAlgorithm lobjHash = new MD5CryptoServiceProvider();
            return Convert.ToBase64String(lobjHash.ComputeHash(bytes));
        }

    }
}
