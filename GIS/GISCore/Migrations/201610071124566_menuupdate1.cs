namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class menuupdate1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbMenu", "Action", c => c.String());
            AddColumn("dbo.tbMenu", "Controller", c => c.String());
            DropColumn("dbo.tbMenu", "ActionDefault");
            DropColumn("dbo.tbMenu", "ControllerDefault");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbMenu", "ControllerDefault", c => c.String());
            AddColumn("dbo.tbMenu", "ActionDefault", c => c.String());
            DropColumn("dbo.tbMenu", "Controller");
            DropColumn("dbo.tbMenu", "Action");
        }
    }
}
