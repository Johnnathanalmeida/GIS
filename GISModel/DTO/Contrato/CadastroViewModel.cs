using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.DTO.Contrato
{
    public class CadastroViewModel
    {

        public string IDContrato { get; set; }

        [Display(Name = "Número")]
        [Required(ErrorMessage = "Informe o número do contrato")]
        public string Numero { get; set; }

        [Display(Name = "Data Início")]
        [Required(ErrorMessage = "Informe a data de início do contrato")]
        public string Inicio { get; set; }

        [Display(Name = "Data Término")]
        [Required(ErrorMessage = "Informe a data de término do contrato")]
        public string Fim { get; set; }

        [Display(Name = "Descrição")]
        public string Descricao { get; set; }




        [Display(Name = "Fornecedor")]
        [Required(ErrorMessage = "Selecione um fornecedor")]
        public string IDFornecedor { get; set; }




        [Display(Name = "Departamento(s)")]
        [Required(ErrorMessage = "Selecione pelo menos um departamento")]
        public string Departamentos { get; set; }


        [Display(Name = "Arquivo")]
        public string NomeArquivoLocal { get; set; }


        public List<Garantia> Garantias { get; set; }

    }
}
