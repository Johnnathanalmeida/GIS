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
        public IBaseBusiness<TipoDeDocumento> CategoriaDeDocumentoBusiness { get; set; }

        #endregion

        public ActionResult Index()
        {
            return View();
        }

	}
}