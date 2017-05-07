jQuery(function ($) {
    $('.dd').nestable();

    $('.dd-handle a').on('mousedown', function (e) {
        e.stopPropagation();
    });

    $('[data-rel="tooltip"]').tooltip();

});


function btnNovoCargo() {
    var sHTML = "<table style='line-height: 2'>";

    sHTML += "<tr>";
    sHTML += "<td width='150px'>Cargo:</td>";
    sHTML += "<td width='136px' align='left'>";
    sHTML += "  <input type='text' maxlength='64' id='txtCargoNome' value='' style='width: 450px;'/>";
    sHTML += "</td>";
    sHTML += "</tr>";
    sHTML += "</table>";

    bootbox.dialog({
        title: "<span class='bigger-110'>Informe os dados do novo Cargo!</span>",
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
                            var pCargo = $("#txtCargoNome").val();
                            $.ajax({
                                method: "POST",
                                url: "/CargoFuncAtiv/CadastrarCargo",
                                data: { Cargo: pCargo },
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

function DeletarCargo(IDCargo, CargoNome) {
    bootbox.confirm({
        backdrop: true,
        message: "Tem certeza que deseja excluir o cargo '" + CargoNome + "'?",
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
                url: "/CargoFuncAtiv/DeletarCargo",
                data: { IDCargo: IDCargo },
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

function CadastrarFuncao(pIDCargo) {
    var sHTML = "<table style='line-height: 2'>";

    sHTML += "<tr>";
    sHTML += "<td width='150px'>Função:</td>";
    sHTML += "<td width='136px' align='left'>";
    sHTML += "  <input type='text' maxlength='64' id='txtFuncaoNome' value='' style='width: 450px;'/>";
    sHTML += "</td>";
    sHTML += "</tr>";
    sHTML += "</table>";

    bootbox.dialog({
        title: "<span class='bigger-110'>Informe os dados da nova Função!</span>",
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
                            var pFuncao = $("#txtFuncaoNome").val();
                            $.ajax({
                                method: "POST",
                                url: "/CargoFuncAtiv/CadastrarFuncao",
                                data: { IDCargo: pIDCargo, FuncaoNome: pFuncao },
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