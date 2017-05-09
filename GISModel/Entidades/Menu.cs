using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("OBJMenu")]
    public class Menu : EntidadeBase
    {

        [Required(ErrorMessage = "Informe o nome do menu")]
        public string Nome { get; set; }

        //[Required(ErrorMessage = "Informe a página padrão")]
        public string Action { get; set; }

        //[Required(ErrorMessage = "Informe o controlador padrão")]
        public string Controller { get; set; }

        [Required(ErrorMessage = "Informe a posição deste menu")]
        public string Ordem { get; set; }

        [Display(Name = "Ícone")]
        public string Icone { get; set; }

        [Display(Name = "Menu Superior")]
        public string UKMenuSuperior { get; set; }

        public virtual Menu MenuSuperior { get; set; }

    }
}
