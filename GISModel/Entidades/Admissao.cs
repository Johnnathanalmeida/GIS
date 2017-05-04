using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbAdmissao")]
    public class Admissao : EntidadeBase
    {

        public string IDAdmissao { get; set; }

        public string IDEmpregado { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }

        [Display(Name = "Fornecedor")]
        public string IDFornecedor { get; set; }

        public virtual Empresa Empresa { get; set; }

        [Display(Name = "Empresa")]
        public string IDEmpresa { get; set; }

        public virtual Departamento Departamento { get; set; }

        [Display(Name = "Departamento")]
        public string IDDepartamentos { get; set; }

        [Display(Name = "Data de Admissão")]
        [Required(ErrorMessage = "Informe a data de admissão")]
        public string DataAdmissao { get; set; }

        [Display(Name = "Data de Demissão")]
        public string DataDemissao { get; set; }       
        
    }
}
