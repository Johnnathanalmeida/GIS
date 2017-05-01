namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGarantia : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbFornecedor", "IDEmpresa", c => c.String());
            AddColumn("dbo.tbFornecedor", "Empresa_ID", c => c.String(maxLength: 128));
            CreateIndex("dbo.tbFornecedor", "Empresa_ID");
            AddForeignKey("dbo.tbFornecedor", "Empresa_ID", "dbo.tbEmpresa", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbFornecedor", "Empresa_ID", "dbo.tbEmpresa");
            DropIndex("dbo.tbFornecedor", new[] { "Empresa_ID" });
            DropColumn("dbo.tbFornecedor", "Empresa_ID");
            DropColumn("dbo.tbFornecedor", "IDEmpresa");
        }
    }
}
