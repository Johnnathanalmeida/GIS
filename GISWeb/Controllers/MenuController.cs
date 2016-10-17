using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
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

    [DadosUsuario]
    [Autorizador]
    public class MenuController : BaseController
    {

        #region

            [Inject]
            public IMenuBusiness MenuBusiness { get; set; }

            [Inject]
            public IPerfilMenuBusiness PerfilMenuBusiness { get; set; }

        #endregion

        [MenuAtivo(MenuAtivo = "Administração/Menu")]
        public ActionResult Index()
        {
            //ViewBag.Menus = MenuBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            ViewBag.Menus = from MenuPrincipal in MenuBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                            join MenuRaiz in MenuBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on MenuPrincipal.IDMenuSuperior equals MenuRaiz.IDMenu into prodGroup
                            from item in prodGroup.DefaultIfEmpty()
                            orderby MenuPrincipal.Ordem
                            select new Menu { IDMenu = MenuPrincipal.IDMenu, Nome = MenuPrincipal.Nome, Ordem = MenuPrincipal.Ordem, DataInclusao = MenuPrincipal.DataInclusao, UsuarioInclusao = MenuPrincipal.UsuarioInclusao, MenuSuperior = new Menu() { Nome = item == null ? string.Empty : item.Nome  } };

            return View();
        }

        private string BuscarMenuSuperior(Menu pMenu, string NomeMenuCompleto)
        {
            if (!string.IsNullOrEmpty(pMenu.IDMenuSuperior)) {
                Menu tMenu = MenuBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.IDMenu.Equals(pMenu.IDMenuSuperior));

                if (tMenu != null) {
                    NomeMenuCompleto = tMenu.Nome + "/" + NomeMenuCompleto;
                    BuscarMenuSuperior(tMenu, NomeMenuCompleto);
                }

            }
            return NomeMenuCompleto;
        }

        [MenuAtivo(MenuAtivo = "Administração/Menu")]
        public ActionResult Novo()
        {
            List<Menu> listaMenus = MenuBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();
            foreach (Menu iMenu in listaMenus) {
                iMenu.Nome = BuscarMenuSuperior(iMenu, iMenu.Nome);
            }

            ViewBag.Menus = new SelectList(listaMenus.OrderBy(p => p.Ordem), "IDMenu", "Nome");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Menu Menu)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    MenuBusiness.Inserir(Menu);

                    TempData["MensagemSucesso"] = "O menu '" + Menu.Nome + "' foi cadastrado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Menu") } });
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
            else
            {
                return Json(new { resultado = TratarRetornoValidacaoToJSON() });
            }
        }

        [MenuAtivo(MenuAtivo = "Administração/Menu")]
        public ActionResult Edicao(string id)
        {

            Menu oMenu = MenuBusiness.Consulta.FirstOrDefault(p => p.IDMenu.Equals(id));

            if (oMenu != null)
            {
                if (!string.IsNullOrEmpty(oMenu.IDMenuSuperior))
                {
                    ViewBag.Menus = new SelectList(MenuBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDMenu", "Nome", oMenu.IDMenuSuperior);
                }
                else
                {
                    ViewBag.Menus = new SelectList(MenuBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDMenu", "Nome");
                }
            }

            return View(oMenu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Menu Menu)
        {
           if (ModelState.IsValid)
            {
                try
                {
                    MenuBusiness.Alterar(Menu);

                    TempData["MensagemSucesso"] = "O menu '" + Menu.Nome + "' foi atualizado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Menu") } });
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
            else
            {
                return Json(new { resultado = TratarRetornoValidacaoToJSON() });
            }
        }

        [HttpPost]
        public ActionResult Terminar(string IDMenu)
        {

            try
            {
                Menu oMenu = MenuBusiness.Consulta.FirstOrDefault(p => p.IDMenu.Equals(IDMenu));
                if (oMenu == null)
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o menu, pois a mesmo não foi localizado." } });

                if (PerfilMenuBusiness.Consulta.Any(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.IDMenu.Equals(IDMenu)))
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o menu, pois a mesmo está vinculado a pelo menos um perfil." } });
                
                oMenu.DataExclusao = DateTime.Now;
                oMenu.UsuarioExclusao = "LoginTeste";
                MenuBusiness.Alterar(oMenu);

                return Json(new { resultado = new RetornoJSON() { Sucesso = "O menu '" + oMenu.Nome + "' foi excluído com sucesso." } });
                
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
        public ActionResult TerminarComRedirect(string IDMenu)
        {

            try
            {
                Menu oMenu = MenuBusiness.Consulta.FirstOrDefault(p => p.IDMenu.Equals(IDMenu));
                if (oMenu == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir a empresa, pois a mesma não foi localizada." } });
                }
                else
                {
                    oMenu.DataExclusao = DateTime.Now;
                    oMenu.UsuarioExclusao = "LoginTeste";

                    MenuBusiness.Alterar(oMenu);

                    TempData["MensagemSucesso"] = "O menu '" + oMenu.Nome + "' foi excluído com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Menu") } });
                }
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


        public RetornoJSON TratarRetornoValidacaoToJSON()
        {

            string msgAlerta = string.Empty;
            foreach (ModelState item in ModelState.Values)
            {
                if (item.Errors.Count > 0)
                {
                    foreach (System.Web.Mvc.ModelError i in item.Errors)
                    {
                        msgAlerta += i.ErrorMessage;
                    }
                }
            }

            return new RetornoJSON()
            {
                Alerta = msgAlerta,
                Erro = string.Empty,
                Sucesso = string.Empty
            };

        }

	}
}