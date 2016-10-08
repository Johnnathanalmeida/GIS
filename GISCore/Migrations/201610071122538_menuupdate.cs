namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class menuupdate : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbMenu", "ActionDefault", c => c.String());
            AddColumn("dbo.tbMenu", "ControllerDefault", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbMenu", "ControllerDefault");
            DropColumn("dbo.tbMenu", "ActionDefault");
        }
    }
}
