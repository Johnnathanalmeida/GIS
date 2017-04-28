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
    public class FornecedorController : BaseController
    {

        #region Inject

            [Inject]
            public IFornecedorBusiness FornecedorBusiness { get; set; }

            [Inject]
            public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

            [Inject]
            public IEmpresaBusiness EmpresaBusiness { get; set; }

        #endregion

        [MenuAtivo(MenuAtivo = "Administracao/Fornecedor")]
        public ActionResult Index()
        {
            ViewBag.Fornecedores = FornecedorBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            return View();
        }

        [MenuAtivo(MenuAtivo = "Administracao/Fornecedor")]
        public ActionResult Novo()
        {
            if (CustomAuthorizationProvider.UsuarioAutenticado.Perfis.Where(p => p.Nome.Equals("Super Administrador")).Count() > 0)
            {
                ViewBag.Perfil = "SuperAdministrador";
                ViewBag.Empresas = EmpresaBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList();
            }
            else if (CustomAuthorizationProvider.UsuarioAutenticado.Perfis.Where(p => p.Nome.Equals("Administrador")).Count() > 0)
            {
                ViewBag.Perfil = "Administrador";
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    fornecedor.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    FornecedorBusiness.Inserir(fornecedor);

                    TempData["MensagemSucesso"] = "O fornecedor '" + fornecedor.Nome + "' foi cadastrado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Fornecedor") } });
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

        [MenuAtivo(MenuAtivo = "Administracao/Fornecedor")]
        public ActionResult Edicao(string id)
        {
            return View(FornecedorBusiness.Consulta.FirstOrDefault(p => p.IDFornecedor.Equals(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Fornecedor fornecedor)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    FornecedorBusiness.Alterar(fornecedor);

                    TempData["MensagemSucesso"] = "O fornecedor '" + fornecedor.Nome + "' foi atualizado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Fornecedor") } });
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

        public ActionResult BuscarFornecedorPorID(string IDFornecedor)
        {

            try
            {
                Fornecedor oFornecedor = FornecedorBusiness.Consulta.FirstOrDefault(p => p.IDFornecedor.Equals(IDFornecedor));
                if (oFornecedor == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Alerta = "Fornecedor com o ID '" + IDFornecedor + "' não encontrado." } });
                }
                else
                {
                    return Json(new { data = RenderRazorViewToString("_Detalhes", oFornecedor) });
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
        public ActionResult Terminar(string IDFornecedor)
        {

            try
            {
                Fornecedor oFornecedor = FornecedorBusiness.Consulta.FirstOrDefault(p => p.IDFornecedor.Equals(IDFornecedor));
                if (oFornecedor == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir a empresa, pois a mesma não foi localizada." } });
                }
                else
                {

                    oFornecedor.DataExclusao = DateTime.Now;
                    oFornecedor.UsuarioExclusao = "LoginTeste";
                    FornecedorBusiness.Alterar(oFornecedor);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "O fornecedor '" + oFornecedor.Nome + "' foi excluído com sucesso." } });
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