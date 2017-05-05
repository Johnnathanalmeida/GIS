using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Ninject;
using GISCore.Business.Abstract;
using GISModel.Entidades;
using GISModel.DTO.Shared;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;

namespace GISWeb.Controllers
{
    [DadosUsuario]
    [Autorizador]
    public class AdmissaoController : Controller
    {
        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public IEmpregadoBusiness EmpregadoBusiness { get; set; }

        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        [Inject]
        public IAdmissaoBusiness AdmissaoBusiness { get; set; }

        [Inject]
        public IFornecedorBusiness FornecedorBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult CarregarEmpresas()
        {
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(new SelectList(EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDEmpresa", "NomeFantasia"));

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult CarregarDepartamentos(string IDEmpresa)
        {
            var jsonSerialiser = new JavaScriptSerializer();
            var json = jsonSerialiser.Serialize(new SelectList(DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.IDEmpresa.Equals(IDEmpresa)).ToList(), "IDDepartamento", "Sigla"));

            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Novo(string IDEmpregado)
        {
            try
            {
                Empregado oEmpregado = EmpregadoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.IDEmpregado.Equals(IDEmpregado));
                ViewBag.TipoEmpregado = oEmpregado.TipoEmpregado.ToString();

                if (oEmpregado.TipoEmpregado.ToString().Equals("Próprio"))
                {
                    ViewBag.Empresas = new SelectList(EmpresaBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDEmpresa", "NomeFantasia");
                    ViewBag.Departamentos = new SelectList(DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDDepartamento", "Sigla");
                }
                else
                {
                    ViewBag.Fornecedores = new SelectList(FornecedorBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList(), "IDFornecedor", "Nome");
                }

                Admissao objAdmissao = new Admissao();
                objAdmissao.IDEmpregado = IDEmpregado;

                return PartialView("Novo", objAdmissao);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Admissao oAdmissao)
        {
            try
            {
                oAdmissao.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                oAdmissao.DataAdmissao = DateTime.Now;
                oAdmissao.DataDemissao = DateTime.MaxValue;
                AdmissaoBusiness.Inserir(oAdmissao);

                TempData["MensagemSucesso"] = "Admissao foi cadastrada com sucesso.";

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

        [HttpPost]
        public ActionResult Demitir(string IDEmpregado)
        {

            try
            {
                Admissao oAdmissao = AdmissaoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.IDUsuarioDemissao) && p.IDEmpregado.Equals(IDEmpregado));
                if (oAdmissao == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível demitir o empregado, pois o mesmo não foi localizado." } });
                }
                else
                {
                    oAdmissao.DataExclusao = DateTime.Now;
                    oAdmissao.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    oAdmissao.IDUsuarioDemissao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    AdmissaoBusiness.Excluir(oAdmissao);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "O empregado foi demitido com sucesso." } });
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
    }
}