namespace GISCore.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class newdb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.OBJAdmissao",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        UKEmpregado = c.String(),
                        UKUsuarioDemissao = c.String(),
                        UKFornecedor = c.String(),
                        UKEmpresa = c.String(),
                        UKDepartamento = c.String(),
                        DataAdmissao = c.DateTime(nullable: false),
                        DataDemissao = c.DateTime(nullable: false),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OBJAlocacao",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        UKEmpregado = c.String(nullable: false),
                        UKContrato = c.String(nullable: false),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Contrato_ID = c.String(maxLength: 128),
                        Empregado_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OBJContrato", t => t.Contrato_ID)
                .ForeignKey("dbo.OBJEmpregado", t => t.Empregado_ID)
                .Index(t => t.Contrato_ID)
                .Index(t => t.Empregado_ID);
            
            CreateTable(
                "dbo.OBJContrato",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Numero = c.String(nullable: false),
                        Inicio = c.DateTime(nullable: false),
                        Fim = c.DateTime(nullable: false),
                        Descricao = c.String(),
                        UKFornecedor = c.String(nullable: false),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Fornecedor_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OBJFornecedor", t => t.Fornecedor_ID)
                .Index(t => t.Fornecedor_ID);
            
            CreateTable(
                "dbo.OBJDepartamento",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Codigo = c.String(nullable: false),
                        Sigla = c.String(nullable: false),
                        Descricao = c.String(),
                        Status = c.Int(nullable: false),
                        UKEmpresa = c.String(nullable: false),
                        UKDepartamentoVinculado = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Empresa_ID = c.String(maxLength: 128),
                        Contrato_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OBJEmpresa", t => t.Empresa_ID)
                .ForeignKey("dbo.OBJContrato", t => t.Contrato_ID)
                .Index(t => t.Empresa_ID)
                .Index(t => t.Contrato_ID);
            
            CreateTable(
                "dbo.OBJEmpresa",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        CNPJ = c.String(nullable: false),
                        RazaoSocial = c.String(),
                        NomeFantasia = c.String(nullable: false),
                        URL_Site = c.String(),
                        URL_LogoMarca = c.String(nullable: false),
                        URL_WS = c.String(),
                        URL_AD = c.String(),
                        UKArquivo150x20 = c.String(),
                        UKArquivo230x50 = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OBJFornecedor",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Numero = c.String(),
                        Nome = c.String(nullable: false),
                        CNPJ = c.String(nullable: false),
                        Endereco = c.String(),
                        Telefone = c.String(),
                        Email = c.String(nullable: false),
                        Descricao = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OBJGarantia",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Descricao = c.String(nullable: false),
                        Intervalo = c.Int(nullable: false),
                        Prazo = c.Int(nullable: false),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Contrato_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OBJContrato", t => t.Contrato_ID)
                .Index(t => t.Contrato_ID);
            
            CreateTable(
                "dbo.OBJEmpregado",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        CPF = c.String(nullable: false),
                        Nome = c.String(nullable: false),
                        Sexo = c.Int(nullable: false),
                        DataNascimento = c.DateTime(nullable: false),
                        Email = c.String(nullable: false),
                        Endereco = c.String(),
                        Telefone = c.String(),
                        TipoEmpregado = c.Int(nullable: false),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OBJArquivo",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        UKObjeto = c.String(),
                        UKCategoria = c.String(),
                        UKTipo = c.String(),
                        NomeLocal = c.String(),
                        NomeRemoto = c.String(),
                        Complemento = c.String(),
                        Comentario = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OBJCargo",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Carg_Nome = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.OBJFuncao",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        UKCargo = c.String(),
                        Func_Nome = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Cargo_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OBJCargo", t => t.Cargo_ID)
                .Index(t => t.Cargo_ID);
            
            CreateTable(
                "dbo.RELContratoArquivo",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        UKContrato = c.String(nullable: false),
                        UKArquivo = c.String(nullable: false),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Arquivo_ID = c.String(maxLength: 128),
                        Contrato_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OBJArquivo", t => t.Arquivo_ID)
                .ForeignKey("dbo.OBJContrato", t => t.Contrato_ID)
                .Index(t => t.Arquivo_ID)
                .Index(t => t.Contrato_ID);
            
            CreateTable(
                "dbo.RELContratoGarantia",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        UKContrato = c.String(nullable: false),
                        UKGarantia = c.String(nullable: false),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Contrato_ID = c.String(maxLength: 128),
                        Garantia_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OBJContrato", t => t.Contrato_ID)
                .ForeignKey("dbo.OBJGarantia", t => t.Garantia_ID)
                .Index(t => t.Contrato_ID)
                .Index(t => t.Garantia_ID);
            
            CreateTable(
                "dbo.RELDepartamentoContrato",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        UKDepartamento = c.String(nullable: false),
                        UKContrato = c.String(nullable: false),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Contrato_ID = c.String(maxLength: 128),
                        Departamento_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OBJContrato", t => t.Contrato_ID)
                .ForeignKey("dbo.OBJDepartamento", t => t.Departamento_ID)
                .Index(t => t.Contrato_ID)
                .Index(t => t.Departamento_ID);
            
            CreateTable(
                "dbo.OBJEstabelecimento",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Nome = c.String(nullable: false),
                        Descricao = c.String(),
                        Endereco = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.RELEstabelecimentoContrato",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        UKEstabelecimento = c.String(nullable: false),
                        UKContrato = c.String(nullable: false),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Contrato_ID = c.String(maxLength: 128),
                        Estabelecimento_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OBJContrato", t => t.Contrato_ID)
                .ForeignKey("dbo.OBJEstabelecimento", t => t.Estabelecimento_ID)
                .Index(t => t.Contrato_ID)
                .Index(t => t.Estabelecimento_ID);
            
            CreateTable(
                "dbo.OBJMenu",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Nome = c.String(nullable: false),
                        Action = c.String(),
                        Controller = c.String(),
                        Ordem = c.String(nullable: false),
                        Icone = c.String(),
                        UKMenuSuperior = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        MenuSuperior_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OBJMenu", t => t.MenuSuperior_ID)
                .Index(t => t.MenuSuperior_ID);
            
            CreateTable(
                "dbo.OBJPerfil",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        Nome = c.String(nullable: false),
                        Descricao = c.String(),
                        ActionDefault = c.String(nullable: false),
                        ControllerDefault = c.String(nullable: false),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.RELPerfilMenu",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        UKPerfil = c.String(nullable: false),
                        UKMenu = c.String(nullable: false),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Menu_ID = c.String(maxLength: 128),
                        Perfil_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OBJMenu", t => t.Menu_ID)
                .ForeignKey("dbo.OBJPerfil", t => t.Perfil_ID)
                .Index(t => t.Menu_ID)
                .Index(t => t.Perfil_ID);
            
            CreateTable(
                "dbo.OBJUsuario",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        CPF = c.String(nullable: false),
                        Nome = c.String(nullable: false),
                        Login = c.String(nullable: false),
                        Senha = c.String(),
                        Email = c.String(nullable: false),
                        UKEmpresa = c.String(nullable: false),
                        UKDepartamento = c.String(nullable: false),
                        TipoDeAcesso = c.Int(nullable: false),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Departamento_ID = c.String(maxLength: 128),
                        Empresa_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OBJDepartamento", t => t.Departamento_ID)
                .ForeignKey("dbo.OBJEmpresa", t => t.Empresa_ID)
                .Index(t => t.Departamento_ID)
                .Index(t => t.Empresa_ID);
            
            CreateTable(
                "dbo.RELUsuarioPerfil",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 128),
                        UKUsuario = c.String(nullable: false),
                        UKPerfil = c.String(nullable: false),
                        UKArea = c.String(),
                        UniqueKey = c.String(),
                        UsuarioInclusao = c.String(),
                        DataInclusao = c.DateTime(nullable: false),
                        UsuarioExclusao = c.String(),
                        DataExclusao = c.DateTime(nullable: false),
                        Perfil_ID = c.String(maxLength: 128),
                        Usuario_ID = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.OBJPerfil", t => t.Perfil_ID)
                .ForeignKey("dbo.OBJUsuario", t => t.Usuario_ID)
                .Index(t => t.Perfil_ID)
                .Index(t => t.Usuario_ID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.RELUsuarioPerfil", "Usuario_ID", "dbo.OBJUsuario");
            DropForeignKey("dbo.RELUsuarioPerfil", "Perfil_ID", "dbo.OBJPerfil");
            DropForeignKey("dbo.OBJUsuario", "Empresa_ID", "dbo.OBJEmpresa");
            DropForeignKey("dbo.OBJUsuario", "Departamento_ID", "dbo.OBJDepartamento");
            DropForeignKey("dbo.RELPerfilMenu", "Perfil_ID", "dbo.OBJPerfil");
            DropForeignKey("dbo.RELPerfilMenu", "Menu_ID", "dbo.OBJMenu");
            DropForeignKey("dbo.OBJMenu", "MenuSuperior_ID", "dbo.OBJMenu");
            DropForeignKey("dbo.RELEstabelecimentoContrato", "Estabelecimento_ID", "dbo.OBJEstabelecimento");
            DropForeignKey("dbo.RELEstabelecimentoContrato", "Contrato_ID", "dbo.OBJContrato");
            DropForeignKey("dbo.RELDepartamentoContrato", "Departamento_ID", "dbo.OBJDepartamento");
            DropForeignKey("dbo.RELDepartamentoContrato", "Contrato_ID", "dbo.OBJContrato");
            DropForeignKey("dbo.RELContratoGarantia", "Garantia_ID", "dbo.OBJGarantia");
            DropForeignKey("dbo.RELContratoGarantia", "Contrato_ID", "dbo.OBJContrato");
            DropForeignKey("dbo.RELContratoArquivo", "Contrato_ID", "dbo.OBJContrato");
            DropForeignKey("dbo.RELContratoArquivo", "Arquivo_ID", "dbo.OBJArquivo");
            DropForeignKey("dbo.OBJFuncao", "Cargo_ID", "dbo.OBJCargo");
            DropForeignKey("dbo.OBJAlocacao", "Empregado_ID", "dbo.OBJEmpregado");
            DropForeignKey("dbo.OBJAlocacao", "Contrato_ID", "dbo.OBJContrato");
            DropForeignKey("dbo.OBJGarantia", "Contrato_ID", "dbo.OBJContrato");
            DropForeignKey("dbo.OBJContrato", "Fornecedor_ID", "dbo.OBJFornecedor");
            DropForeignKey("dbo.OBJDepartamento", "Contrato_ID", "dbo.OBJContrato");
            DropForeignKey("dbo.OBJDepartamento", "Empresa_ID", "dbo.OBJEmpresa");
            DropIndex("dbo.RELUsuarioPerfil", new[] { "Usuario_ID" });
            DropIndex("dbo.RELUsuarioPerfil", new[] { "Perfil_ID" });
            DropIndex("dbo.OBJUsuario", new[] { "Empresa_ID" });
            DropIndex("dbo.OBJUsuario", new[] { "Departamento_ID" });
            DropIndex("dbo.RELPerfilMenu", new[] { "Perfil_ID" });
            DropIndex("dbo.RELPerfilMenu", new[] { "Menu_ID" });
            DropIndex("dbo.OBJMenu", new[] { "MenuSuperior_ID" });
            DropIndex("dbo.RELEstabelecimentoContrato", new[] { "Estabelecimento_ID" });
            DropIndex("dbo.RELEstabelecimentoContrato", new[] { "Contrato_ID" });
            DropIndex("dbo.RELDepartamentoContrato", new[] { "Departamento_ID" });
            DropIndex("dbo.RELDepartamentoContrato", new[] { "Contrato_ID" });
            DropIndex("dbo.RELContratoGarantia", new[] { "Garantia_ID" });
            DropIndex("dbo.RELContratoGarantia", new[] { "Contrato_ID" });
            DropIndex("dbo.RELContratoArquivo", new[] { "Contrato_ID" });
            DropIndex("dbo.RELContratoArquivo", new[] { "Arquivo_ID" });
            DropIndex("dbo.OBJFuncao", new[] { "Cargo_ID" });
            DropIndex("dbo.OBJGarantia", new[] { "Contrato_ID" });
            DropIndex("dbo.OBJDepartamento", new[] { "Contrato_ID" });
            DropIndex("dbo.OBJDepartamento", new[] { "Empresa_ID" });
            DropIndex("dbo.OBJContrato", new[] { "Fornecedor_ID" });
            DropIndex("dbo.OBJAlocacao", new[] { "Empregado_ID" });
            DropIndex("dbo.OBJAlocacao", new[] { "Contrato_ID" });
            DropTable("dbo.RELUsuarioPerfil");
            DropTable("dbo.OBJUsuario");
            DropTable("dbo.RELPerfilMenu");
            DropTable("dbo.OBJPerfil");
            DropTable("dbo.OBJMenu");
            DropTable("dbo.RELEstabelecimentoContrato");
            DropTable("dbo.OBJEstabelecimento");
            DropTable("dbo.RELDepartamentoContrato");
            DropTable("dbo.RELContratoGarantia");
            DropTable("dbo.RELContratoArquivo");
            DropTable("dbo.OBJFuncao");
            DropTable("dbo.OBJCargo");
            DropTable("dbo.OBJArquivo");
            DropTable("dbo.OBJEmpregado");
            DropTable("dbo.OBJGarantia");
            DropTable("dbo.OBJFornecedor");
            DropTable("dbo.OBJEmpresa");
            DropTable("dbo.OBJDepartamento");
            DropTable("dbo.OBJContrato");
            DropTable("dbo.OBJAlocacao");
            DropTable("dbo.OBJAdmissao");
        }
    }
}
