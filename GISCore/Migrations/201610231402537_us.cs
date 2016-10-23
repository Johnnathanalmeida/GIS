namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class us : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbUsuarioPerfil", "IDArea", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbUsuarioPerfil", "IDArea");
        }
    }
}
