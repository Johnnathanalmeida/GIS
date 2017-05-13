jQuery(function ($) {
    $('.dd').nestable();
    $('.dd').nestable('collapseAll');

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

function CadastrarFuncao(pUKCargo) {
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
                                data: { UKCargo: pUKCargo, FuncaoNome: pFuncao },
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

function CadastrarAtividade(pUKFuncao) {
    var sHTML = "<table style='line-height: 2'>";

    sHTML += "<tr>";
    sHTML += "<td width='150px'>Atividade:</td>";
    sHTML += "<td width='136px' align='left'>";
    sHTML += "  <input type='text' maxlength='64' id='txtAtividadeNome' value='' style='width: 450px;'/>";
    sHTML += "</td>";
    sHTML += "</tr>";
    sHTML += "</table>";

    bootbox.dialog({
        title: "<span class='bigger-110'>Informe os dados da nova Atividade!</span>",
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
                            var pAtividade = $("#txtAtividadeNome").val();
                            $.ajax({
                                method: "POST",
                                url: "/CargoFuncAtiv/CadastrarAtividade",
                                data: { UKFuncao: pUKFuncao, AtividadeNome: pAtividade },
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

function AlterarCargo(pUKCargo, pCargo) {
    var sHTML = "<table style='line-height: 2'>";

    sHTML += "<tr>";
    sHTML += "<td width='150px'>Cargo:</td>";
    sHTML += "<td width='136px' align='left'>";
    sHTML += "  <input type='text' maxlength='64' id='txtCargoNome' value='" + pCargo + "' style='width: 450px;'/>";
    sHTML += "</td>";
    sHTML += "</tr>";
    sHTML += "</table>";

    bootbox.dialog({
        title: "<span class='bigger-110'>Informe os dados para atualizar a Cargo!</span>",
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
                                url: "/CargoFuncAtiv/AlterarCargo",
                                data: { UKCargo: pUKCargo, CargoNome: pCargo },
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

function AlterarFuncao(pUKFuncao, pFuncao) {
    var sHTML = "<table style='line-height: 2'>";

    sHTML += "<tr>";
    sHTML += "<td width='150px'>Função:</td>";
    sHTML += "<td width='136px' align='left'>";
    sHTML += "  <input type='text' maxlength='64' id='txtFuncaoNome' value='" + pFuncao + "' style='width: 450px;'/>";
    sHTML += "</td>";
    sHTML += "</tr>";
    sHTML += "</table>";

    bootbox.dialog({
        title: "<span class='bigger-110'>Informe os dados para atualizar a Função!</span>",
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
                                url: "/CargoFuncAtiv/AlterarFuncao",
                                data: { UKFuncao: pUKFuncao, FuncaoNome: pFuncao },
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

function AlterarAtividade(pUKAtividade, pAtividade) {
    var sHTML = "<table style='line-height: 2'>";

    sHTML += "<tr>";
    sHTML += "<td width='150px'>Atividade:</td>";
    sHTML += "<td width='136px' align='left'>";
    sHTML += "  <input type='text' maxlength='64' id='txtAtividadeNome' value='" + pAtividade + "' style='width: 450px;'/>";
    sHTML += "</td>";
    sHTML += "</tr>";
    sHTML += "</table>";

    bootbox.dialog({
        title: "<span class='bigger-110'>Informe os dados para atualizar a Atividade!</span>",
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
                            var pAtividade = $("#txtAtividadeNome").val();
                            $.ajax({
                                method: "POST",
                                url: "/CargoFuncAtiv/AlterarAtividade",
                                data: { UKAtividade: pUKAtividade, AtividadeNome: pAtividade },
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

function DeletarFuncao(IDFuncao, FuncaoNome) {
    bootbox.confirm({
        backdrop: true,
        message: "Tem certeza que deseja excluir a Função '" + FuncaoNome + "'?",
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
            if (result) {
                $.ajax({
                    method: "POST",
                    url: "/CargoFuncAtiv/DeletarFuncao",
                    data: { IDFuncao: IDFuncao },
                    error: function (erro) {
                        ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
                    },
                    success: function (content) {
                        TratarResultadoJSON(content.resultado);
                    }
                });
            }
        }
    });
}

function DeletarAtividade(IDAtividade, UKAtividade, AtividadeNome) {
    bootbox.confirm({
        backdrop: true,
        message: "Tem certeza que deseja excluir a Atividade '" + AtividadeNome + "'?",
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
                url: "/CargoFuncAtiv/DeletarAtividade",
                data: { IDAtividade: IDAtividade, UKAtividade: UKAtividade },
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