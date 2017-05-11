using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class CargoBusiness : BaseBusiness<Cargo>, ICargoBusiness
    {
        public override void Inserir(Cargo cargo)
        {
            cargo.UniqueKey = Guid.NewGuid().ToString();
            base.Inserir(cargo);
        }

        public override void Excluir(Cargo cargo)
        {
            base.Alterar(cargo);
        }
    }
}
