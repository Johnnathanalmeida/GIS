using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Infraestrutura.Comparer
{
    public class UsuarioComparer : IEqualityComparer<Usuario>
    {

        public bool Equals(Usuario x1, Usuario x2)
        {
            if (object.ReferenceEquals(x1, x2))
                return true;
            if (x1 == null || x2 == null)
                return false;
            return x1.UniqueKey.Equals(x2.UniqueKey);
        }

        public int GetHashCode(Usuario x)
        {
            return x.UniqueKey.GetHashCode();
        }

    }
}
