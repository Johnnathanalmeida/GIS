jQuery(function ($) {

});

function OnSuccessAtualizarEstabelecimento(data) {
    $('#formEdicaoEstabelecimento').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginAtualizarEstabelecimento() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formEdicaoEstabelecimento").css({ opacity: "0.5" });
}