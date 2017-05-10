using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class EstabelecimentoBusiness : BaseBusiness<Estabelecimento>, IEstabelecimentoBusiness
    {

        public override void Inserir(Estabelecimento entidade)
        {
            if (Consulta.Any(u => u.Nome.ToUpper().Trim().Equals(entidade.Nome.ToUpper().Trim())))
                throw new InvalidOperationException("Não é possível inserir o estabelecimento, pois o nome informado já está sendo utilizado por outro estabelecimento.");

            base.Inserir(entidade);
        }

        public override void Alterar(Estabelecimento entidade)
        {
            Estabelecimento tempEstabelecimento = Consulta.FirstOrDefault(p => p.UniqueKey.Equals(entidade.UniqueKey));
            if (tempEstabelecimento == null)
            {
                throw new Exception("Não foi possível encontra o estabelecimento através do ID.");
            }
            else
            {
                tempEstabelecimento.DataExclusao = DateTime.Now;
                tempEstabelecimento.UsuarioExclusao = entidade.UsuarioExclusao;
                base.Alterar(tempEstabelecimento);

                entidade.UniqueKey = tempEstabelecimento.UniqueKey;
                entidade.UsuarioExclusao = string.Empty;
                base.Inserir(entidade);
            }

        }

    }
}
