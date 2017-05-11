using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class FuncaoBusiness : BaseBusiness<Funcao>, IFuncaoBusiness
    {
        public override void Inserir(Funcao funcao)
        {
            funcao.UniqueKey = Guid.NewGuid().ToString();
            base.Inserir(funcao);
        }

        public override void Excluir(Funcao funcao)
        {
            base.Alterar(funcao);
        }
    }
}
