using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace GISModel.Entidades
{
    [Table("tbFuncao")]
    public class Funcao : EntidadeBase
    {
       
        public string IDCargo { get; set; }

        public string IDFuncao { get; set; }

        [Display(Name = "Função")]
        public string Func_Nome { get; set; }

    }
}
