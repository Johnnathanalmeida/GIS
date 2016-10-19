AplicaValidacaoCPF();

jQuery(function ($) {

    $("#txtCPF").keydown(function () {
        try {
            $("#txtCPF").unmask();
        } catch (e) { }

        $("#txtCPF").inputmask("999.999.999-99");

    });

    AplicaDatePicker();

});

function OnSuccessCadastrarEmpregado(data) {
    $('#formCadastroEmpregado').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);

    if (data.resultado.Sucesso != null && data.resultado.Sucesso != "") {
        $('#formCadastroEmpregado')[0].reset();
    }
}

function OnBeginCadastrarEmpregado() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroEmpregado").css({ opacity: "0.5" });
}