using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MySql.Data.MySqlClient;

namespace ControleDeEstoque.Web.Conexao
{
    public class getConnection
    {
        //public string connect = ";

       

        public static MySqlConnection Connection()
        {
            MySqlConnection conn = null;
                string connexao = "SERVER = localhost; DATABASE = db_controlostoque; UID = root; PWD = ";

                conn = new MySqlConnection(connexao);

                HttpContext.Current.Response.Write("Connectado com sucesso! ");
              
           
            return conn;

        }    

    }
}