using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace GISModel.Entidades
{
    [Table("OBJAtividade")]
    public class Atividade : EntidadeBase
    {
        [Display(Name = "Atividade")]
        public string Ativ_Nome { get; set; }
    }
}
