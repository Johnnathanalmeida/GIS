using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class FuncaoAtividadeBusiness : BaseBusiness<FuncaoAtividade>, IFuncaoAtividadeBusiness
    {
        public override void Excluir(FuncaoAtividade funcaoAtividade)
        {
            base.Alterar(funcaoAtividade);
        }
    }
}
