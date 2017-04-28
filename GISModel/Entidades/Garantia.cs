using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{

    [Table("tbGarantia")]
    public class Garantia : EntidadeBase
    {

        public string IDGarantia { get; set; }

        public Intervalo Intervalo { get; set; }

        public int Prazo { get; set; }

    }
}
