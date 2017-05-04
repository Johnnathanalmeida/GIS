using GISCore.Business.Abstract;
using GISModel.DTO.Contrato;
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

            [Inject]
            public IDepartamentoContratoBusiness DepartamentoContratoBusiness { get; set; }

        #endregion

        [MenuAtivo(MenuAtivo = "Administracao/Contrato")]
        public ActionResult Index()
        {
            //ViewBag.Contratos = ContratoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();
            List<Contrato> contratos = (from cont in ContratoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        join forn in FornecedorBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on cont.IDFornecedor equals forn.IDFornecedor
                                        select new Contrato()
                                        {
                                            ID = cont.ID,
                                            IDContrato = cont.IDContrato,
                                            Inicio = cont.Inicio,
                                            Fim = cont.Fim,
                                            Numero = cont.Numero,
                                            Descricao = cont.Descricao,
                                            Fornecedor = new Fornecedor() { IDFornecedor = forn.IDFornecedor, Nome = forn.Nome, CNPJ = forn.CNPJ },
                                            Departamentos = new List<Departamento>()
                                        }).ToList();

            foreach (Contrato item in contratos) {

                item.Departamentos = (from contdep in DepartamentoContratoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                                      join dep in DepartamentoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList() on contdep.IDDepartamento equals dep.IDDepartamento
                                      where contdep.IDContrato.Equals(item.IDContrato)
                                      select new Departamento() { 
                                        Sigla = dep.Sigla,
                                        Codigo = dep.Codigo
                                      }
                                     ).ToList();

            }

            ViewBag.Contratos = contratos;

            return View();
        }

        [MenuAtivo(MenuAtivo = "Administracao/Contrato")]
        public ActionResult Novo()
        {
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
        public ActionResult Cadastrar(CadastroViewModel contrato)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Fornecedor forn = null;
                    if (contrato.IDFornecedor.Contains(" - "))
                    {
                        forn = FornecedorBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.CNPJ.ToUpper().Equals(contrato.IDFornecedor.Substring(contrato.IDFornecedor.IndexOf(" - ") + 3).ToUpper()));
                    }
                    else {
                        forn = FornecedorBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.CNPJ.ToUpper().Equals(contrato.IDFornecedor.Trim().ToUpper()));
                    }

                    if (forn == null)
                    {
                        throw new Exception("Não foi possível localizar o fornecedor através do CNPJ.");
                    }
                    else {

                        Contrato obj = new Contrato()
                        {
                            IDContrato = Guid.NewGuid().ToString(),
                            Numero = contrato.Numero,
                            Inicio = DateTime.ParseExact(contrato.Inicio, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                            Fim = DateTime.ParseExact(contrato.Fim, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                            Descricao = contrato.Descricao,
                            UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login,
                            IDFornecedor = forn.IDFornecedor
                        };

                        if (contrato.Departamentos.Contains(","))
                        {
                            foreach (string str in contrato.Departamentos.Split(','))
                            {
                                if (!string.IsNullOrEmpty(str.Trim()) && !str.Trim().Equals(","))
                                {
                                    Departamento dep = null;
                                    if (str.Contains(" - "))
                                        dep = DepartamentoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Codigo.ToUpper().Equals(str.Substring(0, str.IndexOf(" - ")).ToUpper().Trim()));
                                    else
                                        dep = DepartamentoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Sigla.ToUpper().Trim().Equals(str.Trim().ToUpper()));

                                    if (dep != null)
                                    {
                                        DepartamentoContrato dc = new DepartamentoContrato()
                                        {
                                            IDDepartamentoContrato = Guid.NewGuid().ToString(),
                                            IDContrato = obj.IDContrato,
                                            IDDepartamento = dep.IDDepartamento,
                                            UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login
                                        };
                                        DepartamentoContratoBusiness.Inserir(dc);
                                    }

                                }
                            }
                        }
                        else {
                            Departamento dep = null;
                            if (contrato.Departamentos.Contains(" - "))
                                dep = DepartamentoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Codigo.ToUpper().Equals(contrato.Departamentos.Substring(0, contrato.Departamentos.IndexOf(" - ")).ToUpper().Trim()));
                            else
                                dep = DepartamentoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Sigla.ToUpper().Trim().Equals(contrato.Departamentos.Trim().ToUpper()));

                            if (dep != null)
                            {
                                DepartamentoContrato dc = new DepartamentoContrato()
                                {
                                    IDDepartamentoContrato = Guid.NewGuid().ToString(),
                                    IDContrato = obj.IDContrato,
                                    IDDepartamento = dep.IDDepartamento,
                                    UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login
                                };
                                DepartamentoContratoBusiness.Inserir(dc);
                            }
                            else {
                                throw new Exception("Departamento informado '" + contrato.Departamentos + "' não foi encontrado.");
                            }
                        }

                        //Finalização com a inserção do contrato
                        ContratoBusiness.Inserir(obj);

                        //Tratar Garantias

                    }

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