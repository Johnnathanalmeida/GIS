using GISModel.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GISCore.Repository.Configuration
{
    public class GISContext : DbContext
    {

        public GISContext() : base("GISConnectionString") {
            Database.SetInitializer<GISContext>(null);
        }

        public DbSet<Empresa> Empresa { get; set; }

        public DbSet<Departamento> Departamento { get; set; }

        public DbSet<Usuario> Usuario { get; set; }

        public DbSet<Perfil> Perfil { get; set; }

        public DbSet<UsuarioPerfil> UsuarioPerfil { get; set; }

        public DbSet<Menu> Menu { get; set; }

        public DbSet<PerfilMenu> PerfilMenu { get; set; }

        public DbSet<Fornecedor> Fornecedor { get; set; }

        public DbSet<Contrato> Contrato { get; set; }

        public DbSet<Empregado> Empregado { get; set; }

        public DbSet<EmpresaFornecedor> EmpresaFornecedor { get; set; }

    }
}
