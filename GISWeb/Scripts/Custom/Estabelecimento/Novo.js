jQuery(function ($) {

});

function OnSuccessCadastrarEstabelecimento(data) {
    $('#formCadastroEstabelecimento').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarEstabelecimento() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroEstabelecimento").css({ opacity: "0.5" });
}