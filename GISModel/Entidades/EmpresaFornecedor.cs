using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("tbEmpresaFornecedor")]
    public class EmpresaFornecedor : EntidadeBase
    {

        public string IDEmpresaFornecedor { get; set; }

        [Required(ErrorMessage = "Selecione uma empresa")]
        public string IDEmpresa { get; set; }

        public virtual Empresa Empresa { get; set; }

        [Required(ErrorMessage = "Selecione um fornecedor")]
        public string IDFornecedor { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }

    }
}
