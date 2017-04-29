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
    [Table("tbFornecedor")]
    public class Fornecedor : EntidadeBase
    {

        public string IDFornecedor { get; set; }

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




        public virtual Empresa Empresa { get; set; }

        [Display(Name = "Empresa")]
        public string IDEmpresa { get; set; }

    }
}
