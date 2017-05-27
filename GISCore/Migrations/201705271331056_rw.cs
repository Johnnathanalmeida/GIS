namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class rw : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GDOBJTipoDeDocumento", "PermiteMultiplosArquivos", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.GDOBJTipoDeDocumento", "PermiteMultiplosArquivos");
        }
    }
}
