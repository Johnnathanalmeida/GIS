namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class f : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbContrato", "IDEmpresa", "dbo.tbEmpresa");
            DropIndex("dbo.tbContrato", new[] { "IDEmpresa" });
            AddColumn("dbo.tbContrato", "IDFornecedor", c => c.String(nullable: false, maxLength: 128));
            CreateIndex("dbo.tbContrato", "IDFornecedor");
            AddForeignKey("dbo.tbContrato", "IDFornecedor", "dbo.tbFornecedor", "IDFornecedor", cascadeDelete: true);
            DropColumn("dbo.tbContrato", "IDEmpresa");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbContrato", "IDEmpresa", c => c.String(nullable: false, maxLength: 128));
            DropForeignKey("dbo.tbContrato", "IDFornecedor", "dbo.tbFornecedor");
            DropIndex("dbo.tbContrato", new[] { "IDFornecedor" });
            DropColumn("dbo.tbContrato", "IDFornecedor");
            CreateIndex("dbo.tbContrato", "IDEmpresa");
            AddForeignKey("dbo.tbContrato", "IDEmpresa", "dbo.tbEmpresa", "IDEmpresa", cascadeDelete: true);
        }
    }
}
