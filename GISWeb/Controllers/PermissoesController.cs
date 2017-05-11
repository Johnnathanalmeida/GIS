using GISCore.Business.Abstract;
using GISCore.Infraestrutura.Comparer;
using GISModel.DTO.Permissoes;
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
    public class PermissoesController : BaseController
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

            [Inject]
            public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        [MenuAtivo(MenuAtivo = "Administracao/Gerenciar Permissões")]
        public ActionResult Index()
        {
            ViewBag.Empresas = new SelectList(EmpresaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList(), "UniqueKey", "NomeFantasia");
            return View();
        }

        [HttpPost]
        public ActionResult SalvarPermissoes(bool Acao, string Perfil, string UIDsUsuarios, string IDArea)
        {
            try
            {
                if (Acao) { 
                    //Incluir permissão
                    if (UIDsUsuarios.Contains("|")) {
                        foreach (string IDUsuario in UIDsUsuarios.Split('|')) {
                            if (!string.IsNullOrEmpty(IDUsuario)) {
                                UsuarioPerfilBusiness.Inserir(new UsuarioPerfil() { UKArea = IDArea, UKUsuario = IDUsuario, UKPerfil = Perfil, UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login });        
                            }
                        }
                    }
                    else
                    {
                        UsuarioPerfilBusiness.Inserir(new UsuarioPerfil() { UKArea = IDArea, UKUsuario = UIDsUsuarios, UKPerfil = Perfil, UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login });
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
                                UsuarioPerfilBusiness.Alterar(new UsuarioPerfil() { UKArea = IDArea, UKUsuario = IDUsuario, UKPerfil = Perfil, DataExclusao = DateTime.Now, UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login });
                            }
                        }
                    }
                    else
                    {
                        UsuarioPerfilBusiness.Alterar(new UsuarioPerfil() { UKArea = IDArea, UKUsuario = UIDsUsuarios, UKPerfil = Perfil, DataExclusao = DateTime.Now, UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login });
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
        public ActionResult BuscarPermissoesPorEmpresa(string id)
        {
            try
            {

                if (CustomAuthorizationProvider.UsuarioAutenticado.Perfis.Where(p => p.Nome.Equals("Super Administrador")).Count() > 0)
                {
                    ViewBag.Perfis = PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();
                }
                else
                {
                    ViewBag.Perfis = PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && !p.Nome.Equals("Super Administrador")).ToList();
                }

                var lUsuarios = from usuarioperfil in UsuarioPerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKArea.Equals(id)).ToList()
                                join usr in UsuarioBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usuarioperfil.UKUsuario equals usr.UniqueKey
                                select new Usuario { Nome = usr.Nome, Login = usr.Login, UniqueKey = usr.UniqueKey };
                
                List<UsuarioPerfilViewModel> lUsuariosPerfis = new List<UsuarioPerfilViewModel>();
                foreach (Usuario iUsr in lUsuarios.ToList()) {
                    UsuarioPerfilViewModel oUsrPerfViewModel = new UsuarioPerfilViewModel() {
                        IDUsuario = iUsr.UniqueKey,
                        Login = iUsr.Login,
                        Nome = iUsr.Nome
                    };

                    var lPerfis = from usuarioperfil in UsuarioPerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKArea.Equals(id)).ToList()
                                  join perfil in PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usuarioperfil.UKPerfil equals perfil.UniqueKey
                                  where usuarioperfil.UKUsuario.Equals(iUsr.UniqueKey)
                                  select new Perfil { Nome = perfil.Nome, UniqueKey = perfil.UniqueKey };

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
        public ActionResult BuscarPermissoesPorDepartamento(string id)
        {
            try
            {
                if (CustomAuthorizationProvider.UsuarioAutenticado.Perfis.Where(p => p.Nome.Equals("Super Administrador")).Count() > 0)
                {
                    ViewBag.Perfis = PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();
                }
                else
                {
                    ViewBag.Perfis = PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && !p.Nome.Equals("Super Administrador")).ToList();
                }

                var lUsuariosComPermissao = from usuarioperfil in UsuarioPerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKArea.Equals(id)).ToList()
                                            join usr in UsuarioBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usuarioperfil.UKUsuario equals usr.UniqueKey
                                            select new Usuario { Nome = usr.Nome, Login = usr.Login, UniqueKey = usr.UniqueKey };

                List<Usuario> UsuariosPorDepartamento = UsuarioBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKDepartamento.Equals(id)).ToList();
                UsuariosPorDepartamento = UsuariosPorDepartamento.Union(lUsuariosComPermissao.ToList(), new UsuarioComparer()).ToList();

                List<UsuarioPerfilViewModel> lUsuariosPerfis = new List<UsuarioPerfilViewModel>();
                foreach (Usuario iUsr in UsuariosPorDepartamento)
                {
                    UsuarioPerfilViewModel oUsrPerfViewModel = new UsuarioPerfilViewModel()
                    {
                        IDUsuario = iUsr.UniqueKey,
                        Login = iUsr.Login,
                        Nome = iUsr.Nome
                    };

                    var lPerfis = from usuarioperfil in UsuarioPerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKArea.Equals(id)).ToList()
                                  join perfil in PerfilBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on usuarioperfil.UKPerfil equals perfil.UniqueKey
                                  where usuarioperfil.UKUsuario.Equals(iUsr.UniqueKey)
                                  select new Perfil { Nome = perfil.Nome, UniqueKey = perfil.UniqueKey };

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

	}
}