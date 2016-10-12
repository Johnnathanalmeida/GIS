namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Menu2 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbMenu", "Icone", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbMenu", "Icone");
        }
    }
}
