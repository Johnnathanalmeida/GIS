using System;
using System.Collections.Generic;
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

        public string Nome { get; set; }

        public string Descricao { get; set; }

        public string Endereco { get; set; }

    }
}
