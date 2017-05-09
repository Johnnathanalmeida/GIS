using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("GDOBJTipo")]
    public class Tipo : EntidadeBase
    {

        public string UKCategoria { get; set; }

        public string Nome { get; set; }

    }
}
