using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("RELContratoGarantia")]
    public class ContratoGarantia : EntidadeBase
    {

        public virtual Contrato Contrato { get; set; }

        [Display(Name = "Contrato")]
        [Required(ErrorMessage = "Selecione um Contrato")]
        public string UKContrato { get; set; }



        public virtual Garantia Garantia { get; set; }

        [Display(Name = "Garantia")]
        [Required(ErrorMessage = "Selecione uma Garantia")]
        public string UKGarantia { get; set; }
    }

}
