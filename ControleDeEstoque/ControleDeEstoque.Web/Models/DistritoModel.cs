using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ControleDeEstoque.Web.Models
{
    public class DistritoModel
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Informe o nome")]
        public string nome { get; set; }
        public bool activo { get; set; }
        [Required(ErrorMessage = "Informe o província")]
        public int id_provincia{ get; set; }
        [Required(ErrorMessage = "Informe o país")]
        public int id_pais { get; set; }


        public static List<DistritoModel> RecuperarLista(int pagina, int tamPagina, string filtro = "")
        {

            var ret = new List<DistritoModel>();

            using (var conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["Principal"].ConnectionString;
                conn.Open();
                using (var cmd = new SqlCommand())
                {

                    var pos = (pagina - 1) * tamPagina;

                    var filtroWhere = "";
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        filtroWhere = string.Format(" where lower(nome) like '%{0}%'", filtro.ToLower());
                    }

                    cmd.Connection = conn;

                    cmd.CommandText = string.Format(
                        "Select * from tb_distrito " +
                         filtroWhere +
                        "order by nome offset {0} rows fetch next {1} rows only",
                        pos > 0 ? pos - 1 : 0, tamPagina);

                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new  DistritoModel
                        {
                            id = (int)reader["id"],
                            nome = (string)reader["nome"],
                            activo = (bool)reader["activo"],
                            id_pais = (int)reader["id_pais"],
                            id_provincia = (int)reader["id_provincia"]
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

                    cmd.CommandText = "Select count(*) from tb_distrito";

                    ret = (int)cmd.ExecuteScalar();
                }
            }
            return ret;
        }



        public static  DistritoModel RecuperarPeloId(int id)
        {
             DistritoModel ret = null;

            using (var conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["Principal"].ConnectionString;
                conn.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = "Select * from tb_distrito where id=@id";
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new  DistritoModel
                        {
                            id = (int)reader["id"],
                            nome = (string)reader["nome"],
                            activo = (bool)reader["activo"],
                            id_pais = (int)reader["id_pais"],
                            id_provincia = (int)reader["id_provincia"]
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
                        cmd.CommandText = "delete from tb_distrito where id=@id";
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

                        cmd.CommandText = "insert into tb_distrito (nome, activo,id_pais,id_provincia)values (@nome,@activo,@id_pais,@id_provincia); select convert(int, SCOPE_IDENTITY())";
                        cmd.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.nome;
                        cmd.Parameters.Add("@activo", SqlDbType.VarChar).Value = (this.activo ? 1 : 0);
                        cmd.Parameters.Add("@id_pais", SqlDbType.Int).Value = this.id_pais;
                        cmd.Parameters.Add("@id_provincia", SqlDbType.Int).Value = this.id_provincia;

                        ret = (int)cmd.ExecuteScalar();

                    }
                    else
                    {
                        cmd.CommandText = "update tb_distrito set nome=@nome, activo=@activo,id_pais=@id_pais,id_provincia=@id_provincia where id=@id";
                        cmd.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.nome;
                        cmd.Parameters.Add("@activo", SqlDbType.VarChar).Value = (this.activo ? 1 : 0);
                        cmd.Parameters.Add("@id_pais", SqlDbType.Int).Value = this.id_pais;
                        cmd.Parameters.Add("@id_provincia", SqlDbType.Int).Value = this.id_provincia;
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