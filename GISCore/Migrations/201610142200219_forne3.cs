namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forne3 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbFornecedor", "Nome", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbFornecedor", "Nome", c => c.String());
        }
    }
}
