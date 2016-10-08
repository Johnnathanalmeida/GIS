jQuery(function ($) {
    
    $("#ddlEmpresa").change(function () {

        if ($(this).val() != "") {

            $('#ddlDepartamento').empty();
            $('#ddlDepartamento').append($('<option></option>').val("").html("Aguarde ..."));
            $("#ddlDepartamento").attr("disabled", true);
            $(".LoadingLayout").show();

            $.ajax({
                method: "POST",
                url: "/Departamento/ListarDepartamentosPorEmpresa",
                data: { idEmpresa: $(this).val() },
                error: function (erro) {
                    $(".LoadingLayout").hide();
                    ExibirMensagemGritter('Oops! Erro inesperado', erro.responseText, 'gritter-error')
                },
                success: function (content) {
                    $(".LoadingLayout").hide();
                    if (content.resultado.length > 0) {
                        $("#ddlDepartamento").attr("disabled", false);
                        $('#ddlDepartamento').empty();
                        $('#ddlDepartamento').append($('<option></option>').val("").html("Selecione um departamento"));
                        for (var i = 0; i < content.resultado.length; i++) {
                            $('#ddlDepartamento').append(
                                $('<option></option>').val(content.resultado[i].IDDepartamento).html(content.resultado[i].Sigla)
                            );
                        }
                    }
                    else {
                        $('#ddlDepartamento').empty();
                        $('#ddlDepartamento').append($('<option></option>').val("").html("Nenhum departamento encontrado para esta empresa"));
                    }
                }
            });
        }
        else {
            $('#ddlDepartamento').empty();
            $('#ddlDepartamento').append($('<option></option>').val("").html("Selecione antes uma Empresa..."));
            $("#ddlDepartamento").attr("disabled", true);
        }
    });

});

function ListarUsuarios() {
    
    var valEmpresa = $("#ddlEmpresa").val();
    var valOrgao = $("#ddlDepartamento").val();

    if (valEmpresa == "" && valOrgao == "") {
        ExibirMensagemDeAlerta("Selecione uma Empresa ou um Departamento!");
    }
    else if (valOrgao != "") {
        $(".LoadingLayout").show();
        $.post('/Permissoes/BuscarUsuariosPorDepartamento', { id: valOrgao }, function (partial) {
            $(".LoadingLayout").hide();
            TratarResultadoListarUsuarios(partial);
        });
    }
    else if (valEmpresa != "") {
        $(".LoadingLayout").show();
        $.post('/Permissoes/BuscarUsuariosPorEmpresa', { id: valEmpresa }, function (partial) {
            $(".LoadingLayout").hide();
            TratarResultadoListarUsuarios(partial);
        });
    }
    
}

function TratarResultadoListarUsuarios(partial) {
    if (partial.resultado != undefined) {
        TratarResultadoJSON(partial.resultado);
    }
    else {
        $(".conteudoAjax").html(partial.data);

        if (parseInt(partial.usuarios) == 0) {

        }
        else {

            var orderByTable = "[ ";
            var count = parseInt(partial.colunas);
            for (var i = 0; i < count; i++) {
                orderByTable += "null, ";
            }

            if (orderByTable != null) {
                orderByTable = orderByTable.substring(0, orderByTable.length - 2) + "]";
            }

            var oTable1 = AplicajQdataTable("dynamic-table1", eval(orderByTable), false, 25);

            TableTools.classes.container = "btn-group btn-overlap";

            //initiate TableTools extension
            var tableTools_obj = new $.fn.dataTable.TableTools(oTable1, {
                "sRowSelector": "td:not(:last-child)",
                "sRowSelect": "multi",
                "fnRowSelected": function (row) {
                    //check checkbox when row is selected
                    try { $(row).find('input[type=checkbox]').get(idxCol).checked = true }
                    catch (e) { }
                },
                "fnRowDeselected": function (row) {
                    //uncheck checkbox
                    try { $(row).find('input[type=checkbox]').get(idxCol).checked = false }
                    catch (e) { }
                },

                "sSelectedClass": "success"
            });

            $('#dynamic-table1 > thead > tr > th input[type=checkbox]').on('click', function () {

                idxCol = $(this).attr("rel");

                var th_checked = this.checked;//checkbox inside "TH" table header
                $(this).closest('table').find('tbody > tr').each(function () {
                    var row = this;
                    if (th_checked) tableTools_obj.fnSelect(row);
                    else tableTools_obj.fnDeselect(row);
                });

                SalvarPermissoes(th_checked, $(this).attr("id"), "");
            });

            //$('#dynamic-table1').on('click', 'td input[type=checkbox]', function () {
                //var row = $(this).closest('tr').get(0);
                //if (!this.checked) tableTools_obj.fnSelect(row);
                //else tableTools_obj.fnDeselect($(this).closest('tr').get(0));
            //});

        }

    }
}

function SalvarPermissoes(Acao, Perfil, UIDsUsuarios) {

    var valEmpresa = $("#ddlEmpresa").val();
    var valOrgao = $("#ddlDepartamento").val();

    if (valEmpresa == "" && valOrgao == "") {
        ShowMsgAlerta("Selecione uma Empresa ou um Órgão!");
    }
    else {

        if (UIDsUsuarios == "") {
            $('#dynamic-table1 > tbody > tr').each(function () {
                UIDsUsuarios += $(this).find("input").get(0).id;
            });
        }

        $.post('/Permissoes/SalvarPermissoes', { Acao: Acao, Perfil: Perfil, UIDsUsuarios: UIDsUsuarios, Orgao: valOrgao, Empresa: valEmpresa }, function (partial) {
            
            if (partial.resultado != undefined && partial.resultado != "") {
                TratarResultadoJSON(partial.resultado);
            }
            else {

            }

        });

    }

}