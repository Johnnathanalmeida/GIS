namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refresh : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbAdmissao", "Departamento_ID", c => c.String(maxLength: 128));
            AddColumn("dbo.tbAdmissao", "Empresa_ID", c => c.String(maxLength: 128));
            AddColumn("dbo.tbAdmissao", "Fornecedor_ID", c => c.String(maxLength: 128));
            CreateIndex("dbo.tbAdmissao", "Departamento_ID");
            CreateIndex("dbo.tbAdmissao", "Empresa_ID");
            CreateIndex("dbo.tbAdmissao", "Fornecedor_ID");
            AddForeignKey("dbo.tbAdmissao", "Departamento_ID", "dbo.tbDepartamento", "ID");
            AddForeignKey("dbo.tbAdmissao", "Empresa_ID", "dbo.tbEmpresa", "ID");
            AddForeignKey("dbo.tbAdmissao", "Fornecedor_ID", "dbo.tbFornecedor", "ID");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbAdmissao", "Fornecedor_ID", "dbo.tbFornecedor");
            DropForeignKey("dbo.tbAdmissao", "Empresa_ID", "dbo.tbEmpresa");
            DropForeignKey("dbo.tbAdmissao", "Departamento_ID", "dbo.tbDepartamento");
            DropIndex("dbo.tbAdmissao", new[] { "Fornecedor_ID" });
            DropIndex("dbo.tbAdmissao", new[] { "Empresa_ID" });
            DropIndex("dbo.tbAdmissao", new[] { "Departamento_ID" });
            DropColumn("dbo.tbAdmissao", "Fornecedor_ID");
            DropColumn("dbo.tbAdmissao", "Empresa_ID");
            DropColumn("dbo.tbAdmissao", "Departamento_ID");
        }
    }
}
