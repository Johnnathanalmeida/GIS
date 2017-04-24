namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class inicial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbAlocacao",
                c => new
                    {
                        IDAlocacao = c.String(nullable: false, maxLength: 128),
                        IDEmpregado = c.String(nullable: false, maxLength: 128),
                        IDContrato = c.String(nullable: false, maxLength: 128),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDAlocacao)
                .ForeignKey("dbo.tbContrato", t => t.IDContrato, cascadeDelete: true)
                .ForeignKey("dbo.tbEmpregado", t => t.IDEmpregado, cascadeDelete: true)
                .Index(t => t.IDEmpregado)
                .Index(t => t.IDContrato);
            
            CreateTable(
                "dbo.tbContrato",
                c => new
                    {
                        IDContrato = c.String(nullable: false, maxLength: 128),
                        Numero = c.String(nullable: false),
                        Inicio = c.DateTime(nullable: false),
                        Fim = c.DateTime(nullable: false),
                        Descricao = c.String(),
                        IDFornecedor = c.String(nullable: false, maxLength: 128),
                        IDDepartamento = c.String(nullable: false, maxLength: 128),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDContrato)
                .ForeignKey("dbo.tbDepartamento", t => t.IDDepartamento, cascadeDelete: true)
                .ForeignKey("dbo.tbFornecedor", t => t.IDFornecedor, cascadeDelete: true)
                .Index(t => t.IDFornecedor)
                .Index(t => t.IDDepartamento);
            
            CreateTable(
                "dbo.tbDepartamento",
                c => new
                    {
                        IDDepartamento = c.String(nullable: false, maxLength: 128),
                        Codigo = c.String(nullable: false),
                        Sigla = c.String(nullable: false),
                        Descricao = c.String(),
                        Status = c.Int(nullable: false),
                        IDEmpresa = c.String(nullable: false, maxLength: 128),
                        DepartamentoVinculado = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDDepartamento)
                .ForeignKey("dbo.tbEmpresa", t => t.IDEmpresa, cascadeDelete: true)
                .Index(t => t.IDEmpresa);
            
            CreateTable(
                "dbo.tbEmpresa",
                c => new
                    {
                        IDEmpresa = c.String(nullable: false, maxLength: 128),
                        CNPJ = c.String(),
                        RazaoSocial = c.String(),
                        NomeFantasia = c.String(nullable: false),
                        URL_Site = c.String(),
                        URL_LogoMarca = c.String(nullable: false),
                        URL_WS = c.String(),
                        URL_AD = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDEmpresa);
            
            CreateTable(
                "dbo.tbFornecedor",
                c => new
                    {
                        IDFornecedor = c.String(nullable: false, maxLength: 128),
                        Numero = c.String(),
                        Nome = c.String(nullable: false),
                        CNPJ = c.String(),
                        Endereco = c.String(),
                        Telefone = c.String(),
                        Email = c.String(nullable: false),
                        Descricao = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDFornecedor);
            
            CreateTable(
                "dbo.tbEmpregado",
                c => new
                    {
                        IDEmpregado = c.String(nullable: false, maxLength: 128),
                        CPF = c.String(),
                        Nome = c.String(nullable: false),
                        DataNascimento = c.DateTime(nullable: false),
                        Email = c.String(nullable: false),
                        Endereco = c.String(),
                        Telefone = c.String(),
                        IDEmpresa = c.String(maxLength: 128),
                        IDFornecedor = c.String(maxLength: 128),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDEmpregado)
                .ForeignKey("dbo.tbEmpresa", t => t.IDEmpresa)
                .ForeignKey("dbo.tbFornecedor", t => t.IDFornecedor)
                .Index(t => t.IDEmpresa)
                .Index(t => t.IDFornecedor);
            
            CreateTable(
                "dbo.tbEmpresaFornecedor",
                c => new
                    {
                        IDEmpresaFornecedor = c.String(nullable: false, maxLength: 128),
                        IDEmpresa = c.String(nullable: false, maxLength: 128),
                        IDFornecedor = c.String(nullable: false, maxLength: 128),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDEmpresaFornecedor)
                .ForeignKey("dbo.tbEmpresa", t => t.IDEmpresa, cascadeDelete: true)
                .ForeignKey("dbo.tbFornecedor", t => t.IDFornecedor, cascadeDelete: true)
                .Index(t => t.IDEmpresa)
                .Index(t => t.IDFornecedor);
            
            CreateTable(
                "dbo.tbMenu",
                c => new
                    {
                        IDMenu = c.String(nullable: false, maxLength: 128),
                        Nome = c.String(nullable: false),
                        Action = c.String(),
                        Controller = c.String(),
                        Ordem = c.String(nullable: false),
                        Icone = c.String(),
                        IDMenuSuperior = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        MenuSuperior_IDMenu = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.IDMenu)
                .ForeignKey("dbo.tbMenu", t => t.MenuSuperior_IDMenu)
                .Index(t => t.MenuSuperior_IDMenu);
            
            CreateTable(
                "dbo.tbPerfil",
                c => new
                    {
                        IDPerfil = c.String(nullable: false, maxLength: 128),
                        Nome = c.String(nullable: false),
                        Descricao = c.String(),
                        ActionDefault = c.String(nullable: false),
                        ControllerDefault = c.String(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDPerfil);
            
            CreateTable(
                "dbo.tbPerfilMenu",
                c => new
                    {
                        IDPerfilMenu = c.String(nullable: false, maxLength: 128),
                        IDPerfil = c.String(nullable: false, maxLength: 128),
                        IDMenu = c.String(nullable: false, maxLength: 128),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDPerfilMenu)
                .ForeignKey("dbo.tbMenu", t => t.IDMenu, cascadeDelete: false)
                .ForeignKey("dbo.tbPerfil", t => t.IDPerfil, cascadeDelete: false)
                .Index(t => t.IDPerfil)
                .Index(t => t.IDMenu);
            
            CreateTable(
                "dbo.tbUsuario",
                c => new
                    {
                        IDUsuario = c.String(nullable: false, maxLength: 128),
                        CPF = c.String(),
                        Nome = c.String(nullable: false),
                        Login = c.String(nullable: false),
                        Senha = c.String(),
                        Email = c.String(nullable: false),
                        IDEmpresa = c.String(nullable: false, maxLength: 128),
                        IDDepartamento = c.String(nullable: false, maxLength: 128),
                        TipoDeAcesso = c.Int(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDUsuario)
                .ForeignKey("dbo.tbDepartamento", t => t.IDDepartamento, cascadeDelete: false)
                .ForeignKey("dbo.tbEmpresa", t => t.IDEmpresa, cascadeDelete: false)
                .Index(t => t.IDEmpresa)
                .Index(t => t.IDDepartamento);
            
            CreateTable(
                "dbo.tbUsuarioPerfil",
                c => new
                    {
                        IDUsuarioPerfil = c.String(nullable: false, maxLength: 128),
                        IDUsuario = c.String(nullable: false, maxLength: 128),
                        IDPerfil = c.String(nullable: false, maxLength: 128),
                        IDArea = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDUsuarioPerfil)
                .ForeignKey("dbo.tbPerfil", t => t.IDPerfil, cascadeDelete: true)
                .ForeignKey("dbo.tbUsuario", t => t.IDUsuario, cascadeDelete: true)
                .Index(t => t.IDUsuario)
                .Index(t => t.IDPerfil);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbUsuarioPerfil", "IDUsuario", "dbo.tbUsuario");
            DropForeignKey("dbo.tbUsuarioPerfil", "IDPerfil", "dbo.tbPerfil");
            DropForeignKey("dbo.tbUsuario", "IDEmpresa", "dbo.tbEmpresa");
            DropForeignKey("dbo.tbUsuario", "IDDepartamento", "dbo.tbDepartamento");
            DropForeignKey("dbo.tbPerfilMenu", "IDPerfil", "dbo.tbPerfil");
            DropForeignKey("dbo.tbPerfilMenu", "IDMenu", "dbo.tbMenu");
            DropForeignKey("dbo.tbMenu", "MenuSuperior_IDMenu", "dbo.tbMenu");
            DropForeignKey("dbo.tbEmpresaFornecedor", "IDFornecedor", "dbo.tbFornecedor");
            DropForeignKey("dbo.tbEmpresaFornecedor", "IDEmpresa", "dbo.tbEmpresa");
            DropForeignKey("dbo.tbAlocacao", "IDEmpregado", "dbo.tbEmpregado");
            DropForeignKey("dbo.tbEmpregado", "IDFornecedor", "dbo.tbFornecedor");
            DropForeignKey("dbo.tbEmpregado", "IDEmpresa", "dbo.tbEmpresa");
            DropForeignKey("dbo.tbAlocacao", "IDContrato", "dbo.tbContrato");
            DropForeignKey("dbo.tbContrato", "IDFornecedor", "dbo.tbFornecedor");
            DropForeignKey("dbo.tbContrato", "IDDepartamento", "dbo.tbDepartamento");
            DropForeignKey("dbo.tbDepartamento", "IDEmpresa", "dbo.tbEmpresa");
            DropIndex("dbo.tbUsuarioPerfil", new[] { "IDPerfil" });
            DropIndex("dbo.tbUsuarioPerfil", new[] { "IDUsuario" });
            DropIndex("dbo.tbUsuario", new[] { "IDDepartamento" });
            DropIndex("dbo.tbUsuario", new[] { "IDEmpresa" });
            DropIndex("dbo.tbPerfilMenu", new[] { "IDMenu" });
            DropIndex("dbo.tbPerfilMenu", new[] { "IDPerfil" });
            DropIndex("dbo.tbMenu", new[] { "MenuSuperior_IDMenu" });
            DropIndex("dbo.tbEmpresaFornecedor", new[] { "IDFornecedor" });
            DropIndex("dbo.tbEmpresaFornecedor", new[] { "IDEmpresa" });
            DropIndex("dbo.tbEmpregado", new[] { "IDFornecedor" });
            DropIndex("dbo.tbEmpregado", new[] { "IDEmpresa" });
            DropIndex("dbo.tbDepartamento", new[] { "IDEmpresa" });
            DropIndex("dbo.tbContrato", new[] { "IDDepartamento" });
            DropIndex("dbo.tbContrato", new[] { "IDFornecedor" });
            DropIndex("dbo.tbAlocacao", new[] { "IDContrato" });
            DropIndex("dbo.tbAlocacao", new[] { "IDEmpregado" });
            DropTable("dbo.tbUsuarioPerfil");
            DropTable("dbo.tbUsuario");
            DropTable("dbo.tbPerfilMenu");
            DropTable("dbo.tbPerfil");
            DropTable("dbo.tbMenu");
            DropTable("dbo.tbEmpresaFornecedor");
            DropTable("dbo.tbEmpregado");
            DropTable("dbo.tbFornecedor");
            DropTable("dbo.tbEmpresa");
            DropTable("dbo.tbDepartamento");
            DropTable("dbo.tbContrato");
            DropTable("dbo.tbAlocacao");
        }
    }
}
