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
    public class UsuarioController : BaseController
    {

        #region Inject

            [Inject]
            public IEmpresaBusiness EmpresaBusiness { get; set; }

            [Inject]
            public IUsuarioBusiness UsuarioBusiness { get; set; }

            [Inject]
            public IDepartamentoBusiness DepartamentoBusiness { get; set; }

        #endregion

        [MenuAtivo(MenuAtivo = "Administracao/Usuário")]
        public ActionResult Index()
        {
            ViewBag.Usuarios = UsuarioBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList();
            return View();
        }

        [MenuAtivo(MenuAtivo = "Administracao/Usuário")]
        public ActionResult Novo()
        {
            ViewBag.Empresas = new SelectList(EmpresaBusiness.Consulta.ToList(), "IDEmpresa", "NomeFantasia");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Cadastrar(Usuario Usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    UsuarioBusiness.Inserir(Usuario);

                    TempData["MensagemSucesso"] = "O usuário '" + Usuario.Nome + "' foi cadastrado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Usuario") } });
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

        [MenuAtivo(MenuAtivo = "Administracao/Usuário")]
        public ActionResult Edicao(string id)
        {
            return View(UsuarioBusiness.Consulta.FirstOrDefault(p => p.IDUsuario.Equals(id)));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Atualizar(Usuario Usuario)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    UsuarioBusiness.Alterar(Usuario);

                    TempData["MensagemSucesso"] = "O usuário '" + Usuario.Nome + "' foi atualizado com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "Usuario") } });
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