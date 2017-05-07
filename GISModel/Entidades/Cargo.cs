using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace GISModel.Entidades
{
    [Table("tbCargo")]
    public class Cargo : EntidadeBase
    {
       
        public string IDCargo { get; set; }

        [Display(Name = "Cargo")]
        public string Carg_Nome { get; set; }

        public List<Funcao> Funcao { get; set; }

    }
}
