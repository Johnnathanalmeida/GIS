
jQuery(function ($) {

    $("#ddlEmpresa").change(function () {
        
        if ($(this).val() != "") {

            $('#ddFornecedor').empty();
            $('#ddFornecedor').append($('<option></option>').val("").html("Aguarde ..."));
            $("#ddFornecedor").attr("disabled", true);

            $('#ddlDepartamento').empty();
            $('#ddlDepartamento').append($('<option></option>').val("").html("Aguarde ..."));
            $("#ddlDepartamento").attr("disabled", true);

            $.ajax({
                method: "POST",
                url: "/Contrato/CarregarFornecedoresEDepartamentosPorEmpresa",
                data: { IDEmpresa: $(this).val() },
                error: function (erro) {
                    ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
                },
                success: function (content) {
                    if (content.fornecedores.length > 0) {
                        $("#ddFornecedor").attr("disabled", false);
                        $('#ddFornecedor').empty();
                        $('#ddFornecedor').append($('<option></option>').val("").html("Selecione um fornecedor"));
                        for (var i = 0; i < content.fornecedores.length; i++) {
                            $('#ddFornecedor').append(
                                $('<option></option>').val(content.fornecedores[i].Fornecedor.IDFornecedor).html(content.fornecedores[i].Fornecedor.Nome)
                            );
                        }
                    }
                    else {
                        $('#ddFornecedor').empty();
                        $('#ddFornecedor').append($('<option></option>').val("").html("Nenhum fornecedor encontrado para esta empresa"));
                    }

                    //##############################################################################################################################

                    if (content.departamentos.length > 0) {
                        $("#ddlDepartamento").attr("disabled", false);
                        $('#ddlDepartamento').empty();
                        $('#ddlDepartamento').append($('<option></option>').val("").html("Selecione um departamento"));
                        for (var i = 0; i < content.departamentos.length; i++) {
                            $('#ddlDepartamento').append(
                                $('<option></option>').val(content.departamentos[i].IDDepartamento).html(content.departamentos[i].Sigla)
                            );
                        }
                    }
                    else {
                        $('#ddlDepartamento').empty();
                        $('#ddlDepartamento').append($('<option></option>').val("").html("Nenhum departamento encontrado para esta empresa"));
                    }
                }
            });
        }
        else {
            $('#ddFornecedor').empty();
            $('#ddFornecedor').append($('<option></option>').val("").html("Selecione antes uma Empresa..."));
            $("#ddFornecedor").attr("disabled", true);

            $('#ddlDepartamento').empty();
            $('#ddlDepartamento').append($('<option></option>').val("").html("Selecione antes uma Empresa..."));
            $("#ddlDepartamento").attr("disabled", true);
        }

    });


});


function OnSuccessCadastrarContrato(data) {
    $('#formCadastroEmpresa').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarContrato() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroContrato").css({ opacity: "0.5" });
}