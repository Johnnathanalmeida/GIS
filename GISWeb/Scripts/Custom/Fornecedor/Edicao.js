
function OnSuccessAtualizarEmpresa(data) {
    $('#formEdicaoFornecedor').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginAtualizarEmpresa() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formEdicaoFornecedor").css({ opacity: "0.5" });
}