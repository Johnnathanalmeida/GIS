﻿using GISModel.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISModel.Entidades
{

    [Table("tbDepartamento")]
    public class Departamento : EntidadeBase
    {
        
        public string IDDepartamento { get; set; }

        [Required(ErrorMessage = "Informe o código do departamento")]
        public string Codigo { get; set; }

        [Required(ErrorMessage = "Informe a sigla do departamento")]
        public string Sigla { get; set; }

        public string Descricao { get; set; }

        public Situacao Status { get; set; }




        public virtual Empresa Empresa { get; set; }

        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "Selecione uma empresa")]
        public string IDEmpresa { get; set; }




        [Display(Name = "Departamento Vinculado")]
        public string DepartamentoVinculado { get; set; }

    }
}
