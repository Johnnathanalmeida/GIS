namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class db : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbContrato",
                c => new
                    {
                        IDContrato = c.String(nullable: false, maxLength: 128),
                        Numero = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDContrato);
            
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
                        CNPJ = c.String(nullable: false),
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
                "dbo.tbMenu",
                c => new
                    {
                        IDMenu = c.String(nullable: false, maxLength: 128),
                        Nome = c.String(nullable: false),
                        Action = c.String(),
                        Controller = c.String(),
                        Ordem = c.String(nullable: false),
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
                .ForeignKey("dbo.tbMenu", t => t.IDMenu, cascadeDelete: true)
                .ForeignKey("dbo.tbPerfil", t => t.IDPerfil, cascadeDelete: true)
                .Index(t => t.IDPerfil)
                .Index(t => t.IDMenu);
            
            CreateTable(
                "dbo.tbUsuario",
                c => new
                    {
                        IDUsuario = c.String(nullable: false, maxLength: 128),
                        CPF = c.String(nullable: false),
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
            DropForeignKey("dbo.tbDepartamento", "IDEmpresa", "dbo.tbEmpresa");
            DropIndex("dbo.tbUsuarioPerfil", new[] { "IDPerfil" });
            DropIndex("dbo.tbUsuarioPerfil", new[] { "IDUsuario" });
            DropIndex("dbo.tbUsuario", new[] { "IDDepartamento" });
            DropIndex("dbo.tbUsuario", new[] { "IDEmpresa" });
            DropIndex("dbo.tbPerfilMenu", new[] { "IDMenu" });
            DropIndex("dbo.tbPerfilMenu", new[] { "IDPerfil" });
            DropIndex("dbo.tbMenu", new[] { "MenuSuperior_IDMenu" });
            DropIndex("dbo.tbDepartamento", new[] { "IDEmpresa" });
            DropTable("dbo.tbUsuarioPerfil");
            DropTable("dbo.tbUsuario");
            DropTable("dbo.tbPerfilMenu");
            DropTable("dbo.tbPerfil");
            DropTable("dbo.tbMenu");
            DropTable("dbo.tbEmpresa");
            DropTable("dbo.tbDepartamento");
            DropTable("dbo.tbContrato");
        }
    }
}
