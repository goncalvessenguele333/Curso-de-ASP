function set_dados_focus() {
    $('#txt_nome').focus();
}

function set_dados_form(dados) {
    $('#id_cadastro').val(dados.id);
    $('#txt_nome').val(dados.nome);
    $('#ddl_pais').val(dados.id_pais);
    $('#ckb_activo').prop('checked', dados.activo);

    $('#ddl_provincia').val(dados.id_provincia);
    $('#ddl_provincia').prop('disabled', dados.id_provincia <= 0 || dados.id_provincia == undefined);
}

function set_dados_grid(dados) {
    return '<td>' + dados.nome + '</td>' +
        '<td>' + (dados.activo ? 'Sim' : 'Não') + '</td>';
}

function get_dados_inclusao() {
    return {
        id: 0,
        nome: "",
        id_pais: 0,
        id_provincia: 0,
        activo: true

    };

}

function get_dados_form() {
    return {
        id: $('#id_cadastro').val(),
        nome: $('#txt_nome').val(),
        id_pais: $('#ddl_pais').val(),
        id_provincia: $('#ddl_provincia').val(),
        activo: $('#ckb_activo').prop('checked')
    };

}

function preencher_linha_grid(linha) {
    linha
        .eq(0).html(param.nome).end()
        .eq(1).html(param.activo ? 'Sim' : 'Não');
}

$(document).on('change', '#ddl_pais', function () {
    var ddl_pais = $(this),
        id_pais = parseInt(ddl_pais.val());

 
       ddl_provincia = $('#ddl_provincia');
    
    if (id_pais > 0) {
        var url = url_recuperar_provincias;
        param = { idPais: id_pais };
      

       ddl_provincia.empty();
       ddl_provincia.prop('disabled', true);

      $.post(url, add_anti_forgery_token(param), function (response) {
            if (response && response.length > 0) {
                for (var i = 0; i < response.length; i++) {
                    ddl_provincia.append('<option value=' + response[i].id + '>' + response[i].nome + '</option>');
                }
            ddl_provincia.prop('disabled', false);
            }
        });
    }
});