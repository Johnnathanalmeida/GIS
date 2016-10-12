jQuery(function ($) {

    var menuAtivo = $('#ulMenuLateral').data('menuativo');

    if (menuAtivo && menuAtivo != '') {


        var menusASeremAtivados = menuAtivo.split('/');
        var sNameMenuAnterior = "";
        for (var i = 0; i < menusASeremAtivados.length; i++) {
            
            sNameMenuAnterior += menusASeremAtivados[i];

            if (i == menusASeremAtivados.length - 1) {
                $('#ml' + sNameMenuAnterior).addClass('active');
            }
            else {
                $('#ml' + sNameMenuAnterior).addClass('active open');
            }

        }

        //if (menusASeremAtivados.length == 1) {
        //    $('#ml' + menusASeremAtivados[0]).addClass('active');
        //} else if (menusASeremAtivados.length == 2) {
        //    $('#ml' + menusASeremAtivados[0]).addClass('active open');
        //    $('#ml' + menusASeremAtivados[0] + menusASeremAtivados[1]).addClass('active');
        //} else if (menusASeremAtivados.length == 3) {
        //    $('#ml' + menusASeremAtivados[0]).addClass('active open');
        //    $('#ml' + menusASeremAtivados[0] + menusASeremAtivados[1]).addClass('active open');
        //    $('#ml' + menusASeremAtivados[0] + menusASeremAtivados[1] + menusASeremAtivados[2]).addClass('active');
        //}
    }

});

