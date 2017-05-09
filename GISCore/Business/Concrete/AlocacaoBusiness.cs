using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class AlocacaoBusiness : BaseBusiness<Alocacao>, IAlocacaoBusiness
    {

        public override void Inserir(Alocacao alocacao)
        {
           
            if (Consulta.Any(u => string.IsNullOrEmpty(u.UsuarioExclusao) && u.UKContrato.Equals(alocacao.UKContrato) && u.UKEmpregado.Equals(alocacao.UKEmpregado)))
                throw new InvalidOperationException("Este empregado já está alocado neste contrato.");

            alocacao.UniqueKey = Guid.NewGuid().ToString();

            base.Inserir(alocacao);
        }

    }
}
