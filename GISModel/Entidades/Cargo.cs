using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace GISModel.Entidades
{
    [Table("OBJCargo")]
    public class Cargo : EntidadeBase
    {
        
        [Display(Name = "Cargo")]
        public string Nome { get; set; }

        public List<Funcao> Funcao { get; set; }

    }
}
