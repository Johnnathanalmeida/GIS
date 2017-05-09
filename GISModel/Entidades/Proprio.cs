using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GISModel.Entidades
{
    [Table("OBJProprio")]
    public class Proprio : EntidadeBase
    {

        public string UKAdmissao { get; set; }

        public string UKEmpresa { get; set; }

        public string UKDepartamento { get; set; }       
        
    }
}
