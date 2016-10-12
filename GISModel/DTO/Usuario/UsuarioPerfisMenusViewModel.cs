using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GISModel.Entidades;

namespace GISModel.DTO.Usuario
{
    public class UsuarioPerfisMenusViewModel
    {

        public GISModel.Entidades.Usuario Usuario { get; set; }

        public IList<Perfil> Perfis { get; set; }

        public IList<GISModel.Entidades.Menu> Menus { get; set; }

    }
}
