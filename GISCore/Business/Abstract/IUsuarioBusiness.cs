using GISModel.DTO.Conta;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Abstract
{
    public interface IUsuarioBusiness : IBaseBusiness<Usuario>
    {

        Usuario ValidarCredenciais(AutenticacaoModel autenticacaoModel);

        byte[] RecuperarFotoPerfil(string login);

    }
}
