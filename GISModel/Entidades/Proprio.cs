using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("tbAProprio")]
    public class Proprio : EntidadeBase
    {

        public string IDEmpregado { get; set; }

        public string IDAdmissao { get; set; }

        public string IDEmpresa { get; set; }

        public string IDDepartamento { get; set; }       
        
    }
}
