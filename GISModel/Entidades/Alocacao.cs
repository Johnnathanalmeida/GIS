using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("tbAlocacao")]
    public class Alocacao : EntidadeBase
    {

        [Key]
        public string IDAlocacao { get; set; }

        [Display(Name = "Empregado")]
        [Required(ErrorMessage = "Informe um empregado")]
        public string IDEmpregado { get; set; }

        public virtual Empregado Empregado { get; set; }

        [Display(Name = "Contrato")]
        [Required(ErrorMessage = "Informe um contrato")]
        public string IDContrato { get; set; }

        public virtual Contrato Contrato { get; set; }


    }
}
