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
                tempEmpregado.DataExclusao = DateTime.Now;
                tempEmpregado.UsuarioExclusao = empregado.UsuarioExclusao;
                base.Alterar(tempEmpregado);

                empregado.IDEmpregado = tempEmpregado.IDEmpregado;
                empregado.UsuarioExclusao = string.Empty;
                base.Inserir(empregado);
            }

        }

        public override void Excluir(Empregado empregado)
        {
            base.Alterar(empregado);
        }

    }
}
