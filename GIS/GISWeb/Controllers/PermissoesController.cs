using GISCore.Business.Abstract;
using GISModel.DTO.Permissoes;
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
    public class PermissoesController : Controller
    {

        #region Inject

            [Inject]
            public IUsuarioPerfilBusiness UsuarioPerfilBusiness { get; set; }

            [Inject]
            public IUsuarioBusiness UsuarioBusiness { get; set; }

            [Inject]
            public IPerfilBusiness PerfilBusiness { get; set; }

            [Inject]
            public IEmpresaBusiness EmpresaBusiness { get; set; }

        #endregion

        public ActionResult Index()
        {
            ViewBag.Empresas = new SelectList(EmpresaBusiness.Consulta.ToList(), "IDEmpresa", "NomeFantasia");
            return View();
        }

        [HttpPost]
        public ActionResult SalvarPermissoes(bool Acao, string Perfil, string UIDsUsuarios, string Orgao, string Empresa)
        {
            try
            {
                if (Acao) { 
                    //Incluir permissão
                    if (UIDsUsuarios.Contains("|")) {
                        foreach (string IDUsuario in UIDsUsuarios.Split('|')) {
                            if (!string.IsNullOrEmpty(IDUsuario)) {
                                UsuarioPerfilBusiness.Inserir(new UsuarioPerfil() { IDUsuario = IDUsuario, IDPerfil = Perfil });        
                            }
                        }
                    }
                    else
                    {
                        UsuarioPerfilBusiness.Inserir(new UsuarioPerfil() { IDUsuario = UIDsUsuarios, IDPerfil = Perfil });
                    }
                }
                else
                {
                    //Remover permissão
                    if (UIDsUsuarios.Contains("|"))
                    {
                        foreach (string IDUsuario in UIDsUsuarios.Split('|'))
                        {
                            if (!string.IsNullOrEmpty(IDUsuario))
                            {
                                UsuarioPerfilBusiness.Alterar(new UsuarioPerfil() { IDUsuario = IDUsuario, IDPerfil = Perfil, DataExclusao = DateTime.Now, UsuarioExclusao = "LoginTeste" });
                            }
                        }
                    }
                    else
                    {
                        UsuarioPerfilBusiness.Alterar(new UsuarioPerfil() { IDUsuario = UIDsUsuarios, IDPerfil = Perfil, DataExclusao = DateTime.Now, UsuarioExclusao = "LoginTeste" });
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
        public ActionResult BuscarUsuariosPorEmpresa(string id)
        {
            try
            {

                ViewBag.Perfis = PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

                List<Usuario> lUsuarios = UsuarioBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.IDEmpresa.Equals(id)).ToList();

                List<UsuarioPerfilViewModel> lUsuariosPerfis = new List<UsuarioPerfilViewModel>();
                foreach (Usuario iUsr in lUsuarios) {
                    UsuarioPerfilViewModel oUsrPerfViewModel = new UsuarioPerfilViewModel() {
                        IDUsuario = iUsr.IDUsuario,
                        Login = iUsr.Login,
                        Nome = iUsr.Nome
                    };

                    var lPerfis = from usuarioperfil in UsuarioPerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                  join perfil in PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usuarioperfil.IDPerfil equals perfil.IDPerfil
                                  where usuarioperfil.IDUsuario.Equals(iUsr.IDUsuario)
                                  select new Perfil { Nome = perfil.Nome, IDPerfil = perfil.IDPerfil };

                    oUsrPerfViewModel.Perfis = lPerfis.ToList();

                    lUsuariosPerfis.Add(oUsrPerfViewModel);

                }

                return Json(new { data = RenderRazorViewToString("_UsuariosPerfis", lUsuariosPerfis), usuarios = lUsuariosPerfis.Count, colunas = ViewBag.Perfis.Count + 2 });
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
        public ActionResult BuscarUsuariosPorDepartamento(string id)
        {
            try
            {
                ViewBag.Perfis = PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

                List<Usuario> lUsuarios = UsuarioBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.IDDepartamento.Equals(id)).ToList();

                List<UsuarioPerfilViewModel> lUsuariosPerfis = new List<UsuarioPerfilViewModel>();
                foreach (Usuario iUsr in lUsuarios)
                {
                    UsuarioPerfilViewModel oUsrPerfViewModel = new UsuarioPerfilViewModel()
                    {
                        IDUsuario = iUsr.IDUsuario,
                        Login = iUsr.Login,
                        Nome = iUsr.Nome
                    };

                    var lPerfis = from usuarioperfil in UsuarioPerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                  join perfil in PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usuarioperfil.IDPerfil equals perfil.IDPerfil
                                  where usuarioperfil.IDUsuario.Equals(iUsr.IDUsuario)
                                  select new Perfil { Nome = perfil.Nome, IDPerfil = perfil.IDPerfil };

                    oUsrPerfViewModel.Perfis = lPerfis.ToList();

                    lUsuariosPerfis.Add(oUsrPerfViewModel);

                }

                return Json(new { data = RenderRazorViewToString("_UsuariosPerfis", lUsuariosPerfis), usuarios = lUsuariosPerfis.Count, colunas = ViewBag.Perfis.Count + 2 });
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