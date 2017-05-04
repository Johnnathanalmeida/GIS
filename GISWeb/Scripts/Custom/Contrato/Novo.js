jQuery(function ($) {

    AplicaDatePicker(false);

    AutoCompleteFornecedor();

    AutoCompleteDepartamento();
    
    $('#Inicio').removeAttr("data-val-date");
    $('#Fim').removeAttr("data-val-date");

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
