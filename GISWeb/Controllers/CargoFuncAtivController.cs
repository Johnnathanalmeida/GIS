using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using GISWeb.Infraestrutura.Filters;
using Ninject;
using GISCore.Business.Abstract;
using GISModel.Entidades;
using GISModel.DTO.Shared;
using GISWeb.Infraestrutura.Provider.Abstract;

namespace GISWeb.Controllers
{
    [DadosUsuario]
    [Autorizador]
    public class CargoFuncAtivController : BaseController
    {
        [Inject]
        public ICargoBusiness CargoBusiness { get; set; }

        [Inject]
        public IFuncaoBusiness FuncaoBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }


        [MenuAtivo(MenuAtivo = "Administracao/C.F.A")]
        public ActionResult Index()
        {
                        
            List<Cargo> listCargos = (from cg in CargoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                      orderby cg.Carg_Nome
                                      select new Cargo
                                      {
                                          UniqueKey = cg.UniqueKey,
                                          Carg_Nome = cg.Carg_Nome,
                                          Funcao = new List<Funcao>()
                                      }).ToList();

            foreach (Cargo item in listCargos)
            {

                item.Funcao = (from funcao in FuncaoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                               where funcao.UKCargo.Equals(item.UniqueKey)
                                select new Funcao()
                                {
                                    UKCargo = funcao.UKCargo,
                                    UniqueKey = funcao.UniqueKey,
                                    Func_Nome = funcao.Func_Nome
                                }
                                ).ToList();

            }

            ViewBag.Cargos = listCargos;

            return View();
        }

        [HttpPost]
        public ActionResult CadastrarCargo(string Cargo)
        {            
            try
            {
                Cargo oCargo = new Cargo();
                oCargo.Carg_Nome = Cargo;
                oCargo.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                CargoBusiness.Inserir(oCargo);

                TempData["MensagemSucesso"] = "O cargo '" + Cargo + "' foi cadastrado com sucesso.";

                return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "CargoFuncAtiv") } });
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
        public ActionResult DeletarCargo(string IDCargo)
        {
            try
            {
                Cargo oCargo = CargoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(IDCargo));
                if (oCargo == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o cargo, pois o mesmo não foi localizado." } });
                }
                else
                {
                    oCargo.DataExclusao = DateTime.Now;
                    oCargo.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    CargoBusiness.Excluir(oCargo);

                    TempData["MensagemAlerta"] = "O cargo '" + oCargo.Carg_Nome + "' foi excluído com sucesso.";

                    return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "CargoFuncAtiv") } });
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
        public ActionResult CadastrarFuncao(string IDCargo, string FuncaoNome)
        {
            try
            {
                Funcao oFuncao = new Funcao ();
                oFuncao.UKCargo = IDCargo;
                oFuncao.Func_Nome = FuncaoNome;
                oFuncao.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                FuncaoBusiness.Inserir(oFuncao);

                TempData["MensagemSucesso"] = "A função '" + FuncaoNome + "' foi cadastrado com sucesso.";

                return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "CargoFuncAtiv") } });
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