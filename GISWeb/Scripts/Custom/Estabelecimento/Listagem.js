jQuery(function ($) {

    AplicajQdataTable("dynamic-table", [null, null, null, { "bSortable": false }], false, 20);

});

function DeletarEstabelecimento(IDEstabelecimento, Nome) {

    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/Estabelecimento/Terminar",
            data: { IDEstabelecimento: IDEstabelecimento },
            error: function (erro) {
                $(".LoadingLayout").hide();
                $("#dynamic-table").css({ opacity: '' });
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {
                $('.LoadingLayout').hide();
                $("#dynamic-table").css({ opacity: '' });

                TratarResultadoJSON(content.resultado);

                if (content.resultado.Sucesso != null && content.resultado.Sucesso != "") {
                    $("#linha-" + IDEstabelecimento).remove();
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir o estabelecimento '" + Nome + "'?", "Exclusão de Estabelecimento", callback, "btn-danger");

}