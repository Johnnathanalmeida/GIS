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

    [Table("OBJGarantia")]
    public class Garantia : EntidadeBase
    {

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "Informe a descrição da Garantia.")]
        public string Descricao { get; set; }

        [Required(ErrorMessage = "Selecione o intervalor do prazo da garantia.")]
        public Intervalo Intervalo { get; set; }

        [Required(ErrorMessage = "Informe o prazo da garantia.")]
        public int Prazo { get; set; }

    }
}
