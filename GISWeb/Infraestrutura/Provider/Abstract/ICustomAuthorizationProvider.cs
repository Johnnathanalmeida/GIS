using GISModel.DTO.Conta;
using GISModel.DTO.Usuario;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GISWeb.Infraestrutura.Provider.Abstract
{
    public interface ICustomAuthorizationProvider
    {

        UsuarioPerfisMenusViewModel UsuarioAutenticado { get; }

        bool EstaAutenticado { get; }

        void Logar(AutenticacaoModel autenticacaoModel);

        void Deslogar();

    }
}