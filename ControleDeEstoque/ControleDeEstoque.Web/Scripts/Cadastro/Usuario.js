
function set_dados_focus() {
    $('#txt_nome').focus();
}

function set_dados_form(dados) {
    $('#id_cadastro').val(dados.id);
    $('#txt_nome').val(dados.nome);
    $('#txt_User').val(dados.login);
    $('#txt_senha').val(dados.senha);
    $('#ddl_perfil').val(dados.idPerfil);
}

function set_dados_grid(dados) {
    return '<td>' + dados.nome + '</td>' +
        '<td>' + dados.login + '</td>';
}

function get_dados_inclusao() {
    return {
        id: 0,
        nome: '',
        login: '',
        senha: '',
        idPerfil: 0 
    };

}

function get_dados_form() {
    return {
        id: $('#id_cadastro').val(),
        nome: $('#txt_nome').val(),
        login: $('#txt_User').val(),
        senha: $('#txt_senha').val(),
        idPerfil: $('#ddl_perfil').val()
    };

}

function preencher_linha_grid(linha) {
    linha
        .eq(0).html(param.nome).end()
        .eq(1).html(param.login);
}
