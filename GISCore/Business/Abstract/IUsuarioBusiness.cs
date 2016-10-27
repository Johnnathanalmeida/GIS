using GISModel.DTO.Conta;
using GISModel.DTO.Usuario;
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

        UsuarioPerfisMenusViewModel ValidarCredenciais(AutenticacaoModel autenticacaoModel);

        byte[] RecuperarFotoPerfil(string login);

        void DefinirSenha(NovaSenhaViewModel novaSenhaViewModel);

        void SolicitarAcesso(string email);

    }
}
