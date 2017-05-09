using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{

    [Table("RELDepartamentoContrato")]
    public class DepartamentoContrato : EntidadeBase
    {
        public virtual Departamento Departamento { get; set; }

        [Display(Name = "Departamento")]
        [Required(ErrorMessage = "Selecione um Departamento")]
        public string UKDepartamento { get; set; }



        public virtual Contrato Contrato { get; set; }

        [Display(Name = "Contrato")]
        [Required(ErrorMessage = "Selecione um Contrato")]
        public string UKContrato { get; set; }
    }
}
