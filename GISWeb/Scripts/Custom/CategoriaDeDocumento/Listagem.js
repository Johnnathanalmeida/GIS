jQuery(function ($) {
    $('.dd').nestable();
    $('.dd').nestable('collapseAll');

    $('.dd-handle a').on('mousedown', function (e) {
        e.stopPropagation();
    });

    $('[data-rel="tooltip"]').tooltip();

    $("#modalTipoDeDocumentoCadastrar").on("click", function () {
        $("#formCadastroTipoDeDocumento").submit();
    });

    $("#modalTipoDeDocumentoAtualizar").on("click", function () {
        $("#formEdicaoTipoDeDocumento").submit();
    });
});

function btnNovaCategoria() {
    var sHTML = "<table style='line-height: 2'>";

    sHTML += "<tr>";
    sHTML += "<td width='150px'>Categoria:</td>";
    sHTML += "<td width='136px' align='left'>";
    sHTML += "  <input type='text' maxlength='64' id='txtCategoriaNome' value='' style='width: 450px;'/>";
    sHTML += "</td>";
    sHTML += "</tr>";
    sHTML += "</table>";

    bootbox.dialog({
        title: "<span class='bigger-110'>Informe os dados da nova Categoria!</span>",
        message: sHTML,
        buttons:
                {
                    "success":
                    {
                        "label": "Cancelar",
                        "className": "btn-sm btn-danger btnReprovar",
                        "callback": function () {
                        }
                    },
                    "danger":
                    {
                        "label": "Salvar",
                        "className": "btn-sm btn-success btnAprovar",
                        "callback": function () {
                            var pCategoria = $("#txtCategoriaNome").val();
                            $.ajax({
                                method: "POST",
                                url: "/CategoriaDeDocumento/CadastrarCategoria",
                                data: { Categoria: pCategoria },
                                error: function (erro) {
                                    ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
                                },
                                success: function (content) {
                                    TratarResultadoJSON(content.resultado);
                                }
                            });
                        }
                    }
                }
    });
}

function AlterarCategoria(pUKCategoria, pCategoria) {
    var sHTML = "<table style='line-height: 2'>";

    sHTML += "<tr>";
    sHTML += "<td width='150px'>Categoria:</td>";
    sHTML += "<td width='136px' align='left'>";
    sHTML += "  <input type='text' maxlength='64' id='txtCategoriaNome' value='" + pCategoria + "' style='width: 450px;'/>";
    sHTML += "</td>";
    sHTML += "</tr>";
    sHTML += "</table>";

    bootbox.dialog({
        title: "<span class='bigger-110'>Informe os dados para atualizar a Categoria!</span>",
        message: sHTML,
        buttons:
                {
                    "success":
                    {
                        "label": "Cancelar",
                        "className": "btn-sm btn-danger btnReprovar",
                        "callback": function () {
                        }
                    },
                    "danger":
                    {
                        "label": "Salvar",
                        "className": "btn-sm btn-success btnAprovar",
                        "callback": function () {
                            var pCategoria = $("#txtCategoriaNome").val();
                            $.ajax({
                                method: "POST",
                                url: "/CategoriaDeDocumento/AlterarCategoria",
                                data: { UKCategoria: pUKCategoria, CategoriaNome: pCategoria },
                                error: function (erro) {
                                    ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
                                },
                                success: function (content) {
                                    TratarResultadoJSON(content.resultado);
                                }
                            });
                        }
                    }
                }
    });
}

function DeletarCategoria(IDCategoria, CategoriaNome) {
    bootbox.confirm({
        backdrop: true,
        message: "Tem certeza que deseja excluir a Categoria '" + CategoriaNome + "'?",
        title: "Confirmação para excluir.",
        buttons: {
            confirm: {
                label: "Sim",
                className: "btn-success btn-sm",
            },
            cancel: {
                label: "Não",
                className: "btn-sm",
            }
        },
        callback: function (result) {
            $.ajax({
                method: "POST",
                url: "/CategoriaDeDocumento/DeletarCategoria",
                data: { IDCategoria: IDCategoria },
                error: function (erro) {
                    ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
                },
                success: function (content) {
                    TratarResultadoJSON(content.resultado);
                }
            });
        }
    });
}

