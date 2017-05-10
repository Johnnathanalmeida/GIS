using GISCore.Business.Abstract;
using GISModel.DTO.Contrato;
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
            public IDepartamentoBusiness DepartamentoBusiness { get; set; }

            [Inject]
            public IDepartamentoContratoBusiness DepartamentoContratoBusiness { get; set; }

            [Inject]
            public IBaseBusiness<Garantia> BaseBusiness { get; set; }

        #endregion

        [MenuAtivo(MenuAtivo = "Administracao/Contrato")]
        public ActionResult Index()
        {
            //ViewBag.Contratos = ContratoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();
            List<Contrato> contratos = (from cont in ContratoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                        join forn in FornecedorBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList() on cont.UKFornecedor equals forn.UniqueKey
                                        select new Contrato()
                                        {
                                            ID = cont.ID,
                                            UniqueKey = cont.UniqueKey,
                                            Inicio = cont.Inicio,
                                            Fim = cont.Fim,
                                            Numero = cont.Numero,
                                            Descricao = cont.Descricao,
                                            Fornecedor = new Fornecedor() { UniqueKey = forn.UniqueKey, Nome = forn.Nome, CNPJ = forn.CNPJ },
                                            Departamentos = new List<Departamento>()
                                        }).ToList();

            foreach (Contrato item in contratos) {

                item.Departamentos = (from contdep in DepartamentoContratoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                                      join dep in DepartamentoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList() on contdep.UKDepartamento equals dep.UniqueKey
                                      where contdep.UKContrato.Equals(item.UniqueKey)
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
                        if (sExtensao.ToUpper().Contains("PDF") || sExtensao.ToUpper().Contains("DOC") || sExtensao.ToUpper().Contains("DOCX"))
                        {
                            //Após a autenticação está totalmente concluída, mudar para incluir uma pasta com o Login do usuário
                            string sLocalFile = Path.Combine(Path.GetTempPath(), ConfigurationManager.AppSettings["Web:NomeModulo"]);
                            sLocalFile = Path.Combine(sLocalFile, DateTime.Now.ToString("yyyyMMdd"));
                            sLocalFile = Path.Combine(sLocalFile, "Contrato");
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

        //[HttpPost]
        //public ActionResult CarregarFornecedoresEDepartamentosPorEmpresa(string IDEmpresa) {

        //    try
        //    {
        //        List<EmpresaFornecedor> listaFornecedores = EmpresaFornecedorBusiness.Consulta.Where(p => p.Empresa.UniqueKey.Equals(IDEmpresa) && string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();

        //        List<Departamento> listaDepartamentos = DepartamentoBusiness.Consulta.Where(p => p.Empresa.UniqueKey.Equals(IDEmpresa) && string.IsNullOrEmpty(p.UsuarioExclusao)).OrderBy(o => o.Sigla).ToList();

        //        return Json(new { fornecedores = listaFornecedores, departamentos = listaDepartamentos });
        //    }
        //    catch (Exception ex)
        //    {
        //        if (ex.GetBaseException() == null)
        //        {
        //            return Json(new { resultado = new RetornoJSON() { Erro = ex.Message } });
        //        }
        //        else
        //        {
        //            return Json(new { resultado = new RetornoJSON() { Erro = ex.GetBaseException().Message } });
        //        }
        //    }

        //}

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
                            UniqueKey = Guid.NewGuid().ToString(),
                            Numero = contrato.Numero,
                            Inicio = DateTime.ParseExact(contrato.Inicio, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                            Fim = DateTime.ParseExact(contrato.Fim, "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture),
                            Descricao = contrato.Descricao,
                            UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login,
                            UKFornecedor = forn.UniqueKey
                            //, Arquivo = new Arquivo() { NomeLocal = contrato.NomeArquivoLocal, NomeRemoto = Guid.NewGuid().ToString() + contrato.NomeArquivoLocal.Substring(contrato.NomeArquivoLocal.LastIndexOf(".")) }
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
                                            UniqueKey = Guid.NewGuid().ToString(),
                                            UKContrato = obj.UniqueKey,
                                            UKDepartamento = dep.UniqueKey,
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
                                    UniqueKey = Guid.NewGuid().ToString(),
                                    UKContrato = obj.UniqueKey,
                                    UKDepartamento = dep.UniqueKey,
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
            return View(ContratoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(id)));
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