using GISCore.Business.Abstract;
using GISModel.DTO.Menu;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GISWeb.Controllers
{
    public class PerfilMenuController : Controller
    {

        #region Inject

            [Inject]
            public IPerfilMenuBusiness PerfilMenuBusiness { get; set; }

            [Inject]
            public IPerfilBusiness PerfilBusiness { get; set; }

        #endregion

        [HttpPost]
        public ActionResult SalvarEdicaoVinculo(bool Acao, string Perfil, string Menu)
        {
            try
            {
                if (Acao)
                {
                    //Incluir vinculo entre Menu e Perfil
                    if (Menu.Contains("|"))
                    {
                        foreach (string IDUsuario in Menu.Split('|'))
                        {
                            if (!string.IsNullOrEmpty(IDUsuario))
                            {
                                PerfilMenuBusiness.Inserir(new PerfilMenu() { IDMenu = IDUsuario, IDPerfil = Perfil });
                            }
                        }
                    }
                    else
                    {
                        PerfilMenuBusiness.Inserir(new PerfilMenu() { IDMenu = Menu, IDPerfil = Perfil });
                    }
                }
                else
                {
                    //Remover vinculo entre Menu e Perfil
                    if (Menu.Contains("|"))
                    {
                        foreach (string IDUsuario in Menu.Split('|'))
                        {
                            if (!string.IsNullOrEmpty(IDUsuario))
                            {
                                PerfilMenuBusiness.Alterar(new PerfilMenu() { IDMenu = IDUsuario, IDPerfil = Perfil, DataExclusao = DateTime.Now, UsuarioExclusao = "LoginTeste" });
                            }
                        }
                    }
                    else
                    {
                        PerfilMenuBusiness.Alterar(new PerfilMenu() { IDMenu = Menu, IDPerfil = Perfil, DataExclusao = DateTime.Now, UsuarioExclusao = "LoginTeste" });
                    }
                }

                return Json(new { resultado = new RetornoJSON() { } });
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException() == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
                }
                else
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = ex.GetBaseException().Message } });
                }
            }
        }

        [HttpPost]
        public ActionResult BuscarPerfisPorMenu(string IDMenu)
        {
            try
            {
                var varPerfis = from perfil in PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                join perfilmenu in PerfilMenuBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.IDMenu.Equals(IDMenu)).ToList() on perfil.IDPerfil equals perfilmenu.IDPerfil into prodGroup
                                from item in prodGroup.DefaultIfEmpty()
                                select new PerfilMenuViewModel { NomePerfil = perfil.Nome, IDPerfil = perfil.IDPerfil, MenuVinculado = (item == null ? false : true), IDMenu = IDMenu };

                List<PerfilMenuViewModel> lPerfis = varPerfis.ToList();

                return Json(new { data = RenderRazorViewToString("_PerfisPorMenu", lPerfis), perfis = lPerfis.Count });
            }
            catch (Exception ex)
            {
                if (ex.GetBaseException() == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
                }
                else
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = ex.GetBaseException().Message } });
                }
            }
        }

        private string RenderRazorViewToString(string viewName, object model = null)
        {
            ViewData.Model = model;
            using (var sw = new System.IO.StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(ControllerContext,
                                                                         viewName);
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

    }
}