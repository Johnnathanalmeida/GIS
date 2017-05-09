using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("RELEmpresaArquivo")]
    public class EmpresaArquivo : EntidadeBase
    {

        [Required(ErrorMessage = "Selecione uma empresa")]
        public string UKEmpresa { get; set; }

        public virtual Empresa Empresa { get; set; }




        [Required(ErrorMessage = "Selecione um Arquivo")]
        public string UKArquivo { get; set; }

        public virtual Arquivo Arquivo { get; set; }

    }
}
