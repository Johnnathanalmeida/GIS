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

                    string senhaTemp = CreateHashFromPassword(autenticacaoModel.Senha);
                    Usuario oUsuario = Consulta.FirstOrDefault(p => p.IDUsuario.Equals(IDUsuario) && p.Senha.Equals(senhaTemp));

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
            if (Consulta.Any(u => u.Login.Equals(usuario.Login) && string.IsNullOrEmpty(u.UsuarioExclusao)))
                throw new InvalidOperationException("Este login já está sendo usado por outro usuário.");

            if (Consulta.Any(u => u.CPF.Equals(usuario.CPF) && string.IsNullOrEmpty(u.UsuarioExclusao)))
                throw new InvalidOperationException("Este CPF já está sendo usado por outro usuário.");

            if (Consulta.Any(u => u.Email.Equals(usuario.Email) && string.IsNullOrEmpty(u.UsuarioExclusao)))
                throw new InvalidOperationException("Este e-mail já está sendo usado por outro usuário.");

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

        public override void Alterar(Usuario entidade)
        {

            Usuario tempUsuario = Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.IDUsuario.Equals(entidade.IDUsuario));
            if (tempUsuario == null)
            {
                throw new Exception("Não foi possível encontrar o usuário através do ID.");
            }
            else
            {
                if (Consulta.Any(u => u.Email.Equals(entidade.Email) && string.IsNullOrEmpty(u.UsuarioExclusao) && !entidade.Login.ToUpper().Equals(u.Login.ToUpper())))
                    throw new InvalidOperationException("Este e-mail já está sendo usado por outro usuário.");

                tempUsuario.DataExclusao = DateTime.Now;
                tempUsuario.UsuarioExclusao = entidade.UsuarioExclusao;
                base.Alterar(tempUsuario);

                entidade.IDUsuario = Guid.NewGuid().ToString();
                entidade.UsuarioExclusao = string.Empty;
                base.Inserir(entidade);

            }

        }

        public void DefinirSenha(NovaSenhaViewModel novaSenhaViewModel) {
            Usuario oUsuario = Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.IDUsuario.Equals(novaSenhaViewModel.IDUsuario));
            if (oUsuario == null)
            {
                throw new Exception("Não foi possível localizar o usuário através da identificação. Solicite um novo acesso.");
            }
            else {
                oUsuario.Senha = CreateHashFromPassword(novaSenhaViewModel.NovaSenha);
                Alterar(oUsuario);
                EnviarEmailParaUsuarioSenhaAlterada(oUsuario);
            }
        }

        public void SolicitarAcesso(string email) {
            List<Usuario> listaUsuarios = Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Email.ToLower().Equals(email.ToLower())).ToList();
            if (listaUsuarios.Count() > 1 || listaUsuarios.Count() < 1)
            {
                listaUsuarios = Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Login.ToLower().Equals(email.ToLower())).ToList();
                if (listaUsuarios.Count() > 1 || listaUsuarios.Count() < 1)
                {
                    throw new Exception("Não foi possível localizar este usuário no sistema através do e-mail. Tente novamente ou procure o Administrador.");
                }
            }
            
            EnviarEmailParaUsuarioSolicacaoAcesso(listaUsuarios[0]);
        }

        #region E-mails

            private void EnviarEmailParaUsuarioSolicacaoAcesso(Usuario usuario)
            {
                string sRemetente = ConfigurationManager.AppSettings["Web:Remetente"];
                string sSMTP = ConfigurationManager.AppSettings["Web:SMTP"];

                MailMessage mail = new MailMessage(sRemetente, usuario.Email);

                string PrimeiroNome = GISHelpers.Utils.Severino.PrimeiraMaiusculaTodasPalavras(usuario.Nome);
                if (PrimeiroNome.Contains(" "))
                    PrimeiroNome = PrimeiroNome.Substring(0, PrimeiroNome.IndexOf(" "));

                mail.Subject = PrimeiroNome + ", este é o link para redinir sua senha";
                mail.Body = "<html style=\"font-family: Verdana; font-size: 11pt;\"><body>Olá, " + PrimeiroNome + ".";
                mail.Body += "<br /><br />";
                mail.Body += "<span style=\"color: #222;\">Redefina sua senha para começar novamente.";
                mail.Body += "<br /><br />";

                string sLink = "http://localhost:26717/Conta/DefinirNovaSenha/" + WebUtility.UrlEncode(GISHelpers.Utils.Criptografador.Criptografar(usuario.IDUsuario + "#" + DateTime.Now.ToString("yyyyMMdd"), 1)).Replace("%", "_@");

                mail.Body += "Para alterar sua senha do GiS, clique <a href=\"" + sLink + "\">aqui</a> ou cole o seguinte link no seu navegador.";
                mail.Body += "<br /><br />";
                mail.Body += sLink;
                mail.Body += "<br /><br />";
                mail.Body += "O link é válido por 24 horas, portanto, utilize-o imediatamente.";
                mail.Body += "<br /><br />";
                mail.Body += "Obrigado por utilizar o GiS!<br />";
                mail.Body += "<strong>Gestão Inteligente da Segurança</strong>";
                mail.Body += "</span>";
                mail.Body += "<br /><br />";
                mail.Body += "<span style=\"color: #aaa; font-size: 10pt; font-style: italic;\">Mensagem enviada automaticamente, favor não responder este email.</span>";
                mail.Body += "</body></html>";

                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8;


                SmtpClient smtpClient = new SmtpClient(sSMTP, 587);

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

            private void EnviarEmailParaUsuarioSenhaAlterada(Usuario usuario)
            {
                string sRemetente = ConfigurationManager.AppSettings["Web:Remetente"];
                string sSMTP = ConfigurationManager.AppSettings["Web:SMTP"];

                MailMessage mail = new MailMessage(sRemetente, usuario.Email);

                string PrimeiroNome = GISHelpers.Utils.Severino.PrimeiraMaiusculaTodasPalavras(usuario.Nome);
                if (PrimeiroNome.Contains(" "))
                    PrimeiroNome = PrimeiroNome.Substring(0, PrimeiroNome.IndexOf(" "));

                mail.Subject = PrimeiroNome + ", sua senha foi redefinida.";

                mail.Body = "<html style=\"font-family: Verdana; font-size: 11pt;\"><body>Olá, " + PrimeiroNome + ".";
                mail.Body += "<br /><br />";
                mail.Body += "<span style=\"color: #222;\">Você redefiniu sua senha do GiS.";
                mail.Body += "<br /><br />";
                mail.Body += "Obrigado por utilizar o GiS!<br />";
                mail.Body += "<strong>Gestão Inteligente da Segurança</strong>";
                mail.Body += "</span>";
                mail.Body += "<br /><br />";
                mail.Body += "<span style=\"color: #aaa; font-size: 10pt; font-style: italic;\">Mensagem enviada automaticamente, favor não responder este email.</span>";
                mail.Body += "</body></html>";

                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8;


                SmtpClient smtpClient = new SmtpClient(sSMTP, 587);

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

            private void EnviarEmailParaUsuarioRecemCriadoSistema(Usuario usuario)
            {
                string sRemetente = ConfigurationManager.AppSettings["Web:Remetente"];
                string sSMTP = ConfigurationManager.AppSettings["Web:SMTP"];

                MailMessage mail = new MailMessage(sRemetente, usuario.Email);

                string PrimeiroNome = GISHelpers.Utils.Severino.PrimeiraMaiusculaTodasPalavras(usuario.Nome);
                if (PrimeiroNome.Contains(" "))
                    PrimeiroNome = PrimeiroNome.Substring(0, PrimeiroNome.IndexOf(" "));

                mail.Subject = PrimeiroNome + ", seja bem-vindo!";
                mail.Body = "<html style=\"font-family: Verdana; font-size: 11pt;\"><body>Olá, " + PrimeiroNome + ";";
                mail.Body += "<br /><br />";

                string NomeUsuarioInclusao = usuario.UsuarioInclusao;
                Usuario uInclusao = Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Login.Equals(usuario.UsuarioInclusao));
                if (uInclusao != null && !string.IsNullOrEmpty(uInclusao.Nome))
                    NomeUsuarioInclusao = uInclusao.Nome;


                string sLink = "http://localhost:26717/Conta/DefinirNovaSenha/" + WebUtility.UrlEncode(GISHelpers.Utils.Criptografador.Criptografar(usuario.IDUsuario + "#" + DateTime.Now.ToString("yyyyMMdd"), 1)).Replace("%", "_@");

                mail.Body += "Você foi cadastrado no sistema GiS - Gestão Inteligente da Segurança pelo " + GISHelpers.Utils.Severino.PrimeiraMaiusculaTodasPalavras(NomeUsuarioInclusao) + ".";
                mail.Body += "<br /><br />";
                mail.Body += "Clique <a href=\"" + sLink + "\">aqui</a> para ativar sua conta ou cole o seguinte link no seu navegador.";
                mail.Body += "<br /><br />";
                mail.Body += sLink;
                mail.Body += "<br /><br />";
                mail.Body += "Obrigado por utilizar o GiS!<br />";
                mail.Body += "<strong>Gestão Inteligente da Segurança</strong>";
                mail.Body += "<br /><br />";
                mail.Body += "<span style=\"color: #ccc; font-style: italic;\">Mensagem enviada automaticamente, favor não responder este email.</span>";
                mail.Body += "</body></html>";

                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8;

                SmtpClient smtpClient = new SmtpClient(sSMTP, 587);

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
                string sRemetente = ConfigurationManager.AppSettings["Web:Remetente"];
                string sSMTP = ConfigurationManager.AppSettings["Web:SMTP"];

                MailMessage mail = new MailMessage(sRemetente, usuario.Email);

                string PrimeiroNome = GISHelpers.Utils.Severino.PrimeiraMaiusculaTodasPalavras(usuario.Nome);
                if (PrimeiroNome.Contains(" "))
                    PrimeiroNome = PrimeiroNome.Substring(0, PrimeiroNome.IndexOf(" "));

                mail.Subject = PrimeiroNome + ", seja bem-vindo!";
                mail.Body = "<html style=\"font-family: Verdana; font-size: 11pt;\"><body>Olá, " + PrimeiroNome + ".";
                mail.Body += "<br /><br />";

                string NomeUsuarioInclusao = usuario.UsuarioInclusao;
                Usuario uInclusao = Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Login.Equals(usuario.UsuarioInclusao));
                if (uInclusao != null && !string.IsNullOrEmpty(uInclusao.Nome))
                    NomeUsuarioInclusao = uInclusao.Nome;

                string sLink = "http://localhost:26717/";

                mail.Body += "Você foi cadastrado no sistema GiS - Gestão Inteligente da Segurança pelo " + NomeUsuarioInclusao + ".";
                mail.Body += "<br /><br />";
                mail.Body += "Clique <a href=\"" + sLink + "\">aqui</a> para acessar a sua conta ou cole o seguinte link no seu navegador.";
                mail.Body += "<br /><br />";
                mail.Body += sLink;
                mail.Body += "<br /><br />";
                mail.Body += "Obrigado por utilizar o GiS!<br />";
                mail.Body += "<strong>Gestão Inteligente da Segurança</strong>";
                mail.Body += "<br /><br />";
                mail.Body += "<span style=\"color: #ccc; font-style: italic;\">Mensagem enviada automaticamente, favor não responder este email.</span>";
                mail.Body += "</body></html>";

                mail.IsBodyHtml = true;
                mail.BodyEncoding = Encoding.UTF8;

                SmtpClient smtpClient = new SmtpClient(sSMTP, 587);

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

        #endregion

        #region Senhas

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

        #endregion

    }
}
