using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class ArquivoBusiness : BaseBusiness<Arquivo>, IArquivoBusiness
    {

        public override void Inserir(Arquivo entidade)
        {
            base.Inserir(entidade);

            //Mover arquivo da pasta temporária para vault


        }

    }
}
