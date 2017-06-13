using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{

    [Table("OBJDepartamento")]
    public class Departamento : EntidadeBase
    {
        [Required(ErrorMessage = "Informe o código do departamento")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Informe a sigla do departamento")]
        public string Sigla { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        public Situacao Status { get; set; }
        
        public virtual Empresa Empresa { get; set; }

        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "Selecione uma empresa")]
        public string UKEmpresa { get; set; }

        [Display(Name = "Departamento Vinculado")]
        public string UKDepartamentoVinculado { get; set; }

        public List<Departamento> SubDepartamento { get; set; }
    }
}
