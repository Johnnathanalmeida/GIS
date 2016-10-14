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
    public class ContratoController : BaseController
    {
        
        #region Inject

            [Inject]
            public IContratoBusiness ContratoBusiness { get; set; }

        #endregion

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Novo()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Contrato contrato)
        {
            if (ModelState.IsValid)
            {
                try
                {

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