function CadastrarTipo(pUKCategoria) {
    $('#modalTipoDeDocumento').modal('show');
    $('#modalTipoDeDocumentoX').hide();
    $('#modalTipoDeDocumentoFechar').removeClass('disabled');
    $('#modalTipoDeDocumentoFechar').removeAttr('disabled', 'disabled');
    $('#modalTipoDeDocumentoCadastrar').removeClass('disabled');
    $('#modalTipoDeDocumentoCadastrar').removeAttr('disabled', 'disabled');
    $('#modalTipoDeDocumentoAtualizar').hide();
    $('#modalTipoDeDocumentoCorpo').html('');
    $('#modalTipoDeDocumentoCorpoConfirmar').hide();
    $('#modalTipoDeDocumentoCorpoLoading').hide();

    $.ajax({
        method: "GET",
        url: "/TipoDeDocumento/Novo",
        data: { UKCategoria: pUKCategoria },
        error: function (erro) {
            $('#modalTipoDeDocumento').modal('hide');
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $('#modalTipoDeDocumentoCorpo').html(content);
            $("#cbObrigatorio").css("opacity", "1");
            $("#cbObrigatorio").css("position", "initial");
            $("#cbObrigatorio").css("margin-top", "8px");
        },
    });
};

function AtualizarTipo(pUKTipo) {
    $('#modalTipoDeDocumento').modal('show');
    $('#modalTipoDeDocumentoX').hide();
    $('#modalTipoDeDocumentoHeader').html('<i class="ace-icon fa fa-legal green"></i> Cadastro de Tipo de Documento...');
    $('#modalTipoDeDocumentoFechar').removeClass('disabled');
    $('#modalTipoDeDocumentoFechar').removeAttr('disabled', 'disabled');
    $('#modalTipoDeDocumentoCadastrar').hide();
    $('#modalTipoDeDocumentoAtualizar').show();
    $('#modalTipoDeDocumentoAtualizar').removeClass('disabled');
    $('#modalTipoDeDocumentoAtualizar').removeAttr('disabled', 'disabled');
    $('#modalTipoDeDocumentoCorpo').html('');
    $('#modalTipoDeDocumentoCorpoConfirmar').hide();
    $('#modalTipoDeDocumentoCorpoLoading').hide();

    $.ajax({
        method: "GET",
        url: "/TipoDeDocumento/Edicao",
        data: { UKTipo: pUKTipo },
        error: function (erro) {
            $('#modalTipoDeDocumento').modal('hide');
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $('#modalTipoDeDocumentoCorpo').html(content);
            $("#cbObrigatorio").css("opacity", "1");
            $("#cbObrigatorio").css("position", "initial");
            $("#cbObrigatorio").css("margin-top", "8px");
        },
    });
};

function DetalharTipo(pUKTipo) {
    $('#modalTipoDeDocumento').modal('show');
    $('#modalTipoDeDocumentoHeader').html('<i class="ace-icon fa fa-legal green"></i> Cadastro de Tipo de Documento...');
    $('#modalTipoDeDocumentoX').hide();
    $('#modalTipoDeDocumentoFechar').removeClass('disabled');
    $('#modalTipoDeDocumentoFechar').removeAttr('disabled', 'disabled');
    $('#modalTipoDeDocumentoProsseguir').hide();
    $('#modalTipoDeDocumentoCorpo').html('');
    $('#modalTipoDeDocumentoCorpoConfirmar').hide();
    $('#modalTipoDeDocumentoCorpoLoading').hide();

    $.ajax({
        method: "GET",
        url: "/TipoDeDocumento/Detalhes",
        data: { UKTipo: pUKTipo },
        error: function (erro) {
            $('#modalTipoDeDocumento').modal('hide');
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $('#modalTipoDeDocumentoCorpo').html(content);
            $("#cbObrigatorio").css("opacity", "1");
            $("#cbObrigatorio").css("position", "initial");
            $("#cbObrigatorio").css("margin-top", "8px");
        },
    });
};

function DeletarTipo(IDTipo, TipoNome) {
    bootbox.confirm({
        backdrop: true,
        message: "Tem certeza que deseja excluir o Tipo '" + TipoNome + "'?",
        title: "Confirmação para excluir.",
        buttons: {
            confirm: {
                label: "Sim",
                className: "btn-success btn-sm",
            },
            cancel: {
                label: "Não",
                className: "btn-sm",
            }
        },
        callback: function (result) {
            $.ajax({
                method: "POST",
                url: "/TipoDeDocumento/DeletarTipo",
                data: { IDTipo: IDTipo },
                error: function (erro) {
                    ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
                },
                success: function (content) {
                    TratarResultadoJSON(content.resultado);
                }
            });
        }
    });
}

