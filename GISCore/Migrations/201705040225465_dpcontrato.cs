namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dpcontrato : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.tbContrato", "Departamento_ID", "dbo.tbDepartamento");
            DropIndex("dbo.tbContrato", new[] { "Departamento_ID" });
            AddColumn("dbo.tbDepartamento", "Contrato_ID", c => c.String(maxLength: 128));
            CreateIndex("dbo.tbDepartamento", "Contrato_ID");
            AddForeignKey("dbo.tbDepartamento", "Contrato_ID", "dbo.tbContrato", "ID");
            DropColumn("dbo.tbContrato", "IDDepartamento");
            DropColumn("dbo.tbContrato", "Departamento_ID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.tbContrato", "Departamento_ID", c => c.String(maxLength: 128));
            AddColumn("dbo.tbContrato", "IDDepartamento", c => c.String(nullable: false));
            DropForeignKey("dbo.tbDepartamento", "Contrato_ID", "dbo.tbContrato");
            DropIndex("dbo.tbDepartamento", new[] { "Contrato_ID" });
            DropColumn("dbo.tbDepartamento", "Contrato_ID");
            CreateIndex("dbo.tbContrato", "Departamento_ID");
            AddForeignKey("dbo.tbContrato", "Departamento_ID", "dbo.tbDepartamento", "ID");
        }
    }
}
