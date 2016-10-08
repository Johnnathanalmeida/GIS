using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("tbMenu")]
    public class Menu : EntidadeBase
    {

        [Key]
        public string IDMenu { get; set; }

        [Required(ErrorMessage = "Informe o nome do menu")]
        public string Nome { get; set; }

        //[Required(ErrorMessage = "Informe a página padrão")]
        public string Action { get; set; }

        //[Required(ErrorMessage = "Informe o controlador padrão")]
        public string Controller { get; set; }

        [Required(ErrorMessage = "Informe a posição deste menu")]
        public string Ordem { get; set; }

        [Display(Name = "Menu Superior")]
        public string IDMenuSuperior { get; set; }

        public virtual Menu MenuSuperior { get; set; }

    }
}
