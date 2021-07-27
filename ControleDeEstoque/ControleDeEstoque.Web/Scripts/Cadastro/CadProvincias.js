function set_dados_focus() {
    $('#txt_nome').focus();
}

function set_dados_form(dados) {
    $('#id_cadastro').val(dados.id);
    $('#txt_nome').val(dados.nome);
    $('#ddl_pais').val(dados.id_pais);
    $('#ckb_activo').prop('checked', dados.activo);
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
        activo: true  

    };

}

function get_dados_form() {
    return {
        id: $('#id_cadastro').val(),
        nome: $('#txt_nome').val(),
        id_pais: $('#ddl_pais').val(),
        activo: $('#ckb_activo').prop('checked')
    };

}

function preencher_linha_grid(linha) {
    linha
        .eq(0).html(param.nome).end()
        .eq(1).html(param.activo ? 'Sim' : 'Não');
}