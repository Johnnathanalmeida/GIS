using GISCore.Business.Abstract;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GISWeb.Controllers
{
    public class TipoDeDocumentoController : Controller
    {

        #region Inject

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        [Inject]
        public IBaseBusiness<TipoDeDocumento> TipoDeDocumentoBusiness { get; set; }
        
        #endregion

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Novo(string UKCategoria)
        {
            try
            {
                TipoDeDocumento objTipo = new TipoDeDocumento();
                objTipo.UKCategoriaDeDocumento = UKCategoria;
                ViewBag.Intervalos = TipoDeDocumentoBusiness.GetTodosEnumsIntervalo();

                return PartialView("Novo", objTipo);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }
        }

	}
}