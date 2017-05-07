using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{

    [Table("tbEstabelecimento")]
    public class Estabelecimento : EntidadeBase
    {

        public string IDEstabelecimento { get; set; }

        [Required(ErrorMessage = "Informe o nome do estabelecimento.")]
        public string Nome { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

        [Display(Name = "Endereço")]
        public string Endereco { get; set; }




        [Display(Name = "Imagem")]
        public string IDArquivo { get; set; }

        public virtual Arquivo Arquivo { get; set; }

    }
}
