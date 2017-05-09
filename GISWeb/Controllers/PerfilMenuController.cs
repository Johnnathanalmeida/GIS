using GISCore.Business.Abstract;
using GISModel.DTO.Menu;
using GISModel.DTO.Shared;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GISWeb.Controllers
{

    [DadosUsuario]
    [Autorizador]
    public class PerfilMenuController : BaseController
    {

        #region Inject

            [Inject]
            public IPerfilMenuBusiness PerfilMenuBusiness { get; set; }

            [Inject]
            public IPerfilBusiness PerfilBusiness { get; set; }

            [Inject]
            public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

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
                                PerfilMenuBusiness.Inserir(new PerfilMenu() { UKMenu = IDUsuario, UKPerfil = Perfil, UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login });
                            }
                        }
                    }
                    else
                    {
                        PerfilMenuBusiness.Inserir(new PerfilMenu() { UKMenu = Menu, UKPerfil = Perfil, UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login });
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
                                PerfilMenuBusiness.Alterar(new PerfilMenu() { UKMenu = IDUsuario, UKPerfil = Perfil, DataExclusao = DateTime.Now, UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login });
                            }
                        }
                    }
                    else
                    {
                        PerfilMenuBusiness.Alterar(new PerfilMenu() { UKMenu = Menu, UKPerfil = Perfil, DataExclusao = DateTime.Now, UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login });
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
                                join perfilmenu in PerfilMenuBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKMenu.Equals(IDMenu)).ToList() on perfil.UniqueKey equals perfilmenu.UKPerfil into prodGroup
                                from item in prodGroup.DefaultIfEmpty()
                                select new PerfilMenuViewModel { NomePerfil = perfil.Nome, IDPerfil = perfil.UniqueKey, MenuVinculado = (item == null ? false : true), IDMenu = IDMenu };

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

    }
}