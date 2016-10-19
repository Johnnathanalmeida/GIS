jQuery(function ($) {

    AplicajQdataTable("dynamic-table", [null, null, null, null, null, { "bSortable": false }], false, 20);

});


function BuscarDetalhesFornecedor(IDFornecedor) {

    $(".LoadingLayout").show();

    $.ajax({
        method: "POST",
        url: "/Fornecedor/BuscarFornecedorPorID",
        data: { IDFornecedor: IDFornecedor },
        error: function (erro) {
            $(".LoadingLayout").hide();
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $(".LoadingLayout").hide();

            if (content.data != null) {
                bootbox.dialog({
                    message: content.data,
                    title: "<span class='bigger-110'>Detalhes do Fornecedor</span>",
                    backdrop: true,
                    locale: "br",
                    buttons: {},
                    onEscape: true
                });
            }
            else {
                TratarResultadoJSON(content.resultado);
            }

        }
    });

}

function DeletarFornecedor(IDFornecedor, Nome) {

    var callback = function () {
        $('.LoadingLayout').show();
        $('#dynamic-table').css({ opacity: "0.5" });

        $.ajax({
            method: "POST",
            url: "/Fornecedor/Terminar",
            data: { IDFornecedor: IDFornecedor },
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
                    $("#linha-" + IDFornecedor).remove();
                }
            }
        });
    };

    ExibirMensagemDeConfirmacaoSimples("Tem certeza que deseja excluir o fornecedor '" + Nome + "'?", "Exclusão de Fornecedor", callback, "btn-danger");

}