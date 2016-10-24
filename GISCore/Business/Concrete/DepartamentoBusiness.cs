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

        public override void Inserir(Departamento departamento)
        {

            departamento.Status = GISModel.Enums.Situacao.Ativo;

            if (!EmpresaBusiness.Consulta.Any(p => p.IDEmpresa.Equals(departamento.IDEmpresa) && string.IsNullOrEmpty(p.UsuarioExclusao)))
                throw new Exception("Não foi possível localizar a empresa informada.");

            if (Consulta.Any(u => u.Codigo.Equals(departamento.Codigo.Trim()) && u.IDEmpresa.Equals(departamento.IDEmpresa) && string.IsNullOrEmpty(u.UsuarioExclusao)))
                throw new InvalidOperationException("Não é possível inserir o departamento, pois já existe um departamento com este código para esta empresa.");

            if (Consulta.Any(u => u.Sigla.ToUpper().Equals(departamento.Sigla.Trim().ToUpper()) && u.IDEmpresa.Equals(departamento.IDEmpresa) && string.IsNullOrEmpty(u.UsuarioExclusao)))
                throw new InvalidOperationException("Não é possível inserir o departamento, pois já existe um departamento com esta sigla para esta empresa.");

            departamento.IDDepartamento = Guid.NewGuid().ToString();

            base.Inserir(departamento);

        }

        public override void Alterar(Departamento departamento)
        {

            Departamento tempDepartamento = Consulta.FirstOrDefault(p => p.IDDepartamento.Equals(departamento.IDDepartamento));
            if (tempDepartamento == null)
            {
                throw new Exception("Não foi possível encontrar o departamento através do ID.");
            }
            else
            {
                tempDepartamento.DataExclusao = DateTime.Now;
                tempDepartamento.UsuarioExclusao = departamento.UsuarioExclusao;
                base.Alterar(tempDepartamento);

                departamento.IDDepartamento = Guid.NewGuid().ToString();
                departamento.UsuarioExclusao = string.Empty;
                base.Inserir(departamento);
            }

        }

    }
}
