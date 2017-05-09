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
            if (!Consulta.Any(p => p.UKPerfil.Equals(PerfilMenu.UKPerfil) && p.UKMenu.Equals(PerfilMenu.UKMenu) && string.IsNullOrEmpty(p.UsuarioExclusao)))
            {
                PerfilMenu.UniqueKey = Guid.NewGuid().ToString();

                base.Inserir(PerfilMenu);
            }
        }

        public override void Alterar(PerfilMenu entidade)
        {
            List<PerfilMenu> lPerfilMenu = Consulta.Where(p => string.IsNullOrEmpty(p.UsuarioExclusao) && p.UKPerfil.Equals(entidade.UKPerfil) && p.UKMenu.Equals(entidade.UKMenu)).ToList();
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
