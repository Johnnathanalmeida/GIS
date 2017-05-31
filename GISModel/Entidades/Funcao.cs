using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
using System.Collections.Generic;

namespace GISModel.Entidades
{
    [Table("OBJFuncao")]
    public class Funcao : EntidadeBase
    {
       
        public string UKCargo { get; set; }

        [Display(Name = "Função")]
        public string Nome { get; set; }

        public string NomeDeExibicao { get; set; }

        public List<Atividade> Atividade { get; set; }

    }
}
