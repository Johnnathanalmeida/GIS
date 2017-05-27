using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Repository.Configuration
{
    public class GISGlobalContext : DbContext
    {

        public GISGlobalContext() : base("GISGlobalConnectionString") {
            Database.SetInitializer<GISGlobalContext>(null);
        }

        public DbSet<Acesso> Acesso { get; set; }

        public DbSet<Empresa> Empresa { get; set; }

    }
}
