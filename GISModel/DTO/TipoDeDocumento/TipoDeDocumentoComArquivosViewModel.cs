using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.TipoDeDocumento
{
    public class TipoDeDocumentoComArquivosViewModel
    {

        public string UKCategoriaDeDocumento { get; set; }

        public string UniqueKey { get; set; }

        public string Nome { get; set; }

        public bool Obrigatorio { get; set; }

        public string MascaraParaNomeclatura { get; set; }

        public string ExtensoesPermitidas { get; set; }

        public int TamanhoMaximoEmMB { get; set; }

        public Intervalo IntervaloVencimento { get; set; }

        public int PrazoVencimento { get; set; }

        public bool PermiteMultiplosArquivos { get; set; }

        public List<Entidades.Arquivo> Arquivos { get; set; }

    }
}
