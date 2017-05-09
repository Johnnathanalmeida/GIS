using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("RELContratoArquivo")]
    public class ContratoArquivo : EntidadeBase
    {

        public virtual Contrato Contrato { get; set; }

        [Display(Name = "Contrato")]
        [Required(ErrorMessage = "Selecione um Contrato")]
        public string UKContrato { get; set; }



        public virtual Arquivo Arquivo { get; set; }

        [Display(Name = "Arquivo")]
        [Required(ErrorMessage = "Selecione um Arquivo")]
        public string UKArquivo { get; set; }

    }
}
