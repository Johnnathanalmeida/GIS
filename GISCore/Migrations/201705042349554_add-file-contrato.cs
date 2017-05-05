namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addfilecontrato : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbContrato", "NomeArquivoLocal", c => c.String());
            AddColumn("dbo.tbContrato", "NomeArquivoRemoto", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbContrato", "NomeArquivoRemoto");
            DropColumn("dbo.tbContrato", "NomeArquivoLocal");
        }
    }
}
