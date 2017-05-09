﻿using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class UsuarioPerfilBusiness : BaseBusiness<UsuarioPerfil>, IUsuarioPerfilBusiness
    {

        public override void Inserir(UsuarioPerfil UsuarioPerfil)
        {
            if (!Consulta.Any(u => u.UKUsuario.Equals(UsuarioPerfil.UKUsuario) && u.UKPerfil.Equals(UsuarioPerfil.UKPerfil) && u.UKArea.Equals(UsuarioPerfil.UKArea) && string.IsNullOrEmpty(u.UsuarioExclusao)))
            {
                UsuarioPerfil.UniqueKey = Guid.NewGuid().ToString();

                base.Inserir(UsuarioPerfil);
            }
        }

        public override void Alterar(UsuarioPerfil entidade)
        {
            List<UsuarioPerfil> lUsuarioPerfil = Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKPerfil.Equals(entidade.UKPerfil) && p.UKUsuario.Equals(entidade.UKUsuario)).ToList();
            if (lUsuarioPerfil.Count.Equals(1))
            {
                UsuarioPerfil oUsuarioPerfil = lUsuarioPerfil[0];

                oUsuarioPerfil.UsuarioExclusao = entidade.UsuarioExclusao;
                oUsuarioPerfil.DataExclusao = entidade.DataExclusao;

                base.Alterar(oUsuarioPerfil);
            }
        }

    }
}
