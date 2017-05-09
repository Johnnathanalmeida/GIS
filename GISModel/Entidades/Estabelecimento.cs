using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{

    [Table("OBJEstabelecimento")]
    public class Estabelecimento : EntidadeBase
    {

        [Required(ErrorMessage = "Informe o nome do estabelecimento.")]
        public string Nome { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Endereço")]
        public string Endereco { get; set; }

    }
}
