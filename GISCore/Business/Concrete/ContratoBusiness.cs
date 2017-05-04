using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class ContratoBusiness : BaseBusiness<Contrato>, IContratoBusiness
    {

        public override void Inserir(Contrato contrato)
        {
            contrato.Numero = contrato.Numero.Trim();

            if (Consulta.Any(u => u.Numero.Equals(contrato.Numero) && !string.IsNullOrEmpty(u.UsuarioExclusao)))
                throw new InvalidOperationException("Não é possível cadastrar o contrato, pois já existe um contrato ativo com este número.");

            if (string.IsNullOrEmpty(contrato.IDContrato))
                contrato.IDContrato = Guid.NewGuid().ToString();

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
