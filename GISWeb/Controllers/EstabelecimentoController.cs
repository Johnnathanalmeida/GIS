﻿using GISCore.Business.Abstract;
using GISModel.DTO.Shared;
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

        #endregion

        [MenuAtivo(MenuAtivo = "Administracao/Estabelecimento")]
        public ActionResult Index()
        {
            ViewBag.Estabelecimentos = EstabelecimentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

            return View();
        }

        [MenuAtivo(MenuAtivo = "Administracao/Estabelecimento")]
        public ActionResult Novo()
        {
            return View();
        }

        [MenuAtivo(MenuAtivo = "Administracao/Estabelecimento")]
        public ActionResult Edicao(string id)
        {
            return View(EstabelecimentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

                    oEstabelecimento.DataExclusao = DateTime.Now;
                    oEstabelecimento.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    EstabelecimentoBusiness.Alterar(oEstabelecimento);

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
                    EstabelecimentoBusiness.Alterar(entidade);

                    TempData["MensagemSucesso"] = "O estabelecimento '" + entidade.Nome + "' foi atualizado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Empresa") } });
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
        public ActionResult _Upload()
        {
            try
            {
                return PartialView("_Upload");
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
        public ActionResult Upload()
        {
            try
            {
                string fName = string.Empty;
                string msgErro = string.Empty;
                foreach (string fileName in Request.Files.AllKeys)
                {
                    HttpPostedFileBase oFile = Request.Files[fileName];
                    fName = oFile.FileName;
                    if (oFile != null)
                    {
                        string sExtensao = oFile.FileName.Substring(oFile.FileName.LastIndexOf("."));
                        if (sExtensao.ToUpper().Contains("JPG") || sExtensao.ToUpper().Contains("JPEG") || sExtensao.ToUpper().Contains("PNG") || sExtensao.ToUpper().Contains("GIF"))
                        {
                            //Após a autenticação está totalmente concluída, mudar para incluir uma pasta com o Login do usuário
                            string sLocalFile = Path.Combine(Path.GetTempPath(), ConfigurationManager.AppSettings["Web:NomeModulo"]);
                            sLocalFile = Path.Combine(sLocalFile, DateTime.Now.ToString("yyyyMMdd"));
                            sLocalFile = Path.Combine(sLocalFile, "Estabelecimento");
                            sLocalFile = Path.Combine(sLocalFile, CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login);

                            if (!System.IO.Directory.Exists(sLocalFile))
                                Directory.CreateDirectory(sLocalFile);
                            else
                            {
                                //Tratamento de limpar arquivos da pasta, pois o usuário pode estar apenas alterando o arquivo.
                                //Limpar para não ficar lixo.
                                //O arquivo que for salvo abaixo será limpado após o cadastro.
                                //Se o usuário cancelar o cadastro, a rotina de limpar diretórios ficará responsável por limpá-lo.
                                foreach (string iFile in System.IO.Directory.GetFiles(sLocalFile))
                                {
                                    System.IO.File.Delete(iFile);
                                }
                            }

                            sLocalFile = Path.Combine(sLocalFile, oFile.FileName);

                            oFile.SaveAs(sLocalFile);

                        }
                        else
                        {
                            throw new Exception("Extensão do arquivo não permitida.");
                        }

                    }
                }
                if (string.IsNullOrEmpty(msgErro))
                    return Json(new { sucesso = "O upload do arquivo '" + fName + "' foi realizado com êxito.", arquivo = fName, erro = msgErro });
                else
                    return Json(new { erro = msgErro });
            }
            catch (Exception ex)
            {
                return Json(new { erro = ex.Message });
            }
        }

	}
}