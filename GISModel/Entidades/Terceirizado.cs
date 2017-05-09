using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("OBJTerceirizado")]
    public class Terceirizado : EntidadeBase
    {

        public string UKAdmissao { get; set; }

        public string UKFornecedor { get; set; }       
        
    }
}
