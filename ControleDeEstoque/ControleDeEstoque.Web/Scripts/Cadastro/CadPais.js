function set_dados_focus() {
    $('#txt_nome').focus();
}

function set_dados_form(dados) {
    $('#id_cadastro').val(dados.id);
    $('#txt_nome').val(dados.nome);
    $('#txt_codigo_p').val(dados.codigo);
    $('#ckb_activo').prop('checked', dados.activo);
}

function set_dados_grid(dados) {
    return '<td>' + dados.nome + '</td>' +
        '<td>' + dados.codigo + '</td>' +
        '<td>' + (dados.activo ? 'Sim' : 'Não') + '</td>';
}

function get_dados_inclusao() {
    return {
        id: 0,
        nome: "",
        codigo: "",
        activo: true
    };

}

function get_dados_form() {
    return {
        id: $('#id_cadastro').val(),
        nome: $('#txt_nome').val(),
        codigo: $('#txt_codigo_p').val(),
        activo: $('#ckb_activo').prop('checked')
    };

}

function preencher_linha_grid(linha) {
    linha
        .eq(0).html(param.nome).end()
        .eq(1).html(param.codigo).end()
        .eq(2).html(param.activo ? 'Sim' : 'Não');
}

