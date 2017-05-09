using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("OBJAlocacao")]
    public class Alocacao : EntidadeBase
    {

        [Display(Name = "Empregado")]
        [Required(ErrorMessage = "Informe um empregado")]
        public string UKEmpregado { get; set; }

        public virtual Empregado Empregado { get; set; }

        [Display(Name = "Contrato")]
        [Required(ErrorMessage = "Informe um contrato")]
        public string UKContrato { get; set; }

        public virtual Contrato Contrato { get; set; }

    }
}
