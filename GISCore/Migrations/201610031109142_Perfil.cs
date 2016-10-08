namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Perfil : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.tbPerfil", "ActionDefault", c => c.String(nullable: false));
            AlterColumn("dbo.tbPerfil", "ControllerDefault", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.tbPerfil", "ControllerDefault", c => c.String());
            AlterColumn("dbo.tbPerfil", "ActionDefault", c => c.String());
        }
    }
}