function OnSuccessCadastrarTipoDeDocumento(data) {
    $('#modalTipoDeDocumento').hide();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarTipoDeDocumento() {
    $('#modalTipoDeDocumento').modal('show');
    $('#modalTipoDeDocumentoX').hide();
    $('#modalTipoDeDocumentoFechar').addClass('disabled');
    $('#modalTipoDeDocumentoFechar').attr('disabled', 'disabled');
    $('#modalTipoDeDocumentoCadastrar').addClass('disabled');
    $('#modalTipoDeDocumentoCadastrar').attr('disabled', 'disabled');
    $('#modalTipoDeDocumentoAtualizar').hide();
    $('#modalTipoDeDocumentoCorpo').hide();
    $('#modalTipoDeDocumentoCorpoConfirmar').hide();
    $('#modalTipoDeDocumentoCorpoLoading').show();
}

function OnSuccessAtualizarTipoDeDocumento(data) {
    $('#modalTipoDeDocumento').hide();
    TratarResultadoJSON(data.resultado);
}

function OnBeginAtualizarTipoDeDocumento() {
    $('#modalTipoDeDocumento').modal('show');
    $('#modalTipoDeDocumentoX').hide();
    $('#modalTipoDeDocumentoFechar').addClass('disabled');
    $('#modalTipoDeDocumentoFechar').attr('disabled', 'disabled');
    $('#modalTipoDeDocumentoCadastrar').hide();
    $('#modalTipoDeDocumentoAtualizar').addClass('disabled');
    $('#modalTipoDeDocumentoAtualizar').attr('disabled', 'disabled');    
    $('#modalTipoDeDocumentoCorpo').hide();
    $('#modalTipoDeDocumentoCorpoConfirmar').hide();
    $('#modalTipoDeDocumentoCorpoLoading').show();
}

function GerenciarDepartamentos(pUKTipo, pUKEmpresa) {
    $('#modalTipoDeDocumento').modal('show');
    $('#modalTipoDeDocumentoX').hide();
    $('#modalTipoDeDocumentoHeader').html('<i class="ace-icon fa fa-cubes orange"></i> Relacionar Departamentos...');
    $('#modalTipoDeDocumentoFechar').removeClass('disabled');
    $('#modalTipoDeDocumentoFechar').removeAttr('disabled', 'disabled');
    $('#modalTipoDeDocumentoCadastrar').removeClass('disabled');
    $('#modalTipoDeDocumentoCadastrar').removeAttr('disabled', 'disabled');
    $('#modalTipoDeDocumentoAtualizar').hide();
    $('#modalTipoDeDocumentoCorpo').html('');
    $('#modalTipoDeDocumentoCorpoConfirmar').hide();
    $('#modalTipoDeDocumentoCorpoLoading').hide();

    $.ajax({
        method: "GET",
        url: "/TipoDeDocumento/GerenciarDepartamentos",
        data: { UKTipo: pUKTipo },
        error: function (erro) {
            alert(erro.responseText);
            $('#modalTipoDeDocumento').modal('hide');
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            CarregarDepartamentos(pUKEmpresa)
            $('#modalTipoDeDocumentoCorpo').html(content);
        },
    });
}

function OnSuccessGerenciarDpto(data) {
    alert();
    $('#modalTipoDeDocumento').hide();
    TratarResultadoJSON(data.resultado);
}

function OnBeginGerenciarDpto() {
    alert('ww');
    $('#modalTipoDeDocumento').modal('show');
    $('#modalTipoDeDocumentoX').hide();
    $('#modalTipoDeDocumentoFechar').addClass('disabled');
    $('#modalTipoDeDocumentoFechar').attr('disabled', 'disabled');
    $('#modalTipoDeDocumentoCadastrar').hide();
    $('#modalTipoDeDocumentoAtualizar').hide();
    $('#modalTipoDeDocumentoCorpo').hide();
    $('#modalTipoDeDocumentoCorpoConfirmar').hide();
    $('#modalTipoDeDocumentoCorpoLoading').show();
}

function CarregarDepartamentos(UKEmpresa)
{   
    $.ajax({
        url: "/Departamento/CarregarDepartamentos",
        type: 'POST',
        dataType: "json",
        data: { UKEmpresa: UKEmpresa },
        success: function (response) {
            var myObject = eval('(' + response + ')');

            var demo2 = $('.demo2').bootstrapDualListbox({
                nonSelectedListLabel: 'Non-selected',
                selectedListLabel: 'Selected',
                preserveSelectionOnMove: 'moved',
                moveOnSelect: false,
                nonSelectedFilter: 'ion ([7-9]|[1][0-2])'
            });

            $("#duallist").empty();
            for (var i = 0; i < myObject.length; i++) {
                $("#duallist").append('<option value="' + myObject[i].UniqueKey + '">' + myObject[i].Sigla + '</option>');
            }

            $("#duallist").bootstrapDualListbox('refresh');
        }
    });   
}