
function OnSuccessCadastrarContrato(data) {
    $('#formCadastroEmpresa').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarContrato() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroContrato").css({ opacity: "0.5" });
}