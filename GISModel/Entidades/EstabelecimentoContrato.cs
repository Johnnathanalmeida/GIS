using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    public class EstabelecimentoContrato : EntidadeBase
    {

        public string IDEstabelecimentoContrato { get; set; }




        public virtual Estabelecimento Estabelecimento { get; set; }

        [Display(Name = "Estabelecimento")]
        [Required(ErrorMessage = "Selecione um Estabelecimento")]
        public string IDEstabelecimento { get; set; }




        public virtual Contrato Contrato { get; set; }

        [Display(Name = "Contrato")]
        [Required(ErrorMessage = "Selecione um Contrato")]
        public string IDContrato { get; set; }


    }
}
