using GISModel.CustomAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GISModel.Enums;

namespace GISModel.Entidades
{

    [Table("OBJEmpregado")]
    public class Empregado : EntidadeBase
    {

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "CPF obrigatório")]
        [CustomValidationCPF(ErrorMessage = "CPF inválido")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O nome é obrigatório")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O sexo é obrigatório")]
        public Sexo? Sexo { get; set; }

        [Display(Name = "Data de Nascimento")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "Informe o e-mail do empregado")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Informe um e-mail válido")]
        public string Email { get; set; }

        public string Endereco { get; set; }

        public string Telefone { get; set; }

        [Display(Name = "Tipo de Empregado")]
        [Required(ErrorMessage = "Informe o tipo do empregado")]
        public TipoEmpregado? TipoEmpregado { get; set; }

    }
}
