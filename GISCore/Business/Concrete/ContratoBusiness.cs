using GISCore.Business.Abstract;
using GISModel.Entidades;
using Ninject;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class ContratoBusiness : BaseBusiness<Contrato>, IContratoBusiness
    {

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        [Inject]
        public IUsuarioBusiness UsuarioBusiness { get; set; }

        public override void Inserir(Contrato contrato)
        {
            contrato.Numero = contrato.Numero.Trim();

            if (Consulta.Any(u => u.Numero.Equals(contrato.Numero) && !string.IsNullOrEmpty(u.UsuarioExclusao)))
                throw new InvalidOperationException("Não é possível cadastrar o contrato, pois já existe um contrato ativo com este número.");

            if (string.IsNullOrEmpty(contrato.IDContrato))
                contrato.IDContrato = Guid.NewGuid().ToString();

            string sLocalFile = Path.Combine(Path.GetTempPath(), "GIS");
            sLocalFile = Path.Combine(sLocalFile, DateTime.Now.ToString("yyyyMMdd"));
            sLocalFile = Path.Combine(sLocalFile, "Contrato");
            sLocalFile = Path.Combine(sLocalFile, contrato.UsuarioInclusao);
            sLocalFile = Path.Combine(sLocalFile, contrato.Arquivo.NomeLocal);

            Usuario oUser = UsuarioBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.Login.Equals(contrato.UsuarioInclusao));
            if (oUser == null)
            {
                throw new Exception("Não foi possível cadastrar o contrato, pois o usuário de inclusão não foi encontrado na base de dados.");
            }
            else
            {
                Empresa oEmp = EmpresaBusiness.Consulta.FirstOrDefault(a => string.IsNullOrEmpty(a.UsuarioExclusao) && a.IDEmpresa.Equals(oUser.IDEmpresa));
                if (oEmp == null) {
                    throw new Exception("Não foi possível cadastrar o contrato, pois a empresa do usuário de inclusão não foi encontrada na base de dados.");
                }
                else
                {
                    string sDiretorio = ConfigurationManager.AppSettings["DiretorioRaiz"] + "\\Images\\Empresas\\" + oEmp.CNPJ.Replace("/", "").Replace(".", "").Replace("-", "");
                    if (!Directory.Exists(sDiretorio))
                        Directory.CreateDirectory(sDiretorio);

                    if (File.Exists(sLocalFile))
                        File.Move(sLocalFile, sDiretorio + "\\" + contrato.Arquivo.NomeRemoto);
                }
            }


            base.Inserir(contrato);


            

        }

        public override void Alterar(Contrato contrato)
        {

            Contrato tempContrato = Consulta.FirstOrDefault(p => p.IDContrato.Equals(contrato.IDContrato));
            if (tempContrato == null)
            {
                throw new Exception("Não foi possível encontra o contrato através do ID.");
            }
            else
            {
                tempContrato.Numero = contrato.Numero;
                tempContrato.Inicio = contrato.Inicio;
                tempContrato.Fim = contrato.Fim;

                tempContrato.UsuarioExclusao = contrato.UsuarioExclusao;
                tempContrato.DataExclusao = contrato.DataExclusao;

                base.Alterar(tempContrato);
            }

        }

    }
}
