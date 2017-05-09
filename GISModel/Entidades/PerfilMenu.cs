using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{

    [Table("RELPerfilMenu")]
    public class PerfilMenu : EntidadeBase
    {
       
        [Required(ErrorMessage = "Selecione um perfil")]
        public string UKPerfil { get; set; }

        public virtual Perfil Perfil { get; set; }

        [Required(ErrorMessage = "Selecione um menu")]
        public string UKMenu { get; set; }

        public virtual Menu Menu { get; set; }

    }
}
