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

        public bool Obrigatorio { get; set; }

        public string MascaraParaNomeclatura { get; set; }

        public string ExtensoesPermitidas { get; set; }

        public int TamanhoMaximoEmMB { get; set; }

    }
}
