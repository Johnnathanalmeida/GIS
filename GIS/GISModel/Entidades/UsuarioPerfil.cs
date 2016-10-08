using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{

    [Table("tbUsuarioPerfil")]
    public class UsuarioPerfil : EntidadeBase
    {
        [Key]
        public string IDUsuarioPerfil { get; set; }

        [Required(ErrorMessage = "Selecione um usuário")]
        public string IDUsuario { get; set; }

        public virtual Usuario Usuario { get; set; }

        [Required(ErrorMessage = "Selecione um perfil")]
        public string IDPerfil { get; set; }

        public virtual Perfil Perfil { get; set; }

    }
}
