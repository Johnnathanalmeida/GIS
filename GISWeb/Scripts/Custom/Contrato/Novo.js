jQuery(function ($) {

    AplicaDatePicker(false);

    AutoCompleteFornecedor();

    AutoCompleteDepartamento();
    
    $('#Inicio').removeAttr("data-val-date");
    $('#Fim').removeAttr("data-val-date");

    $("#NomeArquivoLocal").attr("data-target", "#modalArquivo");
    $("#NomeArquivoLocal").attr("data-toggle", "modal");
    $("#NomeArquivoLocal").attr("data-backdrop", "static");
    $("#NomeArquivoLocal").attr("data-keyboard", "false");

    $("#NomeArquivoLocal").on("click", function () {

        var btnUploadArquivo = $(this);

        $('#modalArquivoX').show();
        $('#modalArquivoFechar').removeClass('disabled');
        $('#modalArquivoFechar').removeAttr('disabled');
        $('#modalArquivoProsseguir').hide();
        $('#modalArquivoCorpo').html('');
        $('#modalArquivoCorpoLoading').show();

        $.ajax({
            method: "GET",
            url: "/Contrato/_Upload",
            error: function (erro) {
                $('#modalArquivo').modal('hide');
                ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
            },
            success: function (content) {

                $('#modalArquivoCorpoLoading').hide();
                $('#modalArquivoCorpo').html(content);

                InitDropZoneSingle();

                $.validator.unobtrusive.parse('#formUpload');

            },
        });

    });

});

function AutoCompleteFornecedor() {

    var substringMatcher = function () {
        return function findMatches(q, cb) {
            var matches, substringRegex;
            var strs;
            matches = [];
            substrRegex = new RegExp(q, 'i');

            if (q.length > 2) {
                $.ajax({
                    type: 'POST',
                    url: '/Fornecedor/LocalizarFornecedorAutoComplete',
                    data: { q: q },
                    success: function (partial) {

                        if (partial.Erro != undefined && partial.Erro != "") {
                            ExibirMensagemDeErro(partial.Erro);
                        }
                        else {
                            strs = partial.Data;
                            $.each(strs, function (i, str) {
                                if (substrRegex.test(str)) {
                                    matches.push({ value: str });
                                }
                            });
                        }

                        cb(matches);
                    },
                    async: false
                });
            }

        }
    }

    $("#IDFornecedor").typeahead({
        hint: true,
        highlight: true,
        minLength: 1,
    }, {
        name: 'states',
        displayKey: 'value',
        source: substringMatcher(),
        limit: 10
    });

}

function AutoCompleteDepartamento() {
    var tag_input = $('#Departamentos');
    try {
        tag_input.tag(
          {
              placeholder: tag_input.attr('placeholder'),
              source: function (query, process) {
                
                $.post('/Departamento/LocalizarDepartamentoAutoComplete?q=' + encodeURIComponent(query), function (partial) {

                    var arr = [];

                    if (partial.Erro != undefined && partial.Erro != "") {
                        ExibirMensagemDeErro(partial.Erro);
                    }
                    else {
                        for (var x = 0; x < partial.Data.length; x++) {
                            arr.push(partial.Data[x]);
                        }
                    }

                    process(arr);
                });
                  
              }

          }
        );

        $(".tags").css("width", "100%");

    }
    catch (e) {
        alert(e);
        //display a textarea for old IE, because it doesn't support this plugin or another one I tried!
        tag_input.after('<textarea id="' + tag_input.attr('id') + '" name="' + tag_input.attr('name') + '" rows="3">' + tag_input.val() + '</textarea>').remove();
        //$('#form-field-tags').autosize({append: "\n"});
    }
}

function OnSuccessCadastrarContrato(data) {
    $("#formCadastroContrato").css({ opacity: "1" });
    $(".LoadingLayout").hide();
    $('#btnSalvar').show();
    TratarResultadoJSON(data.resultado);
}

function OnBeginCadastrarContrato() {

    var bValid = false;

    if ($.trim($("#Departamentos").val()) == "") {
        bValid = true;
        $("#Departamentos").parent().next().html("Selecione um departamento");
    }
    else {
        $("#Departamentos").parent().next().html("");
    }

    if ($.trim($("#IDFornecedor").val()) == "") {
        bValid = true;
        $("#IDFornecedor").parent().next().html("Selecione um fornecedor");
    }
    else {
        $("#IDFornecedor").parent().next().html("");
    }

    if (bValid) {
        return false;
    }
    
    $(".LoadingLayout").show();
    $('#btnSalvar').hide();
    $("#formCadastroContrato").css({ opacity: "0.5" });
}

