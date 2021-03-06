using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.ComponentModel.DataAnnotations;

using System.Data.SqlClient;


namespace ControleDeEstoque.Web.Models
{
    public class UsuarioModel
    {
        public int id { get; set; }

        [Required(ErrorMessage ="Informe o login")]
        public string login { get; set; }
        [Required(ErrorMessage = "Informe a senha")]
        public string senha { get; set; }
        [Required(ErrorMessage = "Informe o nome")]
        public string nome { get; set; }
        [Required(ErrorMessage = "Informe o perfil")]
        public int idPerfil { get; set; }
        public static UsuarioModel validarUsuario(string login, string senha)
        {
           UsuarioModel ret = null;

                using (var conn =new SqlConnection())
                {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["Principal"].ConnectionString;
                    conn.Open();
                    using (var cmd = new SqlCommand())

                    {
                         cmd.Connection = conn;

                    cmd.CommandText = "Select * from tb_usuarioo where usuario=@login and senha=@senha";
                    cmd.Parameters.Add("@login", SqlDbType.VarChar).Value=login;
                    cmd.Parameters.Add("@senha", SqlDbType.VarChar).Value=CriptoHelpers.HashMD5(senha);

                        var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new UsuarioModel
                        {
                            id =(int)reader["id"],
                            login=(string)reader["usuario"],
                            senha=(string)reader["senha"],
                            nome=(string)reader["nome"],
                            idPerfil = (int)reader["id_perfil"]
                        };
                    }
                       
                    }
                }               
                return ret;
      }

        public static List<UsuarioModel> RecuperarLista(int pagina, int tamPagina)
        {

            var ret = new List<UsuarioModel>();

            using (var conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["Principal"].ConnectionString;
                conn.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    var pos = (pagina - 1) * tamPagina;

                    cmd.CommandText = string.Format(
                        "Select * from tb_usuarioo order by nome offset {0} rows fetch next {1} rows only",
                        pos > 0 ? pos - 1 : 0, tamPagina);                  
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new UsuarioModel
                        {
                            id = (int)reader["id"],
                            login = (string)reader["usuario"],
                            nome = (string)reader["nome"],
                            idPerfil = (int)reader["id_perfil"]
                        });
                    }
                }
            }
            return ret;
        }

            public static int RecuperarQuantidade()
        {
            var ret = 0;

            using (var conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["Principal"].ConnectionString;
                conn.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = "Select count(*) from tb_usuarioo";

                    ret = (int)cmd.ExecuteScalar();
                }
            }
            return ret;
        }
        public static UsuarioModel RecuperarPeloId(int id)
        {
            UsuarioModel ret = null;

            using (var conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["Principal"].ConnectionString;
                conn.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = "Select * from tb_usuarioo where id=@id";
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new UsuarioModel
                        {
                            id = (int)reader["id"],
                            login=(string)reader["usuario"],
                            nome = (string)reader["nome"],
                            idPerfil = (int)reader["id_perfil"]
                        };
                    }
                }
            }
            return ret;
        }

        public static bool ExcluirPeloId(int id)
        {

            var ret = false;
            if (RecuperarPeloId(id) != null)
            {
                using (var conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["Principal"].ConnectionString;
                    conn.Open();

                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText = "delete from tb_usuarioo where id=@id";
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                        ret = (cmd.ExecuteNonQuery() > 0);
                    }



                }

            }
            return ret;

        }

        public int Salvar()
        {
            var ret = 0;
            var model = RecuperarPeloId(this.id);

            using (var conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["Principal"].ConnectionString;
                conn.Open();
                using (var cmd = new SqlCommand())
                {

                    cmd.Connection = conn;

                    if (model == null)
                    {

                        cmd.CommandText = "insert into tb_usuarioo (usuario,senha,nome,id_perfil)values (@usuario,@senha,@nome,@id_perfil); select convert(int, SCOPE_IDENTITY())";
                        cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = this.login;
                        cmd.Parameters.Add("@senha", SqlDbType.VarChar).Value = CriptoHelpers.HashMD5(this.senha);
                        cmd.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.nome;
                        cmd.Parameters.Add("@id_perfil", SqlDbType.Int).Value = this.idPerfil;

                        ret = (int)cmd.ExecuteScalar();

                    }
                    else
                    {
                        cmd.CommandText = "update tb_usuarioo set usuario=@usuario" +
                            (!string.IsNullOrEmpty(this.senha) ? ",senha=@senha" : "") + ", nome=@nome,id_perfil=@id_perfil where id=@id";
                        cmd.Parameters.Add("@usuario", SqlDbType.VarChar).Value = this.login;
                        if(!string.IsNullOrEmpty(this.senha))
                        {
                            cmd.Parameters.Add("@senha", SqlDbType.VarChar).Value = CriptoHelpers.HashMD5(this.senha);
                        }
                        cmd.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.nome;
                        cmd.Parameters.Add("@id_perfil", SqlDbType.Int).Value = this.idPerfil;
                        cmd.Parameters.Add("@id", SqlDbType.Int).Value = this.id;
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            ret = this.id;
                        }
                    }
                }
            }

            return ret;

        }

    }
}