AplicaValidacaoCPF();

jQuery(function ($) {
    $('#txtCPF').mask('999.999.999-99');
    $("#txtTelefone").mask("(99) 9999 - 9999?9", { placeholder: " " });
    AplicaDatePicker(false);

});

function OnSuccessCadastrarEmpregado(data) {
    TratarResultadoJSON(data.resultado);

    $("#btnLimpar").click();
    $(".LoadingLayout").hide();
    $("#formCadastroEmpregado").css({ opacity: "1" });
}

function OnBeginCadastrarEmpregado() {
    $(".LoadingLayout").show();
    $("#formCadastroEmpregado").css({ opacity: "0.5" });
}