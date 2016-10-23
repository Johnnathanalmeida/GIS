jQuery(function ($) {
    Chosen();
});

function OnSuccessPesquisarEmpregado(data) {
    $('#formPesquisaEmpregado').removeAttr('style');
    $(".LoadingLayout").hide();

    if (data.Resultado != undefined) {
        TratarResultadoJSON(data.resultado);
    }
    else {
        $(".conteudoAjax").html(data.data);

        AplicajQdataTable("dynamic-table", [null, null, null, null], false, 20);

        AplicaTooltip();
    }
}

function OnBeginPesquisarEmpregado() {
    $(".LoadingLayout").show();
    $("#formPesquisaEmpregado").css({ opacity: "0.5" });
}