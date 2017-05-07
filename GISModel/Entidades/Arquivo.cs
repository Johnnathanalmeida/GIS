using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("tbArquivo")]
    public class Arquivo : EntidadeBase
    {

        public string NomeLocal { get; set; }

        public string NomeRemoto { get; set; }

        public int Versao { get; set; }

    }
}
