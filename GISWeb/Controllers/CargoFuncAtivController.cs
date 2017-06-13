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

        [Inject]
        public IDepartamentoBusiness DepartamentoBusiness { get; set; }


        [MenuAtivo(MenuAtivo = "Administracao/Cargos")]
        public ActionResult Index()
        {
                        
            List<Cargo> listCargos = (from cg in CargoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao)).ToList()
                                      orderby cg.Nome
                                      select new Cargo
                                      {
                                          ID = cg.ID,
                                          UniqueKey = cg.UniqueKey,
                                          Nome = cg.Nome,
                                          Funcao = new List<Funcao>()
                                      }).ToList();

            foreach (Cargo item in listCargos)
            {

                item.Funcao = (from funcao in FuncaoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                               where funcao.UKCargo.Equals(item.UniqueKey)
                               orderby funcao.Nome
                                select new Funcao()
                                {
                                    ID = funcao.ID,
                                    UKCargo = funcao.UKCargo,
                                    UniqueKey = funcao.UniqueKey,
                                    Nome = funcao.Nome,
                                    NomeDeExibicao = funcao.NomeDeExibicao
                                }
                                ).ToList();

                foreach (Funcao iFuncao in item.Funcao)
                {
                    iFuncao.Atividade = (from funcAtiv in FuncaoAtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                                      join Ativ in AtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList() on funcAtiv.UKAtividade equals Ativ.UniqueKey
                                      where funcAtiv.UKFuncao.Equals(iFuncao.UniqueKey)
                                         orderby Ativ.Nome
                                      select new Atividade()
                                      {
                                          ID = Ativ.ID,
                                          UniqueKey = Ativ.UniqueKey,
                                          Nome = Ativ.Nome
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
                Cargo tempCargo = CargoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.Nome.Equals(Cargo));
                if (tempCargo != null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "O cargo '" + Cargo + "' já existe, favor informar outro cargo." } });
                }

                Cargo oCargo = new Cargo();
                oCargo.Nome = Cargo;
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
        public ActionResult CadastrarFuncao(string UKCargo, string FuncaoNome, string FuncaoNomeExibicao)
        {
            try
            {
                Funcao tempFuncao = FuncaoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKCargo.Equals(UKCargo) && p.Nome.Equals(FuncaoNome));
                if (tempFuncao != null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "A função '" + FuncaoNome + "' já existe, favor informar outra função." } });
                }

                Funcao oFuncao = new Funcao ();
                oFuncao.UKCargo = UKCargo;
                oFuncao.Nome = FuncaoNome;
                oFuncao.NomeDeExibicao = FuncaoNomeExibicao;
                oFuncao.UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                FuncaoBusiness.Inserir(oFuncao);

                TempData["MensagemSucesso"] = "A função '" + FuncaoNomeExibicao + "' foi cadastrado com sucesso.";

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
                List<Atividade> tempAtividade = (from funcAtiv in FuncaoAtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList()
                                            join Ativ in AtividadeBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao)).ToList() on funcAtiv.UKAtividade equals Ativ.UniqueKey
                                                 where funcAtiv.UKFuncao.Equals(UKFuncao) && Ativ.Nome.Equals(AtividadeNome)
                                            select new Atividade()
                                            {
                                                ID = Ativ.ID,
                                                UniqueKey = Ativ.UniqueKey,
                                                Nome = Ativ.Nome
                                            }
                                     ).ToList();

                if (tempAtividade != null && tempAtividade.Count > 0)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "A atividade '" + AtividadeNome + "' já existe, favor informar outra atividade." } });
                }

                Atividade oAtividade = new Atividade()
                {
                    Nome = AtividadeNome,       
                    UniqueKey = Guid.NewGuid().ToString(),
                    UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login
                };

                AtividadeBusiness.Inserir(oAtividade);

                FuncaoAtividade FuncAtiv = new FuncaoAtividade()
                {
                    UniqueKey = Guid.NewGuid().ToString(),
                    UKFuncao = UKFuncao,                    
                    UKAtividade = oAtividade.UniqueKey,
                    UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login
                };

                FuncaoAtividadeBusiness.Inserir(FuncAtiv);
                

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
        public ActionResult AlterarCargo(string UKCargo, string CargoNome)
        {
            try
            {
                Cargo oCargo = CargoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKCargo));
                if (oCargo == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível alterar o cargo, pois o mesmo não foi localizado." } });
                }
                else
                {
                    oCargo.DataExclusao = DateTime.Now;
                    oCargo.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;

                    Cargo nCargo = new Cargo()
                    {
                        Nome = CargoNome,
                        UniqueKey = oCargo.UniqueKey,
                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login
                    };
                    
                    CargoBusiness.Alterar(oCargo);
                    CargoBusiness.Inserir(nCargo);

                    TempData["MensagemSucesso"] = "O cargo '" + oCargo.Nome + "' foi atualizado para '" + CargoNome + "' com sucesso.";

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
        public ActionResult AlterarFuncao(string UKFuncao, string FuncaoNome, string FuncaoNomeDeExibicao)
        {
            try
            {
                Funcao oFuncao = FuncaoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKFuncao));
                if (oFuncao == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível alterar a Função, pois a mesmo não foi localizada." } });
                }
                else
                {
                    oFuncao.DataExclusao = DateTime.Now;
                    oFuncao.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;
                    
                    Funcao nFuncao = new Funcao()
                    {
                        Nome = FuncaoNome,
                        NomeDeExibicao = FuncaoNomeDeExibicao,
                        UniqueKey = oFuncao.UniqueKey,
                        UKCargo = oFuncao.UKCargo,
                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login
                    };

                    FuncaoBusiness.Alterar(oFuncao);
                    FuncaoBusiness.Inserir(nFuncao);

                    TempData["MensagemSucesso"] = "A Função '" + oFuncao.NomeDeExibicao + "' foi atualizada para '" + FuncaoNomeDeExibicao + "' com sucesso.";

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
        public ActionResult AlterarAtividade(string UKAtividade, string AtividadeNome)
        {
            try
            {
                Atividade oAtividade = AtividadeBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UniqueKey.Equals(UKAtividade));
                if (oAtividade == null)
                {
                    return Json(new { resultado = new RetornoJSON() { Erro = "Não foi possível alterar a Atividade, pois a mesmo não foi localizada." } });
                }
                else
                {
                    oAtividade.DataExclusao = DateTime.Now;
                    oAtividade.UsuarioExclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login;

                    Atividade nAtividade = new Atividade()
                    {
                        Nome = AtividadeNome,
                        UniqueKey = oAtividade.UniqueKey,
                        UsuarioInclusao = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.Login
                    };

                    AtividadeBusiness.Alterar(oAtividade);
                    AtividadeBusiness.Inserir(nAtividade);

                    TempData["MensagemSucesso"] = "A Atividade '" + oAtividade.Nome + "' foi atualizada para '" + AtividadeNome + "' com sucesso.";

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
                    CargoBusiness.Alterar(oCargo);

                    TempData["MensagemAlerta"] = "O cargo '" + oCargo.Nome + "' foi excluído com sucesso.";

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
                    FuncaoBusiness.Alterar(oFuncao);

                    TempData["MensagemAlerta"] = "A Função '" + oFuncao.Nome + "' foi excluída com sucesso.";

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
                    AtividadeBusiness.Alterar(oAtividade);

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

                        TempData["MensagemAlerta"] = "A Atividade '" + oAtividade.Nome + "' foi excluída com sucesso.";

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


        public ActionResult GerenciarDepartamentos(string UKFuncao)
        {
            try
            {
                List<Departamento> listDepartamento = (from dpto in DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKEmpresa.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Usuario.UKEmpresa) && string.IsNullOrEmpty(p.UKDepartamentoVinculado)).ToList()
                                                       orderby dpto.Codigo
                                                       select new Departamento
                                                      {
                                                          ID = dpto.ID,
                                                          UniqueKey = dpto.UniqueKey,
                                                          Codigo = dpto.Codigo,
                                                          Sigla = dpto.Sigla,
                                                          SubDepartamento = new List<Departamento>()
                                                      }).ToList();

                foreach (Departamento item in listDepartamento)
                {

                    item.SubDepartamento = (from SubDpto in DepartamentoBusiness.Consulta.Where(a => string.IsNullOrEmpty(a.UsuarioExclusao) && !string.IsNullOrEmpty(a.UKDepartamentoVinculado)).ToList()
                                            where SubDpto.UKDepartamentoVinculado.Equals(item.ID)
                                            orderby SubDpto.Codigo
                                            select new Departamento()
                                            {
                                                ID = SubDpto.ID,
                                                UniqueKey = SubDpto.UniqueKey,
                                                Codigo = SubDpto.Codigo,
                                                Sigla = SubDpto.Sigla,
                                                SubDepartamento = new List<Departamento>()
                                            }).ToList();

                    foreach (Departamento SubSubDepartamento in item.SubDepartamento)
                    {
                        SubSubDepartamento.SubDepartamento = (from subsubDpto in DepartamentoBusiness.Consulta.Where(b => string.IsNullOrEmpty(b.UsuarioExclusao) && !string.IsNullOrEmpty(b.UKDepartamentoVinculado)).ToList()
                                                              where subsubDpto.UKDepartamentoVinculado.Equals(SubSubDepartamento.ID)
                                                              orderby subsubDpto.Codigo
                                                              select new Departamento()
                                                              {
                                                                  ID = subsubDpto.ID,
                                                                  UniqueKey = subsubDpto.UniqueKey,
                                                                  Codigo = subsubDpto.Codigo,
                                                                  Sigla = subsubDpto.Sigla
                                                              }).ToList();
                    }
                }


                ViewBag.Departamento = listDepartamento; //DepartamentoBusiness.Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKEmpresa.Equals(CustomAuthorizationProvider.UsuarioAutenticado.Usuario.UKEmpresa) && string.IsNullOrEmpty(p.UKDepartamentoVinculado)).ToList();
                ViewBag.UKEmpresa = CustomAuthorizationProvider.UsuarioAutenticado.Usuario.UKEmpresa;
                return PartialView("_Departamentos");
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                return Content(ex.Message, "text/html");
            }
        }


        [HttpPost]
        public ActionResult GerenciarDpto(Departamento objDpto)
        {
            try
            {
                //TipoDeDocumento oTipo = TipoDeDocumentoBusiness.Consulta.FirstOrDefault(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.ID.Equals(IDTipo));
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