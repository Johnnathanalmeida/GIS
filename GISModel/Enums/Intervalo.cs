using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Enums
{
    public enum Intervalo
    {
        [Display(Name = "Ano")]
        Ano = 1,
        [Display(Name = "Mês")]
        Mês = 2,
        [Display(Name = "Dia")]
        Dia = 3
    }
}
