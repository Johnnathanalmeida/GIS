namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Menu1 : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbMenu", "Ordem", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbMenu", "Ordem", c => c.Int(nullable: false));
        }
    }
}
