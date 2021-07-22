function set_dados_focus() {
    $('#txt_nome').focus();
}

function set_dados_form(dados) {
    $('#id_cadastro').val(dados.id);
    $('#txt_nome').val(dados.nome);
    $('#txt_sigla').val(dados.sigla);
    $('#ckb_activo').prop('checked', dados.activo);
}

function set_dados_grid(dados) {
    return '<td>' + dados.nome + '</td>' +
           '<td>' + dados.sigla + '</td>' +
        '<td>' + (dados.activo ? 'Sim' : 'Não') + '</td>';
}

function get_dados_inclusao() {
    return {
        id: 0,
        nome: "",
        sigla: "",
        activo: true
    };

}

function get_dados_form() {
    return {
        id: $('#id_cadastro').val(),
        nome: $('#txt_nome').val(),
        sigla: $('#txt_sigla').val(),
        activo: $('#ckb_activo').prop('checked')
    };

}

function preencher_linha_grid(linha) {
    linha
        .eq(0).html(param.nome).end()
        .eq(1).html(param.sigla).end()
        .eq(2).html(param.activo ? 'Sim' : 'Não');
}



