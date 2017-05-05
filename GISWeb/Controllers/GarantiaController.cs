using GISCore.Business.Abstract;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GISWeb.Controllers
{
    public class GarantiaController : Controller
    {

        [Inject]
        public IBaseBusiness<Garantia> BaseBusiness { get; set; }

        [RestritoAAjax]
        public ActionResult NovaGarantia()
        {
            try
            {
                ViewBag.Intervalos = BaseBusiness.GetTodosEnumsIntervalo();
                return PartialView("_NovaGarantia");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }
        }
	
    }
}