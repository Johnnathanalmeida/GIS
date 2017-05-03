namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refresh3 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbFornecedor", "Empresa_ID", "dbo.tbEmpresa");
            DropIndex("dbo.tbFornecedor", new[] { "Empresa_ID" });
            DropColumn("dbo.tbFornecedor", "IDEmpresa");
            DropColumn("dbo.tbFornecedor", "Empresa_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbFornecedor", "Empresa_ID", c => c.String(maxLength: 128));
            AddColumn("dbo.tbFornecedor", "IDEmpresa", c => c.String());
            CreateIndex("dbo.tbFornecedor", "Empresa_ID");
            AddForeignKey("dbo.tbFornecedor", "Empresa_ID", "dbo.tbEmpresa", "ID");
        }
    }
}
