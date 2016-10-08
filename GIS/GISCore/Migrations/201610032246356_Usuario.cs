namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Usuario : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbMenu", "IDMenuSuperior", c => c.String());
            DropColumn("dbo.tbMenu", "MenuSuperior");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbMenu", "MenuSuperior", c => c.String());
            DropColumn("dbo.tbMenu", "IDMenuSuperior");
        }
    }
}
