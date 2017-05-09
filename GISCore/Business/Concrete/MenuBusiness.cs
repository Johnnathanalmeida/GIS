using GISCore.Business.Abstract;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class MenuBusiness : BaseBusiness<Menu>, IMenuBusiness
    {
        public override void Inserir(Menu Menu)
        {
            Menu.Nome = Menu.Nome.Trim();

            if (Consulta.Any(u => u.Nome.Equals(Menu.Nome) && u.UKMenuSuperior.Equals(Menu.UKMenuSuperior)))
                throw new InvalidOperationException("Não é possível inserir o menu, pois já existe um menu cadastro com este nome.");

            Menu.UniqueKey = Guid.NewGuid().ToString();

            base.Inserir(Menu);
        }

        public override void Alterar(Menu entidade)
        {
            Menu tempMenu = Consulta.FirstOrDefault(p => p.UniqueKey.Equals(entidade.UniqueKey));
            if (tempMenu == null)
            {
                throw new Exception("Não foi possível encontra o menu através do ID.");
            }
            else
            {
                tempMenu.DataExclusao = DateTime.Now;
                tempMenu.UsuarioExclusao = entidade.UsuarioExclusao;
                base.Alterar(tempMenu);

                entidade.UniqueKey = tempMenu.UniqueKey;
                entidade.UsuarioExclusao = string.Empty;
                base.Inserir(entidade);
            }
        }

    }
}
