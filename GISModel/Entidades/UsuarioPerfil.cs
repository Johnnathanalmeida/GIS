using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{

    [Table("RELUsuarioPerfil")]
    public class UsuarioPerfil : EntidadeBase
    {
       
        [Required(ErrorMessage = "Selecione um usuário")]
        public string UKUsuario { get; set; }

        public virtual Usuario Usuario { get; set; }




        [Required(ErrorMessage = "Selecione um perfil")]
        public string UKPerfil { get; set; }

        public virtual Perfil Perfil { get; set; }




        public string UKArea { get; set; }

    }
}
