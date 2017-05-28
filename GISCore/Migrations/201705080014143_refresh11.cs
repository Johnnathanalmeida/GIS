namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refresh11 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.tbCargo",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        IDCargo = c.String(),
                        Carg_Nome = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.tbFuncao",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        IDCargo = c.String(),
                        IDFuncao = c.String(),
                        Func_Nome = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Cargo_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.tbCargo", t => t.Cargo_ID)
                .Index(t => t.Cargo_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.tbFuncao", "Cargo_ID", "dbo.tbCargo");
            DropIndex("dbo.tbFuncao", new[] { "Cargo_ID" });
            DropTable("dbo.tbFuncao");
            DropTable("dbo.tbCargo");
        }
    }
}
