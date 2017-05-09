﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{
    [Table("RELEmpresaFornecedor")]
    public class EmpresaFornecedor : EntidadeBase
    {

        [Required(ErrorMessage = "Selecione uma empresa")]
        public string UKEmpresa { get; set; }

        public virtual Empresa Empresa { get; set; }

        [Required(ErrorMessage = "Selecione um fornecedor")]
        public string UKFornecedor { get; set; }

        public virtual Fornecedor Fornecedor { get; set; }

    }
}
