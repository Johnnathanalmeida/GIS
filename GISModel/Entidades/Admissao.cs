using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace GISModel.Entidades
{
    [Table("OBJAdmissao")]
    public class Admissao : EntidadeBase
    {

        
        public string UKEmpregado { get; set; }

        public string UKUsuarioDemissao { get; set; }

        [Display(Name = "Fornecedor")]
        public string UKFornecedor { get; set; }

        [Display(Name = "Empresa")]
        public string UKEmpresa { get; set; }

        [Display(Name = "Departamento")]
        public string UKDepartamento { get; set; }

        [Display(Name = "Data de Admissão")]
        [Required(ErrorMessage = "Informe a data de admissão")]
        public DateTime DataAdmissao { get; set; }

        [Display(Name = "Data de Demissão")]
        public DateTime DataDemissao { get; set; }

    }
}
