using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class EmpregadoBusiness : BaseBusiness<Empregado>, IEmpregadoBusiness
    {

        public override void Inserir(Empregado empregado)
        {

            if (Consulta.Any(u => u.CPF.Equals(empregado.CPF.Trim())))
                throw new InvalidOperationException("Não é possível inserir este empregado, pois já existe um cadastrado com este CPF.");


            empregado.IDEmpregado = Guid.NewGuid().ToString();

            base.Inserir(empregado);

        }

        public override void Alterar(Empregado empregado)
        {

            Empregado tempEmpregado = Consulta.FirstOrDefault(p => p.IDEmpregado.Equals(empregado.IDEmpregado));
            if (tempEmpregado == null)
            {
                throw new Exception("Não foi possível encontrar o empregado através do ID.");
            }
            else
            {
                tempEmpregado.Nome = empregado.Nome;
                tempEmpregado.Email = empregado.Email;
                tempEmpregado.Telefone = empregado.Telefone;
                tempEmpregado.Endereco = empregado.Endereco;
                tempEmpregado.DataNascimento = empregado.DataNascimento;
                tempEmpregado.UsuarioExclusao = empregado.UsuarioExclusao;
                tempEmpregado.DataExclusao = empregado.DataExclusao;

                base.Alterar(tempEmpregado);
            }

        }

    }
}
