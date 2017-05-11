using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{

    [Table("RELFuncaoAtividade")]
    public class FuncaoAtividade : EntidadeBase
    {
        [Display(Name = "Função")]
        public string UKFuncao { get; set; }

        [Display(Name = "Atividade")]
        public string UKAtividade { get; set; }
                
    }
}
