namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class refresh1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.tbEmpregado", "Sexo", c => c.Int(nullable: false));
            AddColumn("dbo.tbEmpregado", "TipoEmpregado", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.tbEmpregado", "TipoEmpregado");
            DropColumn("dbo.tbEmpregado", "Sexo");
        }
    }
}
