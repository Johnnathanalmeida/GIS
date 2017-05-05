using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace GISModel.Entidades
{
    [Table("tbAdmissao")]
    public class Admissao : EntidadeBase
    {

        public string IDAdmissao { get; set; }

        public string IDEmpregado { get; set; }

        [Display(Name = "Fornecedor")]
        public string IDFornecedor { get; set; }

        [Display(Name = "Empresa")]
        public string IDEmpresa { get; set; }
        
        [Display(Name = "Departamento")]
        public string IDDepartamento { get; set; }

        [Display(Name = "Data de Admissão")]
        [Required(ErrorMessage = "Informe a data de admissão")]
        public DateTime DataAdmissao { get; set; }

        [Display(Name = "Data de Demissão")]
        public DateTime DataDemissao { get; set; }       
        
    }
}
