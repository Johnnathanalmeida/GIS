namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class a : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbEmpregadoEmpresa", "IDEmpresa", "dbo.tbEmpresa");
            DropForeignKey("dbo.tbEmpregadoEmpresa", "IDEmpregado", "dbo.tbEmpregado");
            DropForeignKey("dbo.tbEmpregadoEmpresa", "IDFornecedor", "dbo.tbFornecedor");
            DropIndex("dbo.tbEmpregadoEmpresa", new[] { "IDEmpregado" });
            DropIndex("dbo.tbEmpregadoEmpresa", new[] { "IDEmpresa" });
            DropIndex("dbo.tbEmpregadoEmpresa", new[] { "IDFornecedor" });
            DropTable("dbo.tbEmpregadoEmpresa");
        }
        
        public override void Down()
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
                .PrimaryKey(t => t.IDEmpregadoEmpresa);
            
            CreateIndex("dbo.tbEmpregadoEmpresa", "IDFornecedor");
            CreateIndex("dbo.tbEmpregadoEmpresa", "IDEmpresa");
            CreateIndex("dbo.tbEmpregadoEmpresa", "IDEmpregado");
            AddForeignKey("dbo.tbEmpregadoEmpresa", "IDFornecedor", "dbo.tbFornecedor", "IDFornecedor");
            AddForeignKey("dbo.tbEmpregadoEmpresa", "IDEmpregado", "dbo.tbEmpregado", "IDEmpregado", cascadeDelete: true);
            AddForeignKey("dbo.tbEmpregadoEmpresa", "IDEmpresa", "dbo.tbEmpresa", "IDEmpresa");
        }
    }
}
