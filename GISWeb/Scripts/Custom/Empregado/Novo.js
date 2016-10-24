AplicaValidacaoCPF();

jQuery(function ($) {

    $('#txtCPF').mask('999.999.999-99');
    //$('#txtDataNascimento').mask('99/99/9999');
    $("#txtTelefone").mask("(99) 9999 - 9999?9", { placeholder: " " });
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