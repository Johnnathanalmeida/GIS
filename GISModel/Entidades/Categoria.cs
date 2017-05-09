using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{

    [Table("GDOBJCategoria")]
    public class Categoria : EntidadeBase
    {

        public string Nome { get; set; }

    }
}
