using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace GISModel.Entidades
{
    [Table("OBJFuncao")]
    public class Funcao : EntidadeBase
    {
       
        public string UKCargo { get; set; }

        [Display(Name = "Função")]
        public string Func_Nome { get; set; }

    }
}