function InitDropZoneSingle() {
    try {
        Dropzone.autoDiscover = false;

        var dictDefaultMessage = "";
        dictDefaultMessage += '<span class="bigger-150 bolder">';
        dictDefaultMessage += '  <i class="ace-icon fa fa-caret-right red"></i> Arraste o arquivo</span> para upload \
				                <span class="smaller-80 grey">(ou clique)</span> <br /> \
				                <i class="upload-icon ace-icon fa fa-cloud-upload blue fa-3x"></i>';

        var previewTemplate = "";
        previewTemplate += "<div class=\"dz-preview dz-file-preview\">\n ";
        previewTemplate += "    <div class=\"dz-details\">\n ";
        previewTemplate += "        <div class=\"dz-filename\">";
        previewTemplate += "            <span data-dz-name></span>";
        previewTemplate += "        </div>\n    ";
        previewTemplate += "        <div class=\"dz-size\" data-dz-size></div>\n  ";
        previewTemplate += "        <img data-dz-thumbnail />\n  ";
        previewTemplate += "    </div>\n  ";
        previewTemplate += "    <div class=\"progress progress-small progress-striped active\">";
        previewTemplate += "        <div class=\"progress-bar progress-bar-success\" data-dz-uploadprogress></div>";
        previewTemplate += "    </div>\n  ";
        previewTemplate += "    <div class=\"dz-success-mark\">";
        previewTemplate += "        <span></span>";
        previewTemplate += "    </div>\n  ";
        previewTemplate += "    <div class=\"dz-error-mark\">";
        previewTemplate += "        <span></span>";
        previewTemplate += "    </div>\n  ";
        previewTemplate += "    <div class=\"dz-error-message\">";
        previewTemplate += "        <span data-dz-errormessage></span>";
        previewTemplate += "    </div>\n";
        previewTemplate += "</div>";

        //#######################################################################################################
        //Recupera do form montado os respectivos valores retornados do servidor e armazenados na web como 'data'
        //var extensoes = $('#formUpload').data('extensoes');
        //if (extensoes == '') extensoes = null;

        //var uploadMultiplo = $('#formUpload').data('uploadmultiplo');
        //var maxArquivos = 1;
        //if (uploadMultiplo && uploadMultiplo.toUpperCase() == 'TRUE') maxArquivos = 20;
        //#######################################################################################################

        var myDropzone = new Dropzone("#formUpload", {
            paramName: "file",
            uploadMultiple: false, //se habilitar upload múltiplo, pode bugar o SPF
            parallelUploads: 1, //se for mais que 1, pode bugar o SPF
            maxFilesize: 20, // MB
            dictFileTooBig: 'Tamanho máximo permitido ultrapassado.',
            //maxFiles: maxArquivos,
            dictMaxFilesExceeded: 'Limite máximo de número de arquivos permitidos ultrapassado.',
            acceptedFiles: ".pdf,.doc,.docx",
            dictInvalidFileType: 'Extensão de arquivo inválida para este tipo de anexo.',
            addRemoveLinks: true,
            dictCancelUpload: 'Cancelar',
            dictCancelUploadConfirmation: 'Tem certeza que deseja cancelar?',
            dictRemoveFile: 'Remover',
            dictDefaultMessage: dictDefaultMessage,
            dictResponseError: 'Erro ao fazer o upload do arquivo.',
            dictFallbackMessage: 'Este browser não suporta a funcionalidade de arrastar e soltar arquivos para fazer upload.',
            previewTemplate: previewTemplate,
        });

        myDropzone.on('sending', function (file) {
            if (!$('#formUpload').valid()) {
                myDropzone.removeFile(file);
            } else {
                $('#modalArquivoX').hide();
                $('#modalArquivoFechar').addClass('disabled');
                $('#modalArquivoFechar').attr('disabled', 'disabled');
            }
        });

        myDropzone.on('canceled', function () {
            if (myDropzone.getUploadingFiles().length === 0 && myDropzone.getQueuedFiles().length === 0) {
                $('#modalArquivoX').show();
                $('#modalArquivoFechar').removeClass('disabled');
                $('#modalArquivoFechar').removeAttr('disabled', 'disabled');
            }
        });

        myDropzone.on('success', function (file, content) {
            if (content.sucesso) {
                ExibirMensagemGritter('Sucesso!', content.sucesso, 'gritter-success');

                $("#NomeArquivoLocal").val(content.arquivo);

                if (myDropzone.getUploadingFiles().length === 0 && myDropzone.getQueuedFiles().length === 0 && myDropzone.getRejectedFiles().length === 0) {
                    $('#modalArquivo').modal('hide');
                }
            } else if (content.alerta) {
                ExibirMensagemGritter('Alerta!', content.alerta, 'gritter-warning');

                if (myDropzone.getUploadingFiles().length === 0 && myDropzone.getQueuedFiles().length === 0 && myDropzone.getRejectedFiles().length === 0) {
                    $('#modalArquivo').modal('hide');
                }
            }
            else {
                $('#modalArquivoX').show();
                $('#modalArquivoFechar').removeClass('disabled');
                $('#modalArquivoFechar').removeAttr('disabled', 'disabled');

                if (content.erro)
                    ExibirMensagemGritter('Oops! Erro inesperado', content.erro, 'gritter-error');
                else
                    ExibirMensagemGritter('Oops! Erro inesperado', 'Ocorreu algum problema não identificado ao fazer o upload do arquivo para o servidor.', 'gritter-error');
            }
        });

        myDropzone.on('error', function () {
            $('#modalArquivoX').show();
            $('#modalArquivoFechar').removeClass('disabled');
            $('#modalArquivoFechar').removeAttr('disabled', 'disabled');
        });

        myDropzone.on('removedfile', function (file) {
            if (myDropzone.getUploadingFiles().length === 0 && myDropzone.getQueuedFiles().length === 0) {
                $('#modalArquivoX').show();
                $('#modalArquivoFechar').removeClass('disabled');
                $('#modalArquivoFechar').removeAttr('disabled', 'disabled');
            }
        });

        myDropzone.on('maxfilesexceeded', function () {
            ExibirMensagemGritter('Alerta', 'Só é permitida a inclusão de 1 arquivo para cada tipo de anexo.', 'gritter-warning');
        });

        $(document).one('ajaxloadstart.page', function (e) {
            try {
                myDropzone.destroy();
            } catch (e) { }
        });

    } catch (e) {
        ExibirMensagemGritter('Alerta', 'Este browser não é compatível com o componente Dropzone.js. Sugerimos a utilização do Google Chrome ou Internet Explorer 10 (ou versão superior).', 'gritter-warning');
    }
}

