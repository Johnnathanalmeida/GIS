using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("GDOBJTipoDeDocumento")]
    public class TipoDeDocumento : EntidadeBase
    {

        public string UKCategoriaDeDocumento { get; set; }

        public string Nome { get; set; }

        [Display(Name = "Obrigatório")]
        public bool Obrigatorio { get; set; }

        [Display(Name = "Mascara para Nomeclatura")]
        public string MascaraParaNomeclatura { get; set; }

        [Display(Name = "Extensões Permitidas")]
        public string ExtensoesPermitidas { get; set; }

        [Display(Name = "Tamanho máximo (Mb)")]
        public int TamanhoMaximoEmMB { get; set; }


        [Display(Name = "Intervalo de Vencimento")]
        public Intervalo IntervaloVencimento { get; set; }

        [Display(Name = "Prazo de Vencimento")]
        public int PrazoVencimento { get; set; }

    }
}
