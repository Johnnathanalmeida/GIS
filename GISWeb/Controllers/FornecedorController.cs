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
    public class FornecedorController : BaseController
    {

        #region Inject

            [Inject]
            public IFornecedorBusiness FornecedorBusiness { get; set; }

        #endregion

        [MenuAtivo(MenuAtivo = "Administracao/Fornecedor")]
        public ActionResult Index()
        {
            return View();
        }

        [MenuAtivo(MenuAtivo = "Administracao/Fornecedor")]
        public ActionResult Novo()
        {
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

	}
}