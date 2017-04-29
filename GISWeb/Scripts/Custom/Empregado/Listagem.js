jQuery(function ($) {

    AplicajQdataTable("dynamic-table", [null, null, null, null, null, null, { "bSortable": false }], false, 20);

});

function DeletarEmpregado(IDEmpregado, Nome) {

    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/Empregado/Terminar",
            data: { IDEmpregado: IDEmpregado },
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
                    $("#linha-" + IDEmpregado).remove();
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir o empregado '" + Nome + "'?", "Exclusão de empregado", callback, "btn-danger");

}
