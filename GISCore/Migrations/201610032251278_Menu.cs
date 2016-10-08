namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Menu : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbMenu", "MenuSuperior_IDMenu", c => c.String(maxLength: 128));
            CreateIndex("dbo.tbMenu", "MenuSuperior_IDMenu");
            AddForeignKey("dbo.tbMenu", "MenuSuperior_IDMenu", "dbo.tbMenu", "IDMenu");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbMenu", "MenuSuperior_IDMenu", "dbo.tbMenu");
            DropIndex("dbo.tbMenu", new[] { "MenuSuperior_IDMenu" });
            DropColumn("dbo.tbMenu", "MenuSuperior_IDMenu");
        }
    }
}
