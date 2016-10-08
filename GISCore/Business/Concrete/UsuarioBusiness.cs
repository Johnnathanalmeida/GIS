using GISCore.Business.Abstract;
using GISModel.DTO.Conta;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class UsuarioBusiness : BaseBusiness<Usuario>, IUsuarioBusiness
    {

        public Usuario ValidarCredenciais(AutenticacaoModel autenticacaoModel)
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
                        return oUsuario;
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

            //Enviar e-mail
            EnviarEmailParaUsuarioRecemCriado(usuario);
        }

        private void EnviarEmailParaUsuarioRecemCriado(Usuario usuario)
        {
            var body = "<p>Email From: {0} ({1})</p><p>Message:</p><p>{2}</p>";
            var message = new MailMessage();
            message.To.Add(new MailAddress(usuario.Email));  // replace with valid value 
            message.From = new MailAddress("johnnathanalmeida22@gmail.com");  // replace with valid value
            message.Subject = "Your email subject";
            message.Body = string.Format(body, usuario.Nome, usuario.Email, "Teste");
            message.IsBodyHtml = true;

            using (var smtp = new SmtpClient())
            {
                var credential = new NetworkCredential
                {
                    UserName = "johnnathanalmeida22@gmail.com",  // replace with valid value
                    Password = "jrpalmeidaasdf0422"  // replace with valid value
                };
                smtp.Credentials = credential;
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.SendMailAsync(message);
                
            }
        }
    }
}
