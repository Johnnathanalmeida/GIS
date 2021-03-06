namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class cont : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbArquivo",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        NomeLocal = c.String(),
                        NomeRemoto = c.String(),
                        Versao = c.Int(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            AddColumn("dbo.tbContrato", "IDArquivo", c => c.String());
            AddColumn("dbo.tbContrato", "Arquivo_ID", c => c.String(maxLength: 128));
            AddColumn("dbo.tbEstabelecimento", "IDArquivo", c => c.String());
            AddColumn("dbo.tbEstabelecimento", "Arquivo_ID", c => c.String(maxLength: 128));
            AlterColumn("dbo.tbGarantia", "Descricao", c => c.String(nullable: false));
            AlterColumn("dbo.tbEstabelecimento", "Nome", c => c.String(nullable: false));
            CreateIndex("dbo.tbContrato", "Arquivo_ID");
            CreateIndex("dbo.tbEstabelecimento", "Arquivo_ID");
            AddForeignKey("dbo.tbContrato", "Arquivo_ID", "dbo.tbArquivo", "ID");
            AddForeignKey("dbo.tbEstabelecimento", "Arquivo_ID", "dbo.tbArquivo", "ID");
            DropColumn("dbo.tbContrato", "NomeArquivoLocal");
            DropColumn("dbo.tbContrato", "NomeArquivoRemoto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbContrato", "NomeArquivoRemoto", c => c.String());
            AddColumn("dbo.tbContrato", "NomeArquivoLocal", c => c.String());
            DropForeignKey("dbo.tbEstabelecimento", "Arquivo_ID", "dbo.tbArquivo");
            DropForeignKey("dbo.tbContrato", "Arquivo_ID", "dbo.tbArquivo");
            DropIndex("dbo.tbEstabelecimento", new[] { "Arquivo_ID" });
            DropIndex("dbo.tbContrato", new[] { "Arquivo_ID" });
            AlterColumn("dbo.tbEstabelecimento", "Nome", c => c.String());
            AlterColumn("dbo.tbGarantia", "Descricao", c => c.String());
            DropColumn("dbo.tbEstabelecimento", "Arquivo_ID");
            DropColumn("dbo.tbEstabelecimento", "IDArquivo");
            DropColumn("dbo.tbContrato", "Arquivo_ID");
            DropColumn("dbo.tbContrato", "IDArquivo");
            DropTable("dbo.tbArquivo");
        }
    }
}
