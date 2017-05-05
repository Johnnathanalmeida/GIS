﻿$(function () {

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
        alert();
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
                $('#modalArquivo').modal('hide');
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $('#modalAdmissaoCorpo').html(content);

            },
        });


        
    });
});

function OnSuccessCadastrarAdmissao(data) {
    alert('1');
}

function OnBeginCadastrarAdmissao() {
    alert('2');
}