using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("RELFuncaoDepartamento")]
    public class FuncaoDepartamento : EntidadeBase
    {

        public virtual Funcao Funcao { get; set; }

        [Display(Name = "Função")]
        [Required(ErrorMessage = "Selecione uma função")]
        public string UKFuncao { get; set; }



        public virtual Departamento Departamento { get; set; }

        [Display(Name = "Departamento")]
        [Required(ErrorMessage = "Selecione um Departamento")]
        public string UKDepartamento { get; set; }

    }
}
