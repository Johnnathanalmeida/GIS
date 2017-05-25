namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ref1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GDOBJTipoDeDocumento", "UKCategoriaDeDocumento", c => c.String());
            DropColumn("dbo.GDOBJTipoDeDocumento", "UKCategoria");
        }
        
        public override void Down()
        {
            AddColumn("dbo.GDOBJTipoDeDocumento", "UKCategoria", c => c.String());
            DropColumn("dbo.GDOBJTipoDeDocumento", "UKCategoriaDeDocumento");
        }
    }
}
