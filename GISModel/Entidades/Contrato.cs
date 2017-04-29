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

        public string IDContrato { get; set; }

        [Display(Name = "Número")]
        [Required(ErrorMessage = "Informe o número do contrato")]
        public string Numero { get; set; }

        [Display(Name = "Data Início")]
        [Required(ErrorMessage = "Informe a data de início do contrato")]
        public DateTime Inicio { get; set; }

        [Display(Name = "Data Término")]
        [Required(ErrorMessage = "Informe a data de término do contrato")]
        public DateTime Fim { get; set; }

        public string Descricao { get; set; }




        [Display(Name = "Fornecedor")]
        [Required(ErrorMessage = "Selecione um fornecedor")]
        public string IDFornecedor { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }




        [Display(Name = "Departamento")]
        [Required(ErrorMessage = "Selecione um departamento")]
        public string IDDepartamento { get; set; }

        public virtual Departamento Departamento { get; set; }


        public List<Garantia> Garantias { get; set; }

    }
}
