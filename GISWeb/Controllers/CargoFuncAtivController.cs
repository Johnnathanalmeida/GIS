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
        public IAtividadeBusiness AtividadeBusiness { get; set; }

        [Inject]
        public IFuncaoAtividadeBusiness FuncaoAtividadeBusiness { get; set; }

        [Inject]
        public ICustomAuthorizationProvider CustomAuthorizationProvider { get; set; }


        [MenuAtivo(MenuAtivo = "Administracao/Cargos")]
        public ActionResult Index()
        {
                        
            List<Cargo> listCargos = (from cg in CargoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                      orderby cg.Carg_Nome
                                      select new Cargo
                                      {
                                          ID = cg.ID,
                                          UniqueKey = cg.UniqueKey,
                                          Carg_Nome = cg.Carg_Nome,
                                          Funcao = new List<Funcao>()
                                      }).ToList();

            foreach (Cargo item in listCargos)
            {

                item.Funcao = (from funcao in FuncaoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                               where funcao.UKCargo.Equals(item.UniqueKey)
                               orderby funcao.Func_Nome
                                select new Funcao()
                                {
                                    ID = funcao.ID,
                                    UKCargo = funcao.UKCargo,
                                    UniqueKey = funcao.UniqueKey,
                                    Func_Nome = funcao.Func_Nome
                                }
                                ).ToList();

                foreach (Funcao iFuncao in item.Funcao)
                {
                    iFuncao.Atividade = (from funcAtiv in FuncaoAtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                                      join Ativ in AtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList() on funcAtiv.UKAtividade equals Ativ.UniqueKey
                                      where funcAtiv.UKFuncao.Equals(iFuncao.UniqueKey)
                                         orderby Ativ.Ativ_Nome
                                      select new Atividade()
                                      {
                                          ID = Ativ.ID,
                                          UniqueKey = Ativ.UniqueKey,
                                          Ativ_Nome = Ativ.Ativ_Nome
                                      }
                                     ).ToList();
                }
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

        [HttpPost]
        public ActionResult CadastrarAtividade(string UKFuncao, string AtividadeNome)
        {
            try
            {
                Atividade oAtividade = new Atividade()
                {
                    UniqueKey = Guid.NewGuid().ToString(),
                    Ativ_Nome = AtividadeNome,                    
                    UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login
                };                

                FuncaoAtividade FuncAtiv = new FuncaoAtividade()
                {
                    UniqueKey = Guid.NewGuid().ToString(),
                    UKFuncao = UKFuncao,
                    UKAtividade =oAtividade.UniqueKey,
                    UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login
                };

                FuncaoAtividadeBusiness.Inserir(FuncAtiv);
                AtividadeBusiness.Inserir(oAtividade);

                TempData["MensagemSucesso"] = "A atividade '" + AtividadeNome + "' foi cadastrada com sucesso.";

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
                Cargo oCargo = CargoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.ID.Equals(IDCargo));
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
        public ActionResult DeletarFuncao(string IDFuncao)
        {
            try
            {
                Funcao oFuncao = FuncaoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.ID.Equals(IDFuncao));
                if (oFuncao == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir a função, pois a mesma não foi localizado." } });
                }
                else
                {
                    oFuncao.DataExclusao = DateTime.Now;
                    oFuncao.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    FuncaoBusiness.Excluir(oFuncao);

                    TempData["MensagemAlerta"] = "A Função '" + oFuncao.Func_Nome + "' foi excluída com sucesso.";

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
        public ActionResult DeletarAtividade(string IDAtividade, string UKAtividade)
        {
            try
            {
                Atividade oAtividade = AtividadeBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.ID.Equals(IDAtividade));
                if (oAtividade == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir a Atividade, pois a mesma não foi localizado." } });
                }
                else
                {
                    oAtividade.DataExclusao = DateTime.Now;
                    oAtividade.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    AtividadeBusiness.Excluir(oAtividade);

                    FuncaoAtividade oFuncaoAtividade = FuncaoAtividadeBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKAtividade.Equals(UKAtividade));
                    if (oFuncaoAtividade == null)
                    {
                        return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível excluir o relacionamento FuncaoAtividade, pois o mesmo não foi localizado." } });
                    }
                    else
                    {
                        oFuncaoAtividade.DataExclusao = DateTime.Now;
                        oFuncaoAtividade.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                        FuncaoAtividadeBusiness.Excluir(oFuncaoAtividade);

                        TempData["MensagemAlerta"] = "A Atividade '" + oAtividade.Ativ_Nome + "' foi excluída com sucesso.";

                        return Json(new { resultado = new RetornoJSON() { URL = Url.Action("Index", "CargoFuncAtiv") } });
                    }
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