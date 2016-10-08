using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class PerfilMenuBusiness : BaseBusiness<PerfilMenu>, IPerfilMenuBusiness
    {

        public override void Inserir(PerfilMenu PerfilMenu)
        {
            if (!Consulta.Any(p => p.IDPerfil.Equals(PerfilMenu.IDPerfil) && p.IDMenu.Equals(PerfilMenu.IDMenu) && string.IsNullOrEmpty(p.UsuarioExclusao)))
            {
                PerfilMenu.IDPerfilMenu = Guid.NewGuid().ToString();

                base.Inserir(PerfilMenu);
            }
        }

        public override void Alterar(PerfilMenu entidade)
        {
            List<PerfilMenu> lPerfilMenu = Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.IDPerfil.Equals(entidade.IDPerfil) && p.IDMenu.Equals(entidade.IDMenu)).ToList();
            if (lPerfilMenu.Count.Equals(1))
            {
                PerfilMenu oPerfilMenu = lPerfilMenu[0];

                oPerfilMenu.UsuarioExclusao = entidade.UsuarioExclusao;
                oPerfilMenu.DataExclusao = entidade.DataExclusao;

                base.Alterar(oPerfilMenu);
            }
        }

    }
}
