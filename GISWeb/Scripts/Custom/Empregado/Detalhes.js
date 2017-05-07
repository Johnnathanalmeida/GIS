$(function () {

    AplicaDatePicker(false);

    $("#cbxEmpresa").on('change', function () {
        $.ajax({
            url: "/Admissao/CarregarDepartamentos",
            type: 'POST',
            dataType: "json",
            data: { IDEmpresa: $("#cbxEmpresa").val() },
            success: function (response) {
                var myObject = eval('(' + response + ')');

                $("#cbxDepartamento").empty();
                $("#cbxDepartamento").append('<option value=""> Selecione o departamento... </option>');

                for (var i = 0; i < myObject.length; i++) {
                    $("#cbxDepartamento").append('<option value="' + myObject[i].Value + '">' + myObject[i].Text + '</option>');
                }

                $("#cbxDepartamento").trigger("chosen:updated");
            }
        });
    });

    $("#modalAdmissaoProsseguir").on("click", function () {
        $("#formCadastroAdmissao").submit();
    });

    $("#btnAdmitir").on("click", function () {
        $('#modalAdmissao').modal('show');
        $('#modalAdmissaoX').hide();
        $('#modalAdmissaoFechar').removeClass('disabled');
        $('#modalAdmissaoFechar').removeAttr('disabled', 'disabled');
        $('#modalAdmissaoProsseguir').removeClass('disabled');
        $('#modalAdmissaoProsseguir').removeAttr('disabled', 'disabled');
        $('#modalAdmissaoCorpo').html('');
        $('#modalAdmissaoCorpoConfirmar').hide();
        $('#modalAdmissaoCorpoLoading').hide();

        $.ajax({
            method: "GET",
            url: "/Admissao/Novo",
            data: { IDEmpregado: $('#IDEmpregado').text().trim() },
            error: function (erro) {
                $('#modalAdmissao').modal('hide');
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $('#modalAdmissaoCorpo').html(content);
            },
        });
    });

    $("#btnDemitir").on("click", function () {
        bootbox.dialog({
            title: "<span class='bigger-110'>Atenção!</span>",
            message: "<span>Tem certeza que deseja demitir o empregado?</span> ",
            buttons:
                    {
                        "success":
                        {
                            "label": "Não",
                            "className": "btn-sm btn-danger btnReprovar",
                            "callback": function () {
                            }
                        },
                        "danger":
                        {
                            "label": "Sim",
                            "className": "btn-sm btn-success btnAprovar",
                            "callback": function () {
                                $.ajax({
                                    method: "POST",
                                    url: "/Admissao/Demitir",
                                    data: { IDEmpregado: $('#IDEmpregado').text().trim() },
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
    });
});

function OnSuccessCadastrarAdmissao(data) {    
    $('#modalAdmissao').hide();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarAdmissao() {
    $('#modalAdmissao').modal('show');
    $('#modalAdmissaoX').hide();
    $('#modalAdmissaoFechar').addClass('disabled');
    $('#modalAdmissaoFechar').attr('disabled', 'disabled');
    $('#modalAdmissaoProsseguir').addClass('disabled');
    $('#modalAdmissaoProsseguir').attr('disabled', 'disabled');
    $('#modalAdmissaoCorpo').hide();
    $('#modalAdmissaoCorpoConfirmar').hide();
    $('#modalAdmissaoCorpoLoading').show();
}