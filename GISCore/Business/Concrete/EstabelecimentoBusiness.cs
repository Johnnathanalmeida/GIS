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
            if (Consulta.Any(u => string.IsNullOrEmpty(u.UsuarioExclusao) && u.Nome.ToUpper().Trim().Equals(entidade.Nome.ToUpper().Trim())))
                throw new InvalidOperationException("Não é possível inserir o estabelecimento, pois o nome informado já existe no banco de dados.");

            base.Inserir(entidade);
        }

        public override void Alterar(Estabelecimento entidade)
        {
            Estabelecimento tempEstabelecimento = Consulta.FirstOrDefault(p => p.UniqueKey.Equals(entidade.UniqueKey) && string.IsNullOrEmpty(p.UsuarioExclusao));
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
