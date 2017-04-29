using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbTerceirizado")]
    public class Terceirizado : EntidadeBase
    {

        public string IDAdmissao { get; set; }

        public string IDEmpregado { get; set; }

        public string IDFornecedor { get; set; }       
        
    }
}
