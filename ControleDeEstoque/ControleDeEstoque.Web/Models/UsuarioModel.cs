using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using ControleDeEstoque.Web.Conexao;
using MySql.Data.MySqlClient;


namespace ControleDeEstoque.Web.Models
{
    public class UsuarioModel
    {
       
        public static bool validarUsuario(string login, string senha)
        {
            try
            {
                var ret = false;


                using (var conn = Conexao.getConnection.Connection())
                {
                    conn.Open();
                    using (var cmd = new MySqlCommand())

                    {
                         cmd.Connection = conn;

                        cmd.CommandText = string.Format("Select * from tb_usuario where usuario='{0}' and senha='{1}'",login, senha);
                        ret = ((int)cmd.ExecuteScalar() > 0);
                       
                    }
                }               
                return ret;
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}