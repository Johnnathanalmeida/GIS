using GISModel.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("OBJFornecedor")]
    public class Fornecedor : EntidadeBase
    {

        [Display(Name = "Número")]
        public string Numero { get; set; }

        [Required(ErrorMessage = "Informe um nome para o fornecedor")]
        public string Nome { get; set; }

        [Display(Name = "CNPJ")]
        [Required(ErrorMessage = "Informe um CNPJ")]
        [CustomValidationCNPJ(ErrorMessage = "CPNJ inválido")]
        public string CNPJ { get; set; }

        [Display(Name = "Endereço")]
        public string Endereco { get; set; }

        public string Telefone { get; set; }

        [Display(Name = "E-mail")]
        [Required(ErrorMessage = "Informe o e-mail do usuário")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Informe um e-mail válido")]
        public string Email { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }

    }
}
