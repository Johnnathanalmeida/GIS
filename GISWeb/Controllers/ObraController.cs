using GISWeb.Infraestrutura.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GISWeb.Controllers
{

    [DadosUsuario]
    [Autorizador]
    public class ObraController : BaseController
    {
        //
        // GET: /Obra/
        public ActionResult Index()
        {
            return View();
        }
	}
}