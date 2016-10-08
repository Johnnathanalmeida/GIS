using GISCore.Business.Abstract;
using GISModel.DTO.Departamento;
using GISModel.Entidades;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class DepartamentoBusiness : BaseBusiness<Departamento>, IDepartamentoBusiness
    {

        [Inject]
        public IEmpresaBusiness EmpresaBusiness { get; set; }

        public override void Inserir(Departamento pDepartamento)
        {

            pDepartamento.Status = GISModel.Enums.Situacao.Ativo;

            if (!EmpresaBusiness.Consulta.Any(p => p.IDEmpresa.Equals(pDepartamento.IDEmpresa)))
                throw new Exception("Não foi possível localizar a empresa informada.");
            
            if (Consulta.Any(u => u.Codigo.Equals(pDepartamento.Codigo.Trim()) && u.IDEmpresa.Equals(pDepartamento.IDEmpresa)))
                throw new InvalidOperationException("Não é possível inserir o departamento, pois já existe um departamento com este código para esta empresa.");

            if (Consulta.Any(u => u.Sigla.ToUpper().Equals(pDepartamento.Sigla.Trim().ToUpper()) && u.IDEmpresa.Equals(pDepartamento.IDEmpresa)))
                throw new InvalidOperationException("Não é possível inserir o departamento, pois já existe um departamento com esta sigla para esta empresa.");

            pDepartamento.IDDepartamento = Guid.NewGuid().ToString();

            base.Inserir(pDepartamento);

        }

    }
}
