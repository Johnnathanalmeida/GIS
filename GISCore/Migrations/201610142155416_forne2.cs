namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forne2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbFornecedor", "IDEmpresa", c => c.String(maxLength: 128));
            CreateIndex("dbo.tbFornecedor", "IDEmpresa");
            AddForeignKey("dbo.tbFornecedor", "IDEmpresa", "dbo.tbEmpresa", "IDEmpresa");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbFornecedor", "IDEmpresa", "dbo.tbEmpresa");
            DropIndex("dbo.tbFornecedor", new[] { "IDEmpresa" });
            DropColumn("dbo.tbFornecedor", "IDEmpresa");
        }
    }
}
