using GISWeb.Infraestrutura.Provider.Abstract;
using GISCore.Business.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GISModel.Entidades;

namespace GISWeb.Controllers
{
    public class CategoriaDeDocumentoController : Controller
    {

        #region Inject

            [Inject]
            public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

            [Inject]
            public IBaseBusiness<CategoriaDeDocumento> CategoriaDeDocumentoBusiness { get; set; }

        #endregion



	}
}