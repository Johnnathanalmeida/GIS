namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refresh : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.GDOBJTipoDeDocumento", "CategoriaDeDocumento_ID", c => c.String(maxLength: 128));
            CreateIndex("dbo.GDOBJTipoDeDocumento", "CategoriaDeDocumento_ID");
            AddForeignKey("dbo.GDOBJTipoDeDocumento", "CategoriaDeDocumento_ID", "dbo.GDOBJCategoriaDeDocumento", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.GDOBJTipoDeDocumento", "CategoriaDeDocumento_ID", "dbo.GDOBJCategoriaDeDocumento");
            DropIndex("dbo.GDOBJTipoDeDocumento", new[] { "CategoriaDeDocumento_ID" });
            DropColumn("dbo.GDOBJTipoDeDocumento", "CategoriaDeDocumento_ID");
        }
    }
}
