using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("RELObjetoArquivo")]
    public class ObjetoArquivo : EntidadeBase
    {

        public virtual Object Objeto { get; set; }

        [Display(Name = "Objeto")]
        [Required(ErrorMessage = "Selecione um Objeto")]
        public string UKObjeto { get; set; }



        public virtual Arquivo Arquivo { get; set; }

        [Display(Name = "Arquivo")]
        [Required(ErrorMessage = "Selecione um Arquivo")]
        public string UKArquivo { get; set; }

    }
}
