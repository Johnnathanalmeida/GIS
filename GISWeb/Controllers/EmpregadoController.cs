using GISCore.Business.Abstract;
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
    public class EmpregadoController : BaseController
    {

        #region Inject

            [Inject]
            public IEmpregadoBusiness EmpregadoBusiness { get; set; }

            [Inject]
            public IEmpresaBusiness EmpresaBusiness { get; set; }

            [Inject]
            public IFornecedorBusiness FornecedorBusiness { get; set; }

            [Inject]
            public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        #endregion

        public ActionResult Index()
        {
            return View();
        }

        [MenuAtivo(MenuAtivo = "Novo/Empregado/Próprios")]
        public ActionResult NovoProprio()
        {
            ViewBag.Empresas = new SelectList(EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDEmpresa", "NomeFantasia");
            return View();
        }

        [MenuAtivo(MenuAtivo = "Novo/Empregado/Terceirizados")]
        public ActionResult NovoTerceirizado()
        {
            ViewBag.Fornecedores = new SelectList(FornecedorBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDFornecedor", "Nome");

            return View();
        }

        [MenuAtivo(MenuAtivo = "Administração/Empresa")]
        public ActionResult Edicao(string id)
        {
            return View(EmpregadoBusiness.Consulta.FirstOrDefault(p => p.IDEmpregado.Equals(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Empregado empregado)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    EmpregadoBusiness.Inserir(empregado);
                    return Json(new { resultado = new RetornoJSON() { Sucesso = "O empregado '" + empregado.Nome + "' foi cadastrado com sucesso." } });
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
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Empregado empregado)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    empregado.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    EmpregadoBusiness.Alterar(empregado);

                    TempData["MensagemSucesso"] = "O empregado '" + empregado.Nome + "' foi atualizado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Empregado") } });
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