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
            ViewBag.UKEmpresa = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.UKEmpresa;
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

        [HttpPost]
        public ActionResult AlterarCategoria(string UKCategoria, string CategoriaNome)
        {
            try
            {
                CategoriaDeDocumento oCategoria = CategoriaDeDocumentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKCategoria));
                if (oCategoria == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível alterar a Categoria, pois a mesma não foi localizada." } });
                }
                else
                {
                    CategoriaDeDocumento oNovaCategoria = CategoriaDeDocumentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(CategoriaNome));
                    if (oNovaCategoria != null)
                    {
                        return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível alterar a categoria '" + CategoriaNome + "'pois já existe uma categoria com este nome." } });
                    }

                    oCategoria.DataExclusao = DateTime.Now;
                    oCategoria.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;

                    CategoriaDeDocumento nCategoria = new CategoriaDeDocumento()
                    {
                        Nome = CategoriaNome,
                        UniqueKey = oCategoria.UniqueKey,
                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login
                    };

                    CategoriaDeDocumentoBusiness.Alterar(oCategoria);
                    CategoriaDeDocumentoBusiness.Inserir(nCategoria);
                    
                    TempData["MensagemSucesso"] = "A categoria '" + oCategoria.Nome + "' foi atualizada para '" + nCategoria.Nome + "' com sucesso.";

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
        public ActionResult DeletarCategoria(string IDCategoria)
        {
            try
            {
                CategoriaDeDocumento oCategoria = CategoriaDeDocumentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.ID.Equals(IDCategoria));
                if (oCategoria == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir a categoria, pois a mesma não foi localizada." } });
                }
                else
                {
                    oCategoria.DataExclusao = DateTime.Now;
                    oCategoria.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    CategoriaDeDocumentoBusiness.Alterar(oCategoria);

                    TempData["MensagemAlerta"] = "A categoria '" + oCategoria.Nome + "' foi excluída com sucesso.";

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