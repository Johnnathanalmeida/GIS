using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{

    [Table("RELObjetoTipoDeDocumento")]
    public class ObjetoTipoDeDocumento : EntidadeBase
    {

        public virtual Object Objeto { get; set; }

        [Display(Name = "Objeto")]
        [Required(ErrorMessage = "Selecione um Objeto")]
        public string UKObjeto { get; set; }



        public virtual TipoDeDocumento TipoDeDocumento { get; set; }

        [Display(Name = "Tipo de Documento")]
        [Required(ErrorMessage = "Selecione um Tipo de Documento")]
        public string UKTipoDeDocumento { get; set; }

    }
}
