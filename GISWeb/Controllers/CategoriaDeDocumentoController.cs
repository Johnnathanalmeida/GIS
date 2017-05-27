using GISWeb.Infraestrutura.Provider.Abstract;
using GISCore.Business.Abstract;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GISModel.Entidades;
using GISModel.DTO.Shared;
using GISWeb.Infraestrutura.Filters;


namespace GISWeb.Controllers
{
    [DadosUsuario]
    [Autorizador]
    public class CategoriaDeDocumentoController : BaseController
    {

        #region Inject

            [Inject]
            public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }

            [Inject]
            public IBaseBusiness<CategoriaDeDocumento> CategoriaDeDocumentoBusiness { get; set; }

            [Inject]
            public IBaseBusiness<TipoDeDocumento> TipoDeDocumentoBusiness { get; set; }
        #endregion

        [MenuAtivo(MenuAtivo = "Administracao/Tipos de documento")]
        public ActionResult Index()
        {
            List<CategoriaDeDocumento> listCategorias = (from cg in CategoriaDeDocumentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                      orderby cg.Nome
                                      select new CategoriaDeDocumento
                                      {
                                          ID = cg.ID,
                                          UniqueKey = cg.UniqueKey,
                                          Nome = cg.Nome,
                                          Tipos = new List<TipoDeDocumento>()
                                      }).ToList();

            foreach (CategoriaDeDocumento item in listCategorias)
            {
                item.Tipos = (from tipo in TipoDeDocumentoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                              where tipo.UKCategoriaDeDocumento.Equals(item.UniqueKey)
                              orderby tipo.Nome
                              select new TipoDeDocumento()
                              {
                                  ID = tipo.ID,
                                  UKCategoriaDeDocumento = tipo.UKCategoriaDeDocumento,
                                  UniqueKey = tipo.UniqueKey,
                                  Nome = tipo.Nome
                              }
                                ).ToList();
            }

            ViewBag.Categorias = listCategorias;
            return View();
        }


        [HttpPost]
        public ActionResult CadastrarCategoria(string Categoria)
        {
            try
            {
                CategoriaDeDocumento tempCategoria = CategoriaDeDocumentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Nome.Equals(Categoria));
                if (tempCategoria != null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "A Categoria '" + Categoria + "' já existe, favor informar outra Categoria." } });
                }

                CategoriaDeDocumento oCategoria = new CategoriaDeDocumento();
                oCategoria.Nome = Categoria;
                oCategoria.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                CategoriaDeDocumentoBusiness.Inserir(oCategoria);

                TempData["MensagemSucesso"] = "A Categoria '" + Categoria + "' foi cadastrada com sucesso.";

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

	}
}