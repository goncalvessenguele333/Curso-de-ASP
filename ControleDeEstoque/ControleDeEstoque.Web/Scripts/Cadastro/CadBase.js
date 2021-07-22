function add_anti_forgery_token(data) {
    data.__RequestVerificationToken = $('[name=__RequestVerificationToken]').val();
    return data;
}

function formatar_mensagem_aviso(mensagens) {
    var ret = "";
    for (var i = 0; i < mensagens.length; i++) {
        ret += '<li>' + mensagens[i] + '</li>';
    }

    return '<ul>' + ret + '</ul>';
}

function abrirForm(dados) {
    set_dados_form(dados);
    var modal_cadastro = $('#modal_cadastro');

    $('#msg_mensagam_aviso').empty();
    $('#msg_aviso').hide();
    $('#msg_mensagam_aviso').hide();
    $('#msg_erro').hide();


    bootbox.dialog({
        title:'Cadastro de '+ titulo_pagina,
        message: modal_cadastro
    })
        .on('shown.bs.modal', function () {
            modal_cadastro.show(0, function () {
                set_dados_focus();
            });
        })
        .on('hidden.bs.modal', function () {
            modal_cadastro.hide().appendTo('body');
        });
}

function criar_linha_grid(dados) {
    var ret =
        '<tr data-id=' + dados.id + '>' +
        set_dados_grid(dados) +    
        '<td>' +
        '<a class="btn btn-primary btn_alterar" role="button" style="margin-rigth:30px"><i class="glyphicon glyphicon-pencil"></i>Alterar</a>' +
        '<a class="btn btn-danger btn_excluir" role="button"><i class="glyphicon glyphicon-trash"></i>Excluir</a>' +
        '</td>' +
        '</tr>';

    return ret;
}

$(document).on('click', '.btn_alterar', function () {

    var btn = $(this),
        id = btn.closest('tr').attr('data-id'),
        url = url_recuparar_produto,
        param = { 'id': id };
    $.post(url, add_anti_forgery_token(param), function (response) {
        if (response) {
            /*response.senha = senha_p;*/
           abrirForm(response);
        }
    })
})
    .on('click', '#btn_incluir', function () {

        abrirForm(get_dados_inclusao());

    })
    .on('click', '.btn_excluir', function () {

        var btn = $(this),
            tr = btn.closest('tr'),
            id = tr.attr('data-id'),
            url = url_exclusao_produto,
            param = { 'id': id };

        bootbox.confirm({
            message: "Realmente deseja excluir o " + titulo_pagina + "?",
            buttons: {
                confirm: {
                    label: 'sim',
                    className: 'btn-danger'
                },
                cancel: {
                    label: 'Não',
                    className: 'btn-success'
                }
            },
            callback: function (result) {
                if (result) {


                    $.post(url, add_anti_forgery_token(param), function (response) {
                        if (response) {
                            tr.remove();
                        }
                    })
                }
            }
        })

    })
    .on('click', '.page-item', function () {
        var btn = $(this),
            tamPag = $('#del_tam_pag').val(),
            pagina = btn.text(),
            url = url_grupo_produto_pagina,
            param = { 'pagina': pagina, 'tamPag': tamPag };

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response) {
                var table = $('#grid_cadastro').find('tbody');
                table.empty();
                for (var i = 0; i < response.length; i++) {
                    table.append(criar_linha_grid(response[i]));
                }
                btn.siblings().removeClass('active');
                btn.addClass('active');

            }
        })


    })
    .on('click', '#btn_confirmar', function () {

        var btn = $(this),
            url = url_salvar_grupo_produto;
        param = get_dados_form();

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response.Resultado == "Ok") {
                if (param.id == 0) {
                    param.id = response.IdSalvo;
                    var table = $('#grid_cadastro').find('tbody'),
                        linha = criar_linha_grid(param);
                    table.append(linha);
                }
                else {
                    var linha = $('#grid_cadastro').find('tr[data-id=' + param.id + ']').find('td');
                    preencher_linha_grid(linha);

                }
                $('#modal_cadastro').parents('.bootbox').modal('hide');

            }
            else if (response.Resultado == "ERRO") {
                $('#msg_aviso').hide();
                $('#msg_mensagam_aviso').hide();
                $('#msg_erro').show();
            }
            else if (response.Resultado == "AVISO") {
                $('#msg_mensagam_aviso').html(formatar_mensagem_aviso(response.Mensagens));
                $('#msg_aviso').show();
                $('#msg_mensagam_aviso').show();
                $('#msg_erro').hide();
            }
        })
    })
    .on('change', '#del_tam_pag', function () {
        var ddl = $(this),
            tamPag = ddl.val(),
            pagina = 1,
            url = url_grupo_produto_p,
            param = { 'pagina': pagina, 'tamPag': tamPag };

        $.post(url, add_anti_forgery_token(param), function (response) {
            if (response) {
                var table = $('#grid_cadastro').find('tbody');
                table.empty();
                for (var i = 0; i < response.length; i++) {
                    table.append(criar_linha_grid(response[i]));
                }
                ddl.siblings().removeClass('active');
                ddl.addClass('active');

            }
        })

    });