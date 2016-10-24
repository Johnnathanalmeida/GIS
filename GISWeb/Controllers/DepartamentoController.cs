using GISCore.Business.Abstract;
using GISModel.DTO.Departamento;
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
    public class DepartamentoController : BaseController
    {

        #region Inject

            [Inject]
            public IEmpresaBusiness EmpresaBusiness { get; set; }

            [Inject]
            public IDepartamentoBusiness DepartamentoBusiness { get; set; }

            [Inject]
            public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        [MenuAtivo(MenuAtivo = "Administracao/Departamento")]
        public ActionResult Index()
        {
            ViewBag.Departamentos = DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();
            return View();
        }

        [MenuAtivo(MenuAtivo = "Administracao/Departamento")]
        public ActionResult Novo()
        {
            ViewBag.Empresas = new SelectList(EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDEmpresa", "NomeFantasia");
            return View();
        }

        public ActionResult ListarDepartamentosPorEmpresa(string idEmpresa) {
            return Json(new { resultado = DepartamentoBusiness.Consulta.Where(p => p.IDEmpresa.Equals(idEmpresa) && string.IsNullOrEmpty(p.UsuarioExclusao)).ToList().OrderBy(p=>p.Sigla) });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Departamento Departamento)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    bool bRedirect = false;
                    if (Departamento.IDDepartamento != null && Departamento.IDDepartamento.Equals("redirect"))
                        bRedirect = true;

                    Departamento.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    DepartamentoBusiness.Inserir(Departamento);

                    if (bRedirect)
                    {
                        TempData["MensagemSucesso"] = "O departamento '" + Departamento.Sigla + "' foi cadastrado com sucesso.";
                        return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Departamento") } });
                    }
                    else {
                        return Json(new { resultado = new RetornoJSON() { Sucesso = "O departamento '" + Departamento.Sigla + "' foi cadastrado com sucesso." } });
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
            else
            {
                return Json(new { resultado = TratarRetornoValidacaoToJSON() });
            }
        }

        [MenuAtivo(MenuAtivo = "Administracao/Departamento")]
        public ActionResult Edicao(string id)
        {
            ViewBag.Empresas = new SelectList(EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDEmpresa", "NomeFantasia");

            return View(DepartamentoBusiness.Consulta.FirstOrDefault(p => p.IDDepartamento.Equals(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Departamento Departamento)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Departamento.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    DepartamentoBusiness.Alterar(Departamento);

                    TempData["MensagemSucesso"] = "O departamento '" + Departamento.Sigla + "' foi atualizado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Departamento") } });
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


	}
}