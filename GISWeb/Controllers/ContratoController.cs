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
    public class ContratoController : BaseController
    {
        
        #region Inject

            [Inject]
            public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

            [Inject]
            public IContratoBusiness ContratoBusiness { get; set; }

            [Inject]
            public IFornecedorBusiness FornecedorBusiness { get; set; }

            [Inject]
            public IEmpresaBusiness EmpresaBusiness { get; set; }

            [Inject]
            public IEmpresaFornecedorBusiness EmpresaFornecedorBusiness { get; set; }

            [Inject]
            public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        #endregion

        [MenuAtivo(MenuAtivo = "Administracao/Contrato")]
        public ActionResult Index()
        {
            ViewBag.Contratos = ContratoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            return View();
        }

        [MenuAtivo(MenuAtivo = "Administracao/Contrato")]
        public ActionResult Novo()
        {

            if (CustomAuthorizationProvider.UsuarioAutenticado.Perfis.Where(p => p.Nome.Equals("Super Administrador")).Count() > 0) {
                ViewBag.Perfil = "SuperAdministrador";
                ViewBag.Empresas = new SelectList(EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).OrderBy(o => o.NomeFantasia).ToList(), "IDEmpresa", "NomeFantasia");
            }
            else if (CustomAuthorizationProvider.UsuarioAutenticado.Perfis.Where(p => p.Nome.Equals("Administrador")).Count() > 0)
            { 
                ViewBag.Perfil = "Administrador";

                //ViewBag.Fornecedores = new SelectList(EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDEmpresa", "NomeFantasia");
                //ViewBag.Departamentos = new SelectList(EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDEmpresa", "NomeFantasia");    
            }

            return View();
        }

        [HttpPost]
        public ActionResult CarregarFornecedoresEDepartamentosPorEmpresa(string IDEmpresa) {

            try
            {
                List<EmpresaFornecedor> listaFornecedores = EmpresaFornecedorBusiness.Consulta.Where(p => p.Empresa.IDEmpresa.Equals(IDEmpresa) && string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

                List<Departamento> listaDepartamentos = DepartamentoBusiness.Consulta.Where(p => p.Empresa.IDEmpresa.Equals(IDEmpresa)).OrderBy(o => o.Sigla).ToList();

                return Json(new { fornecedores = listaFornecedores, departamentos = listaDepartamentos });
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
        public ActionResult Cadastrar(Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    contrato.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    ContratoBusiness.Inserir(contrato);

                    TempData["MensagemSucesso"] = "O contrato '" + contrato.Numero + "' foi cadastrado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Contrato") } });
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

        [MenuAtivo(MenuAtivo = "Administracao/Contrato")]
        public ActionResult Edicao(string id)
        {
            return View(ContratoBusiness.Consulta.FirstOrDefault(p => p.IDContrato.Equals(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ContratoBusiness.Alterar(contrato);

                    TempData["MensagemSucesso"] = "O contrato '" + contrato.Numero + "' foi atualizado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Contrato") } });
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