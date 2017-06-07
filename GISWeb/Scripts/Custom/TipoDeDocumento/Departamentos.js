jQuery(function ($) {
    alert();
});

function OnSuccessGerenciarDpto(data) {
    alert();
    $('#modalTipoDeDocumento').hide();
    TratarResultadoJSON(data.resultado);
}

function OnBeginGerenciarDpto() {
    alert('ww');
    $('#modalTipoDeDocumento').modal('show');
    $('#modalTipoDeDocumentoX').hide();
    $('#modalTipoDeDocumentoFechar').addClass('disabled');
    $('#modalTipoDeDocumentoFechar').attr('disabled', 'disabled');
    $('#modalTipoDeDocumentoCadastrar').hide();
    $('#modalTipoDeDocumentoAtualizar').hide();
    $('#modalTipoDeDocumentoCorpo').hide();
    $('#modalTipoDeDocumentoCorpoConfirmar').hide();
    $('#modalTipoDeDocumentoCorpoLoading').show();
}