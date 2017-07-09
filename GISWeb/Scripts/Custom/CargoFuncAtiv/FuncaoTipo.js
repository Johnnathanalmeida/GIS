function GerenciarTipos(pUKFuncao) {
    $('#modalFuncaoTipo').modal('show');
    $('#modalFuncaoTipoX').hide();
    $('#modalFuncaoTipoFechar').removeClass('disabled');
    $('#modalFuncaoTipoFechar').removeAttr('disabled', 'disabled');
    $('#modalFuncaoTipoSalvar').removeClass('disabled');
    $('#modalFuncaoTipoSalvar').removeAttr('disabled', 'disabled');
    $('#modalFuncaoTipoCorpo').html('');
    $('#modalFuncaoTipoCorpoConfirmar').hide();
    $('#modalFuncaoTipoCorpoLoading').hide();

    $.ajax({
        method: "GET",
        url: "/CargoFuncAtiv/GerenciarTipos",
        data: { UKFuncao: pUKFuncao },
        error: function (erro) {
            alert(erro.responseText);
            $('#modalFuncaoTipo').modal('hide');
            ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
        },
        success: function (content) {
            debugger;
            $('#modalFuncaoTipoCorpo').html(content);
        },
    });
}

function OnSuccessGerenciarDpto(data) {
    $('#modalFuncaoTipo').hide();
    TratarResultadoJSON(data.resultado);
}

function OnBeginGerenciarDpto() {
    $('#modalFuncaoTipo').modal('show');
    $('#modalFuncaoTipoX').hide();
    $('#modalFuncaoTipoFechar').addClass('disabled');
    $('#modalFuncaoTipoFechar').attr('disabled', 'disabled');
    $('#modalFuncaoTipoSalvar').hide();
    $('#modalFuncaoTipoCorpo').hide();
    $('#modalFuncaoTipoCorpoConfirmar').hide();
    $('#modalFuncaoTipoCorpoLoading').show();
}

function CarregarDepartamentos(UKEmpresa) {
    //$.ajax({
    //    url: "/Departamento/CarregarDepartamentos",
    //    type: 'POST',
    //    dataType: "json",
    //    data: { UKEmpresa: UKEmpresa },
    //    success: function (response) {
    //        var myObject = eval('(' + response + ')');

    //        var demo2 = $('.demo2').bootstrapDualListbox({
    //            nonSelectedListLabel: 'Non-selected',
    //            selectedListLabel: 'Selected',
    //            preserveSelectionOnMove: 'moved',
    //            moveOnSelect: false,
    //            nonSelectedFilter: 'ion ([7-9]|[1][0-2])'
    //        });

    //        $("#duallist").empty();
    //        for (var i = 0; i < myObject.length; i++) {
    //            $("#duallist").append('<option value="' + myObject[i].UniqueKey + '">' + myObject[i].Sigla + '</option>');
    //        }

    //        $("#duallist").bootstrapDualListbox('refresh');
    //    }
    //});
}
