using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{

    [Table("GDOBJCategoriaDeDocumento")]
    public class CategoriaDeDocumento : EntidadeBase
    {

        public string Nome { get; set; }

        public List<TipoDeDocumento> Tipos { get; set; }

    }
}
