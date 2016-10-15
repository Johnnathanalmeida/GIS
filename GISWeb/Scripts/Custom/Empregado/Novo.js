AplicaValidacaoCPF();

jQuery(function ($) {

    $("#txtCPF").keydown(function () {
        try {
            $("#txtCPF").unmask();
        } catch (e) { }

        $("#txtCPF").inputmask("999.999.999-99");

    });

});

function OnSuccessCadastrarEmpregado(data) {
    $('#formCadastroEmpregado').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarEmpregado() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroEmpregado").css({ opacity: "0.5" });
}