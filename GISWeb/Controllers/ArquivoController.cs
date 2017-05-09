using GISWeb.Infraestrutura.Filters;
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
    public class ArquivoController : BaseController
    {

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

                        string sLocalFile = Path.Combine(Path.GetTempPath(), ConfigurationManager.AppSettings["Web:NomeModulo"]);
                        sLocalFile = Path.Combine(sLocalFile, DateTime.Now.ToString("yyyyMMdd"));
                        //Após a autenticação está totalmente concluída, mudar para incluir uma pasta com o Login do usuário
                        sLocalFile = Path.Combine(sLocalFile, "LoginTeste");

                        if (!System.IO.Directory.Exists(sLocalFile))
                            Directory.CreateDirectory(sLocalFile);

                        sLocalFile = Path.Combine(sLocalFile, oFile.FileName);

                        if (!System.IO.File.Exists(sLocalFile))
                            oFile.SaveAs(sLocalFile);
                        else
                            throw new Exception("Já existe um arquivo com este nome no diretório temporário. Altere o nome do arquivo ou escolha outro.");
                       

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