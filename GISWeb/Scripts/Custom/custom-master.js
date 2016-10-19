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

    }

});

