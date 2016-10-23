using GISCore.Business.Abstract;
using GISModel.DTO.Empregado;
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

        [MenuAtivo(MenuAtivo = "Novo/Empregado/Proprios")]
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

        public ActionResult Edicao(string id)
        {
            return View(EmpregadoBusiness.Consulta.FirstOrDefault(p => p.IDEmpregado.Equals(id)));
        }

        [MenuAtivo(MenuAtivo = "Pesquisa/Empregado/Proprios")]
        public ActionResult PesquisaProprio() {
            ViewBag.Empresas = new SelectList(EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).OrderBy(p => p.NomeFantasia).ToList(), "IDEmpresa", "NomeFantasia");
            return View();
        }

        [MenuAtivo(MenuAtivo = "Pesquisa/Empregado/Terceirizados")]
        public ActionResult PesquisaTerceirizado()
        {
            ViewBag.Fornecedores = new SelectList(FornecedorBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).OrderBy(p => p.Nome).ToList(), "IDFornecedor", "Nome");
            return View();
        }

        [HttpPost]
        public ActionResult CarregarContratosPorFornecedor() {
            return Json(new { resultado = TratarRetornoValidacaoToJSON() });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PesquisarTerceirizados(PesquisaEmpregadoViewModel FornecedorContrato) {
          
            try
            {

                if (!string.IsNullOrEmpty(FornecedorContrato.Fornecedor) && string.IsNullOrEmpty(FornecedorContrato.Contrato))  {
                    List<Empregado> listaEmpregados = EmpregadoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.IDFornecedor.Equals(FornecedorContrato.Fornecedor)).ToList();
                    return Json(new { data = RenderRazorViewToString("_Terceirizados", listaEmpregados) });
                }
                else if (string.IsNullOrEmpty(FornecedorContrato.Fornecedor) && !string.IsNullOrEmpty(FornecedorContrato.Contrato)) {
                    return Json(new { data = RenderRazorViewToString("_Terceirizados") });
                }
                else if (!string.IsNullOrEmpty(FornecedorContrato.Fornecedor) && !string.IsNullOrEmpty(FornecedorContrato.Contrato)) {
                    return Json(new { data = RenderRazorViewToString("_Terceirizados") });
                }
                else {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "É necessário selecionar pelo menos um filtro." } });
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PesquisarProprios(PesquisaEmpregadoViewModel EmpresaContrato)
        {

            try
            {

                if (!string.IsNullOrEmpty(EmpresaContrato.Empresa) && string.IsNullOrEmpty(EmpresaContrato.Contrato))
                {
                    List<Empregado> listaEmpregados = EmpregadoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.IDEmpresa.Equals(EmpresaContrato.Empresa)).ToList();
                    return Json(new { data = RenderRazorViewToString("_Proprios", listaEmpregados) });
                }
                else if (string.IsNullOrEmpty(EmpresaContrato.Empresa) && !string.IsNullOrEmpty(EmpresaContrato.Contrato))
                {
                    return Json(new { data = RenderRazorViewToString("_Proprios") });
                }
                else if (!string.IsNullOrEmpty(EmpresaContrato.Empresa) && !string.IsNullOrEmpty(EmpresaContrato.Contrato))
                {
                    return Json(new { data = RenderRazorViewToString("_Proprios") });
                }
                else
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "É necessário selecionar pelo menos um filtro." } });
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Empregado empregado)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    empregado.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
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