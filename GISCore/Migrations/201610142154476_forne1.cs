namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class forne1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbFornecedor",
                c => new
                    {
                        IDFornecedor = c.String(nullable: false, maxLength: 128),
                        Numero = c.String(),
                        Nome = c.String(),
                        CNPJ = c.String(),
                        Endereco = c.String(),
                        Telefone = c.String(),
                        Email = c.String(nullable: false),
                        Descricao = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.IDFornecedor);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.tbFornecedor");
        }
    }
}
