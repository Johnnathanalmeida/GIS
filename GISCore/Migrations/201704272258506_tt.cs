namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class tt : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbEmpregado", "Empresa_ID", "dbo.tbEmpresa");
            DropForeignKey("dbo.tbEmpregado", "Fornecedor_ID", "dbo.tbFornecedor");
            DropIndex("dbo.tbEmpregado", new[] { "Empresa_ID" });
            DropIndex("dbo.tbEmpregado", new[] { "Fornecedor_ID" });
            AlterColumn("dbo.tbEmpresa", "CNPJ", c => c.String(nullable: false));
            AlterColumn("dbo.tbFornecedor", "CNPJ", c => c.String(nullable: false));
            AlterColumn("dbo.tbEmpregado", "CPF", c => c.String(nullable: false));
            AlterColumn("dbo.tbUsuario", "CPF", c => c.String(nullable: false));
            DropColumn("dbo.tbEmpregado", "IDEmpresa");
            DropColumn("dbo.tbEmpregado", "IDFornecedor");
            DropColumn("dbo.tbEmpregado", "Empresa_ID");
            DropColumn("dbo.tbEmpregado", "Fornecedor_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbEmpregado", "Fornecedor_ID", c => c.String(maxLength: 128));
            AddColumn("dbo.tbEmpregado", "Empresa_ID", c => c.String(maxLength: 128));
            AddColumn("dbo.tbEmpregado", "IDFornecedor", c => c.String());
            AddColumn("dbo.tbEmpregado", "IDEmpresa", c => c.String());
            AlterColumn("dbo.tbUsuario", "CPF", c => c.String());
            AlterColumn("dbo.tbEmpregado", "CPF", c => c.String());
            AlterColumn("dbo.tbFornecedor", "CNPJ", c => c.String());
            AlterColumn("dbo.tbEmpresa", "CNPJ", c => c.String());
            CreateIndex("dbo.tbEmpregado", "Fornecedor_ID");
            CreateIndex("dbo.tbEmpregado", "Empresa_ID");
            AddForeignKey("dbo.tbEmpregado", "Fornecedor_ID", "dbo.tbFornecedor", "ID");
            AddForeignKey("dbo.tbEmpregado", "Empresa_ID", "dbo.tbEmpresa", "ID");
        }
    }
}
