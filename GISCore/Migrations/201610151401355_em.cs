namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class em : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbEmpregadoEmpresa",
                c => new
                    {
                        IDEmpregadoEmpresa = c.String(nullable: false, maxLength: 128),
                        IDEmpregado = c.String(nullable: false, maxLength: 128),
                        IDEmpresa = c.String(maxLength: 128),
                        IDFornecedor = c.String(maxLength: 128),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDEmpregadoEmpresa)
                .ForeignKey("dbo.tbEmpresa", t => t.IDEmpresa)
                .ForeignKey("dbo.tbEmpregado", t => t.IDEmpregado, cascadeDelete: true)
                .ForeignKey("dbo.tbFornecedor", t => t.IDFornecedor)
                .Index(t => t.IDEmpregado)
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbEmpresaFornecedor", "IDFornecedor", "dbo.tbFornecedor");
            DropForeignKey("dbo.tbEmpresaFornecedor", "IDEmpresa", "dbo.tbEmpresa");
            DropForeignKey("dbo.tbEmpregadoEmpresa", "IDFornecedor", "dbo.tbFornecedor");
            DropForeignKey("dbo.tbEmpregadoEmpresa", "IDEmpregado", "dbo.tbEmpregado");
            DropForeignKey("dbo.tbEmpregadoEmpresa", "IDEmpresa", "dbo.tbEmpresa");
            DropIndex("dbo.tbEmpresaFornecedor", new[] { "IDFornecedor" });
            DropIndex("dbo.tbEmpresaFornecedor", new[] { "IDEmpresa" });
            DropIndex("dbo.tbEmpregadoEmpresa", new[] { "IDFornecedor" });
            DropIndex("dbo.tbEmpregadoEmpresa", new[] { "IDEmpresa" });
            DropIndex("dbo.tbEmpregadoEmpresa", new[] { "IDEmpregado" });
            DropTable("dbo.tbEmpresaFornecedor");
            DropTable("dbo.tbEmpregadoEmpresa");
        }
    }
}
