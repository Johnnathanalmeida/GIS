using GISCore.Repository.Abstract;
using GISCore.Repository.Configuration;
using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Repository.Concrete
{
    public class BaseRepository<T> : IBaseRepository<T> where T : EntidadeBase
    {

        protected GISContext Context;

        public BaseRepository(GISContext contextParam)
        {
            Context = contextParam;
        }

        public void Inserir(T entidade)
        {
            entidade.ID = Guid.NewGuid().ToString();
            entidade.DataInclusao = DateTime.Now;
            entidade.UsuarioInclusao = entidade.UsuarioInclusao;
            entidade.DataExclusao = DateTime.MaxValue;

            Context.Entry(entidade).State = EntityState.Added;
            Context.SaveChanges();
        }

        public void Alterar(T entidade)
        {
            Context.Entry(entidade).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public void Excluir(T entidade)
        {
            Context.Entry(entidade).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public IQueryable<T> Consulta
        {
            get
            {
                return from c in Context.Set<T>() select c;
            }

        }
       
    }
}
