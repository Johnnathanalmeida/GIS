//using BotDetect.Web.UI.Mvc;
using GISCore.Business.Abstract;
using GISModel.DTO.Conta;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace GISWeb.Controllers
{
    public class ContaController : Controller
    {

        #region

            [Inject]
            public ICustomAuthorizationProvider AutorizacaoProvider { get; set; }

            [Inject]
            public IUsuarioBusiness UsuarioBusiness { get; set; }

        #endregion

        public ActionResult Login()
        {
            return View();
        }

        [Autorizador]
        [DadosUsuario]
        public ActionResult Perfil()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AutenticacaoModel usuario)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    AutorizacaoProvider.Logar(usuario);
                    return Json(new { url = Url.Action("Index", "Painel") });
                }

                return View(usuario);
            }
            catch (Exception ex)
            {
                return Json(new { alerta = ex.Message, titulo = "Oops! Problema ao realizar login..." });
            }
        }

        //[HttpPost]
        //[AllowAnonymous]
        //[ValidateAntiForgeryToken]
        //[CaptchaValidation("CaptchaCode", "LoginCaptcha", "Código do CAPTCHA incorreto.")]
        //public ActionResult LoginComCaptcha(AutenticacaoModel usuario)
        //{
        //    MvcCaptcha.ResetCaptcha("LoginCaptcha");
        //    ViewBag.IncluirCaptcha = Convert.ToBoolean(ConfigurationManager.AppSettings["AD:IncluirCaptchaNoLogin"]);

        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            AutorizacaoProvider.Logar(usuario);

        //            return Json(new { url = Url.Action(ConfigurationManager.AppSettings["Web:DefaultAction"], ConfigurationManager.AppSettings["Web:DefaultController"]) });
        //        }

        //        return View("Login", usuario);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { alerta = ex.Message, titulo = "Oops! Problema ao realizar login..." });
        //    }
        //}

        public ActionResult Logout()
        {
            AutorizacaoProvider.Deslogar();
            Session.Clear();

            return RedirectToAction("Login", "Conta");
        }

        [OutputCache(Duration = 604800, Location = OutputCacheLocation.Client, VaryByParam = "login")]
        public ActionResult FotoPerfil(string login)
        {
            byte[] avatar = null;

            try
            {
                avatar = UsuarioBusiness.RecuperarFotoPerfil(login);
            }
            catch { }

            if (avatar == null || avatar.Length == 0)
                avatar = System.IO.File.ReadAllBytes(Server.MapPath("~/Content/Ace/avatars/unknown.png"));

            return File(avatar, "image/jpeg");
        }

	}
}