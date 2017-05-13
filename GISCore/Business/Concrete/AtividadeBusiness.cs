using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class AtividadeBusiness : BaseBusiness<Atividade>, IAtividadeBusiness
    {
        public override void Inserir(Atividade atividade)
        {
            if (string.IsNullOrEmpty(atividade.UniqueKey))
                atividade.UniqueKey = Guid.NewGuid().ToString();
            base.Inserir(atividade);
        }
    }
}
