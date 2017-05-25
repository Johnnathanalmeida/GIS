jQuery(function ($) {
    $('.dd').nestable();
    $('.dd').nestable('collapseAll');

    $('.dd-handle a').on('mousedown', function (e) {
        e.stopPropagation();
    });

    $('[data-rel="tooltip"]').tooltip();

    

});

function btnNovaCategoria() {
    var sHTML = "<table style='line-height: 2'>";

    sHTML += "<tr>";
    sHTML += "<td width='150px'>Categoria:</td>";
    sHTML += "<td width='136px' align='left'>";
    sHTML += "  <input type='text' maxlength='64' id='txtCategoriaNome' value='' style='width: 450px;'/>";
    sHTML += "</td>";
    sHTML += "</tr>";
    sHTML += "</table>";

    bootbox.dialog({
        title: "<span class='bigger-110'>Informe os dados da nova Categoria!</span>",
        message: sHTML,
        buttons:
                {
                    "success":
                    {
                        "label": "Cancelar",
                        "className": "btn-sm btn-danger btnReprovar",
                        "callback": function () {
                        }
                    },
                    "danger":
                    {
                        "label": "Salvar",
                        "className": "btn-sm btn-success btnAprovar",
                        "callback": function () {
                            var pCategoria = $("#txtCategoriaNome").val();
                            $.ajax({
                                method: "POST",
                                url: "/CategoriaDeDocumento/CadastrarCategoria",
                                data: { Categoria: pCategoria },
                                error: function (erro) {
                                    ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
                                },
                                success: function (content) {
                                    TratarResultadoJSON(content.resultado);
                                }
                            });
                        }
                    }
                }
    });
}

function CadastrarTipo(pUKCategoria) {
    $('#modalTipoDeDocumento').modal('show');
    $('#modalTipoDeDocumentoX').hide();
    $('#modalTipoDeDocumentoFechar').removeClass('disabled');
    $('#modalTipoDeDocumentoFechar').removeAttr('disabled', 'disabled');
    $('#modalTipoDeDocumentoProsseguir').removeClass('disabled');
    $('#modalTipoDeDocumentoProsseguir').removeAttr('disabled', 'disabled');
    $('#modalTipoDeDocumentoCorpo').html('');
    $('#modalTipoDeDocumentoCorpoConfirmar').hide();
    $('#modalTipoDeDocumentoCorpoLoading').hide();

    $.ajax({
        method: "GET",
        url: "/TipoDeDocumento/Novo",
        data: { UKCategoria: pUKCategoria },
        error: function (erro) {
            $('#modalTipoDeDocumento').modal('hide');
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            $('#modalTipoDeDocumentoCorpo').html(content);
        },
    });
};