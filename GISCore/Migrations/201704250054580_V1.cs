namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class V1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbContrato", "Descricao1", c => c.String());
            DropColumn("dbo.tbContrato", "Descricao");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbContrato", "Descricao", c => c.String());
            DropColumn("dbo.tbContrato", "Descricao1");
        }
    }
}
