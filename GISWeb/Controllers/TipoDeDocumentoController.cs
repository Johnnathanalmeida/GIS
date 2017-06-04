using GISCore.Business.Abstract;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GISModel.DTO.Shared;

namespace GISWeb.Controllers
{
    public class TipoDeDocumentoController : Controller
    {

        #region Inject

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

        [Inject]
        public IBaseBusiness<TipoDeDocumento> TipoDeDocumentoBusiness { get; set; }
        
        #endregion

        public ActionResult Index()
        {
            return View();
        }


        public ActionResult Novo(string UKCategoria)
        {
            try
            {
                TipoDeDocumento objTipo = new TipoDeDocumento();
                objTipo.UKCategoriaDeDocumento = UKCategoria;
                ViewBag.Intervalos = TipoDeDocumentoBusiness.GetTodosEnumsIntervalo();

                return PartialView("Novo", objTipo);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }
        }

        public ActionResult Detalhes(string UKTipo)
        {
            try
            {
                TipoDeDocumento objTipo = TipoDeDocumentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKTipo));
                ViewBag.Intervalos = TipoDeDocumentoBusiness.GetTodosEnumsIntervalo();

                return PartialView("_Detalhes", objTipo);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }
        }

        public ActionResult Edicao(string UKTipo)
        {
            try
            {
                TipoDeDocumento objTipo = TipoDeDocumentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKTipo));
                ViewBag.Intervalos = TipoDeDocumentoBusiness.GetTodosEnumsIntervalo();

                return PartialView("Edicao", objTipo);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(TipoDeDocumento oTipo)
        {
            try
            {
                oTipo.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                TipoDeDocumentoBusiness.Inserir(oTipo);

                return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "CategoriaDeDocumento") } });
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
        public ActionResult Atualizar(TipoDeDocumento pTipo)
        {
            try
            {
                TipoDeDocumento oTipo = TipoDeDocumentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(pTipo.UniqueKey));
                if (oTipo == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível alterar o Tipo, pois o mesmo não foi localizado." } });
                }
                else
                {
                    TipoDeDocumento oNovoTipo = TipoDeDocumentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(pTipo.Nome));
                    if (oNovoTipo != null)
                    {
                        return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível alterar o Tipo '" + pTipo.Nome + "'pois já existe um Tipo com este nome." } });
                    }

                    oTipo.DataExclusao = DateTime.Now;
                    oTipo.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;

                    TipoDeDocumento nTipo = new TipoDeDocumento()
                    {
                        Nome = pTipo.Nome,
                        Obrigatorio = pTipo.Obrigatorio,
                        MascaraParaNomeclatura = pTipo.MascaraParaNomeclatura,
                        ExtensoesPermitidas = pTipo.ExtensoesPermitidas,
                        TamanhoMaximoEmMB = pTipo.TamanhoMaximoEmMB,
                        UKCategoriaDeDocumento = pTipo.UKCategoriaDeDocumento,
                        IntervaloVencimento = pTipo.IntervaloVencimento,
                        PrazoVencimento = pTipo.PrazoVencimento,
                        PermiteMultiplosArquivos = pTipo.PermiteMultiplosArquivos,
                        UniqueKey = oTipo.UniqueKey,
                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login
                    };

                    TipoDeDocumentoBusiness.Alterar(oTipo);
                    TipoDeDocumentoBusiness.Inserir(nTipo);

                    TempData["MensagemSucesso"] = "o Tipo '" + oTipo.Nome + "' foi atualizado para '" + nTipo.Nome + "' com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "CategoriaDeDocumento") } });
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
        public ActionResult DeletarTipo(string IDTipo)
        {
            try
            {
                TipoDeDocumento oTipo = TipoDeDocumentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.ID.Equals(IDTipo));
                if (oTipo == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o Tipo, pois o mesmo não foi localizado." } });
                }
                else
                {
                    oTipo.DataExclusao = DateTime.Now;
                    oTipo.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    TipoDeDocumentoBusiness.Alterar(oTipo);

                    TempData["MensagemAlerta"] = "O Tipo '" + oTipo.Nome + "' foi excluído com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "CategoriaDeDocumento") } });
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