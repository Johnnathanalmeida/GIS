jQuery(function ($) {

    Chosen();

});

function OnSuccessPesquisarEmpregado(data) {
    $('#formPesquisaEmpregado').removeAttr('style');
    $(".LoadingLayout").hide();

    TratarResultadoJSON(data.resultado);



}

function OnBeginPesquisarEmpregado() {


    $(".LoadingLayout").show();
    $("#formPesquisaEmpregado").css({ opacity: "0.5" });
}