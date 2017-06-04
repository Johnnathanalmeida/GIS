using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
using GISModel.DTO.TipoDeDocumento;
using GISModel.Entidades;
using GISWeb.Infraestrutura.Filters;
using GISWeb.Infraestrutura.Provider.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GISWeb.Controllers
{
    [DadosUsuario]
    [Autorizador]
    public class EstabelecimentoController : BaseController
    {

        #region Inject

            [Inject]
            public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

            [Inject]
            public IEstabelecimentoBusiness EstabelecimentoBusiness { get; set; }

            [Inject]
            public IBaseBusiness<CategoriaDeDocumento> CategoriaDeDocumentoBusiness { get; set; }

            [Inject]
            public IBaseBusiness<TipoDeDocumento> TipoDeDocumentoBusiness { get; set; }

            [Inject]
            public IArquivoBusiness ArquivoBusiness { get; set; }

        #endregion

        [MenuAtivo(MenuAtivo = "Administracao/Estabelecimentos")]
        public ActionResult Index()
        {
            ViewBag.Estabelecimentos = EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            return View();
        }

        [MenuAtivo(MenuAtivo = "Administracao/Estabelecimentos")]
        public ActionResult Novo()
        {

            ViewBag.TiposDeDocumento = (from cat in CategoriaDeDocumentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Nome.Equals("Estabelecimento")).ToList()
                                        join tip in TipoDeDocumentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on cat.UniqueKey equals tip.UKCategoriaDeDocumento
                                        select new TipoDeDocumentoComArquivosViewModel()
                                        {
                                            UniqueKey = tip.UniqueKey,
                                            Nome = tip.Nome,
                                            Obrigatorio = tip.Obrigatorio,
                                            ExtensoesPermitidas = tip.ExtensoesPermitidas,
                                            TamanhoMaximoEmMB = tip.TamanhoMaximoEmMB,
                                            MascaraParaNomeclatura = tip.MascaraParaNomeclatura
                                        }).ToList();

            return View();
        }

        [MenuAtivo(MenuAtivo = "Administracao/Estabelecimentos")]
        public ActionResult Edicao(string id)
        {
            return View(EstabelecimentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(id)));
        }

        public ActionResult Cadastrar(Estabelecimento entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entidade.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    EstabelecimentoBusiness.Inserir(entidade);

                    TempData["MensagemSucesso"] = "O estabelecimento '" + entidade.Nome + "' foi cadastrado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Estabelecimento") } });
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
        public ActionResult Terminar(string IDEstabelecimento)
        {

            try
            {
                Estabelecimento oEstabelecimento = EstabelecimentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(IDEstabelecimento));
                if (oEstabelecimento == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o estabelecimento, pois o mesmo não foi localizado." } });
                }
                else
                {
                    
                    oEstabelecimento.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    EstabelecimentoBusiness.Terminar(oEstabelecimento);

                    return Json(new { resultado = new RetornoJSON() { Sucesso = "O estabelecimento '" + oEstabelecimento.Nome + "' foi excluído com sucesso." } });
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
        public ActionResult TerminarComRedirect(string IDEstabelecimento)
        {

            try
            {
                Estabelecimento oEstabelecimento = EstabelecimentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(IDEstabelecimento));
                if (oEstabelecimento == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o estabelecimento, pois o mesmo não foi localizado." } });
                }
                else
                {
                    oEstabelecimento.DataExclusao = DateTime.Now;
                    oEstabelecimento.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;

                    EstabelecimentoBusiness.Alterar(oEstabelecimento);

                    TempData["MensagemSucesso"] = "O estabelecimento '" + oEstabelecimento.Nome + "' foi excluído com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Estabelecimento") } });
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
        public ActionResult Atualizar(Estabelecimento entidade)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    entidade.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    EstabelecimentoBusiness.Alterar(entidade);

                    TempData["MensagemSucesso"] = "O estabelecimento '" + entidade.Nome + "' foi atualizado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Estabelecimento") } });
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

        [RestritoAAjax]
        public ActionResult _Upload(string UKTipoDeDocumento)
        {
            try
            {
                TipoDeDocumento oTipo = TipoDeDocumentoBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.UniqueKey.Equals(UKTipoDeDocumento));
                return PartialView("_Upload", oTipo);
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }
        }

        [HttpPost]
        [RestritoAAjax]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(string UKTipoDeDocumento, string NomeTipoDeDocumento)
        {
            try
            {
                string fName = string.Empty;

                List<TipoDeDocumentoComArquivosViewModel> lista = new List<TipoDeDocumentoComArquivosViewModel>();

                TipoDeDocumentoComArquivosViewModel DTOTipoDeDocumento = new TipoDeDocumentoComArquivosViewModel();
                DTOTipoDeDocumento.UniqueKey = UKTipoDeDocumento;
                DTOTipoDeDocumento.Nome = NomeTipoDeDocumento;
                DTOTipoDeDocumento.Arquivos = new List<Arquivo>();

                List<string> Arquivos = new List<string>();
                
                foreach (string fileName in Request.Files.AllKeys)
                {
                    HttpPostedFileBase oFile = Request.Files[fileName];
                    fName = oFile.FileName;
                    if (oFile != null)
                    {
                        string sLocalFile = Path.Combine(Path.GetTempPath(), ConfigurationManager.AppSettings["Web:NomeModulo"]);
                        sLocalFile = Path.Combine(sLocalFile, DateTime.Now.ToString("yyyyMMdd"));
                        sLocalFile = Path.Combine(sLocalFile, "Estabelecimento");
                        sLocalFile = Path.Combine(sLocalFile, CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login);
                        sLocalFile = Path.Combine(sLocalFile, Guid.NewGuid().ToString());

                        if (!System.IO.Directory.Exists(sLocalFile))
                            Directory.CreateDirectory(sLocalFile);

                        Arquivos.Add(Path.Combine(sLocalFile, oFile.FileName));

                        oFile.SaveAs(Path.Combine(sLocalFile, oFile.FileName));
                    }
                }

                foreach (string iArq in Arquivos) {
                    DTOTipoDeDocumento.Arquivos.Add(new Arquivo()
                    {
                        NomeRemoto = iArq,
                        NomeLocal = iArq.Substring(iArq.LastIndexOf(@"\") + 1)
                    });
                }

                lista.Add(DTOTipoDeDocumento);

                return Json(new { sucesso = "O upload do arquivo '" + fName + "' foi realizado com êxito.", UKTipoDeDocumento = UKTipoDeDocumento, data = RenderRazorViewToString("_TipoDeDocWidget", lista) });
                
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }

	}
}