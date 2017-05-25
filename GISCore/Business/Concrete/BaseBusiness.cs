using GISCore.Business.Abstract;
using GISCore.Repository.Abstract;
using GISModel.Entidades;
using GISModel.Enums;
using Ninject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Business.Concrete
{
    public class BaseBusiness<T> : IBaseBusiness<T> where T : EntidadeBase
    {

        [Inject]
        public IBaseRepository<T> Repository { get; set; }

        public virtual void Inserir(T entidade)
        {
            Repository.Inserir(entidade);
        }

        public virtual void AlterarComHistorico(T entidade)
        {
            T tempObjeto = null;

            if (!string.IsNullOrEmpty(entidade.ID))
                tempObjeto = Consulta.FirstOrDefault(p => p.UniqueKey.Equals(entidade.ID));
            else if (!string.IsNullOrEmpty(entidade.UniqueKey))
                tempObjeto = Consulta.FirstOrDefault(p => p.UniqueKey.Equals(entidade.UniqueKey));

            if (tempObjeto == null)
            {
                throw new Exception("Não foi possível encontra o objeto a ser atualizado através de sua identifação.");
            }
            else
            {
                tempObjeto.DataExclusao = DateTime.Now;
                tempObjeto.UsuarioExclusao = entidade.UsuarioExclusao;
                Repository.Alterar(tempObjeto);

                entidade.UniqueKey = tempObjeto.UniqueKey;
                entidade.UsuarioExclusao = string.Empty;
                Repository.Inserir(entidade);
            }
        }

        public virtual void Terminar(T entidade)
        {
            entidade.DataExclusao = DateTime.Now;
            Repository.Alterar(entidade);
        }

        public virtual void Alterar(T entidade)
        {
            Repository.Alterar(entidade);
        }

        public virtual void Excluir(T entidade)
        {
            Repository.Excluir(entidade);
        }

        public virtual IQueryable<T> Consulta
        {
            get { return Repository.Consulta; }
        }

        public List<string> GetTodosEnumsIntervalo()
        {
            List<string> lista = new List<string>();

            foreach (Intervalo item in Enum.GetValues(typeof(Intervalo)))
            {
                lista.Add(item.ToString());
                //GISHelpers.Utils.EnumExtensions.GetDisplayName(item)
            }

            return lista;
        }

    }
}