function FormNovaGarantia() {

    $('#modalGarantiaX').show();
    $('#modalGarantiaFechar').removeClass('disabled');
    $('#modalGarantiaFechar').removeAttr('disabled');
    $('#modalGarantiaProsseguir').hide();
    $('#modalGarantiaCorpo').html('');
    $('#modalGarantiaCorpoLoading').show();

    $.ajax({
        method: "POST",
        url: "/Garantia/Nova",
        error: function (erro) {
            $('#modalGarantia').modal('hide');
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {

            $('#modalGarantiaCorpoLoading').hide();
            $('#modalGarantiaCorpo').html(content);

            $('#modalGarantiaProsseguir').show();

            $("#modalGarantiaProsseguir").off("click");
            $("#modalGarantiaProsseguir").click(function () {
                
                var validate = true;
                if ($.trim($("#txtDescricao").val()) == "") {
                    validate = false;
                    $("#txtDescricao").next().html($("#txtDescricao").data("val-required"));
                }
                else {
                    $("#txtDescricao").next().html("");
                }

                if ($.trim($("#txtPrazo").val()) == "") {
                    validate = false;
                    $("#txtPrazo").next().html($("#txtPrazo").data("val-required"));
                }
                else {
                    $("#txtPrazo").next().html("");
                }

                if ($.trim($("#ddlIntervalo").val()) == "") {
                    validate = false;
                    $("#ddlIntervalo").next().html($("#ddlIntervalo").data("val-required"));
                }
                else {
                    $("#ddlIntervalo").next().html("");
                }

                if (!validate) {
                    ExibirMensagemDeAlerta("Informe os campos obritatórios.");
                }
                else {
                    InserirLinhaGarantia($.trim($("#txtDescricao").val()), $.trim($("#txtPrazo").val()), $.trim($("#ddlIntervalo").val()));
                    $('#modalGarantia').modal('hide');
                }

            });

        },
    });

}

function InserirLinhaGarantia(Descricao, Prazo, Intervalo) {
    
    var sHTML = "<tr><td>" + Descricao + "</td><td class=\"center\">" + Prazo + "</td><td class=\"center\">" + Intervalo + "</td><td class=\"center\"><a href=\"#\" class=\"red CustomTooltip\" title=\"Excluir\" onclick=\"ExcluirLinhaGarantia(this); return false;\"><i class=\"ace-icon fa fa-remove bigger-120\"/></a></td></tr>";

    if ($("#TableGarantias").length == 0) {
        $("#alertaGarantia").hide();

        sHTML = "<table id=\"TableGarantias\" class=\"table table-striped table-bordered table-hover\"><tr><th>Descrição</th><th width=\"80px\" class=\"center\">Prazo</th><th width=\"80px\" class=\"center\">Intervalo</th><th width=\"30px\"></th></tr>" + sHTML + "</table>";
        $("#conteudoGarantia").html(sHTML);

        AplicaTooltip();
    }
    else {
        $('#TableGarantias tr:last').after(sHTML);

        AplicaTooltip();
    }
}

function ExcluirLinhaGarantia(obj) {
    $(obj).parent().parent().remove();

    if ($("#TableGarantias tr").length == 1) {
        $("#conteudoGarantia").html("");
        $("#alertaGarantia").show();
    }

}

function FormNovoEstabelecimento() {

    $('#modalEstabelecimentoX').show();
    $('#modalEstabelecimentoFechar').removeClass('disabled');
    $('#modalEstabelecimentoFechar').removeAttr('disabled');
    $('#modalEstabelecimentoProsseguir').hide();
    $('#modalEstabelecimentoCorpo').html('');
    $('#modalEstabelecimentoCorpoLoading').show();

    $.ajax({
        method: "POST",
        url: "/Estabelecimento/Novo",
        error: function (erro) {
            $('#modalEstabelecimento').modal('hide');
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {

            $('#modalEstabelecimentoCorpoLoading').hide();
            $('#modalEstabelecimentoCorpo').html(content);

            $('#modalEstabelecimentoProsseguir').show();

            $("#modalEstabelecimentoProsseguir").off("click");
            $("#modalEstabelecimentoProsseguir").click(function () {

                var validate = true;
                if ($.trim($("#txtEstabNome").val()) == "") {
                    validate = false;
                    $("#txtEstabNome").next().html($("#txtEstabNome").data("val-required"));
                }
                else {
                    $("#txtEstabNome").next().html("");
                }

                if (!validate) {
                    ExibirMensagemDeAlerta("Informe os campos obritatórios.");
                }
                else {
                    InserirLinhaEstabelecimento($.trim($("#txtEstabNome").val()), $.trim($("#txtEstabDescricao").val()), $.trim($("#txtEstabEndereco").val()), $.trim($("#txtEstabArquivo").val()));
                    $('#modalEstabelecimento').modal('hide');
                }

            });

            AplicaDatePicker(false);

        },
    });

}

function InserirLinhaEstabelecimento(Nome, Descricao, Endereco, Arquivo) {

    var sHTML = "<tr><td>" + Nome + "</td><td>" + Descricao + "</td><td>" + Endereco + "</td><td>" + Arquivo + "</td><td class=\"center\"><a href=\"#\" class=\"red CustomTooltip\" title=\"Excluir\" onclick=\"ExcluirLinhaEstabelecimento(this); return false;\"><i class=\"ace-icon fa fa-remove bigger-120\"/></a></td></tr>";

    if ($("#TableEstabelecimentos").length == 0) {
        $("#alertaEstabelecimento").hide();

        sHTML = "<table id=\"TableEstabelecimentos\" class=\"table table-striped table-bordered table-hover\"><tr><th width=\"200px\">Nome</th><th>Descrição</th><th width=\"200px\">Endereço</th><th width=\"120px\">Arquivo</th><th width=\"30px\"></th></tr>" + sHTML + "</table>";
        $("#conteudoEstabelecimento").html(sHTML);

        AplicaTooltip();
    }
    else {
        $('#TableEstabelecimentos tr:last').after(sHTML);

        AplicaTooltip();
    }
}

function ExcluirLinhaEstabelecimento(obj) {
    $(obj).parent().parent().remove();

    if ($("#TableEstabelecimentos tr").length == 1) {
        $("#conteudoEstabelecimento").html("");
        $("#alertaEstabelecimento").show();
    }

}
