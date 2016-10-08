using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("tbContrato")]
    public class Contrato : EntidadeBase
    {

        [Key]
        public string IDContrato { get; set; }

        public string Numero { get; set; }

    }
}
