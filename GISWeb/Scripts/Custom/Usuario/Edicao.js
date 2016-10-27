
function OnSuccessAtualizarUsuario(data) {
    $('#formEdicaoUsuario').removeAttr('style');
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginAtualizarUsuario() {
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formEdicaoUsuario").css({ opacity: "0.5" });
}