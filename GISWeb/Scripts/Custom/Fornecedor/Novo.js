﻿jQuery(function ($) {

    $('#txtCNPJ').mask('99.999.999/9999-99');

});

function OnSuccessCadastrarFornecedor(data) {
    $('#formCadastroFornecedor').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarFornecedor() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroFornecedor").css({ opacity: "0.5" });
}