$(function () {

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

    $("#btnAdmitir").on("click", function () {
        $('#modalAdmissao').modal('show');
        $('#modalAdmissaoX').hide();
        $('#modalAdmissaoFechar').addClass('disabled');
        $('#modalAdmissaoFechar').attr('disabled', 'disabled');
        $('#modalAdmissaoProsseguir').addClass('disabled');
        $('#modalAdmissaoProsseguir').attr('disabled', 'disabled');
        $('#modalAdmissaoCorpo').html('');
        $('#modalAdmissaoCorpoConfirmar').hide();
        $('#modalAdmissaoCorpoLoading').hide();


        $.ajax({
            method: "GET",
            url: "/Admissao/Cadastrar",
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