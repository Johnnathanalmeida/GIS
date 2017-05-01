namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addGarantia2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbGarantia",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        IDGarantia = c.String(),
                        Intervalo = c.Int(nullable: false),
                        Prazo = c.Int(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Contrato_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbContrato", t => t.Contrato_ID)
                .Index(t => t.Contrato_ID);
            
            CreateTable(
                "dbo.tbDepartamentoContrato",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        IDDepartamentoContrato = c.String(),
                        IDDepartamento = c.String(nullable: false),
                        IDContrato = c.String(nullable: false),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Contrato_ID = c.String(maxLength: 128),
                        Departamento_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbContrato", t => t.Contrato_ID)
                .ForeignKey("dbo.tbDepartamento", t => t.Departamento_ID)
                .Index(t => t.Contrato_ID)
                .Index(t => t.Departamento_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbDepartamentoContrato", "Departamento_ID", "dbo.tbDepartamento");
            DropForeignKey("dbo.tbDepartamentoContrato", "Contrato_ID", "dbo.tbContrato");
            DropForeignKey("dbo.tbGarantia", "Contrato_ID", "dbo.tbContrato");
            DropIndex("dbo.tbDepartamentoContrato", new[] { "Departamento_ID" });
            DropIndex("dbo.tbDepartamentoContrato", new[] { "Contrato_ID" });
            DropIndex("dbo.tbGarantia", new[] { "Contrato_ID" });
            DropTable("dbo.tbDepartamentoContrato");
            DropTable("dbo.tbGarantia");
        }
    }
}
