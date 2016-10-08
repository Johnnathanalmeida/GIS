using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{

    [Table("tbPerfilMenu")]
    public class PerfilMenu : EntidadeBase
    {
        [Key]
        public string IDPerfilMenu { get; set; }

        [Required(ErrorMessage = "Selecione um perfil")]
        public string IDPerfil { get; set; }

        public virtual Perfil Perfil { get; set; }

        [Required(ErrorMessage = "Selecione um menu")]
        public string IDMenu { get; set; }

        public virtual Menu Menu { get; set; }

    }
}
