using GISWeb.Infraestrutura.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GISWeb.Controllers
{
    public class EstabelecimentoController : Controller
    {

        [RestritoAAjax]
        public ActionResult Novo()
        {
            try
            {
                return PartialView("_Novo");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }
        }

	}
}