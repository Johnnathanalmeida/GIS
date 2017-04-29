AplicaValidacaoCPF();

jQuery(function ($) {
    $('#txtCPF').mask('999.999.999-99');
    $("#txtTelefone").mask("(99) 9999 - 9999?9", { placeholder: " " });
    AplicaDatePicker();

});

function OnSuccessAtualizarEmpregado(data) {
    $('#formEdicaoEmpregado').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginAtualizarEmpregado() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formEdicaoEmpregado").css({ opacity: "0.5" });
}