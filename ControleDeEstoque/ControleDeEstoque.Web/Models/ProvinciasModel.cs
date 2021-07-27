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
    public class ProvinciasModel
    {
        public int id { get; set; }
        [Required(ErrorMessage = "Informe o nome")]
        public string nome { get; set; }
        public bool activo { get; set; }
        [Required(ErrorMessage = "Informe o país")]
        public int id_pais { get; set; }


        public static List<ProvinciasModel> RecuperarLista(int pagina, int tamPagina, string filtro = "", int id_pais = 0)
        {

            var ret = new List<ProvinciasModel>();

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

                    if (id_pais > 0)
                    {
                        filtroWhere += (string.IsNullOrEmpty(filtroWhere) ? " where" : " and") + 
                            string.Format(" id_pais= {0}", id_pais);
                    }

                    cmd.Connection = conn;

                    cmd.CommandText = string.Format(
                        "Select * from tb_provincia " +
                         filtroWhere +
                        "order by nome offset {0} rows fetch next {1} rows only",
                        pos > 0 ? pos - 1 : 0, tamPagina);

                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new ProvinciasModel
                        {
                            id = (int)reader["id"],
                            nome = (string)reader["nome"],
                            activo = (bool)reader["activo"],
                            id_pais = (int)reader["id_pais"]
                        });


                    }
                }
            }
            return ret;
        }

        public static List<ProvinciasModel> RecuperarListaPais(int id_pais)
        {

            var ret = new List<ProvinciasModel>();

            using (var conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["Principal"].ConnectionString;
                conn.Open();
                using (var cmd = new SqlCommand())
                {
                 
                    cmd.Connection = conn;

                    cmd.CommandText = string.Format("Select * from tb_provincia where id_pais={0}",id_pais);

                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new ProvinciasModel
                        {
                            id = (int)reader["id"],
                            nome = (string)reader["nome"],
                            activo = (bool)reader["activo"],
                            id_pais = (int)reader["id_pais"]
                        });


                    }
                }
            }
            return ret;
        }


        public static List<ProvinciasModel> RecuperarProvinciasActivos()
        {

            var ret = new List<ProvinciasModel>();

            using (var conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["Principal"].ConnectionString;
                conn.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = string.Format("Select * from tb_provincia where activo=1 order by nome");
                    var reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new ProvinciasModel
                        {
                            id = (int)reader["id"],
                            nome = (string)reader["nome"],
                            activo = (bool)reader["activo"],
                            id_pais = (int)reader["id_pais"]

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

                    cmd.CommandText = "Select count(*) from tb_provincia";

                    ret = (int)cmd.ExecuteScalar();
                }
            }
            return ret;
        }



        public static ProvinciasModel RecuperarPeloId(int id)
        {
            ProvinciasModel ret = null;

            using (var conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["Principal"].ConnectionString;
                conn.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = "Select * from tb_provincia where id=@id";
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new ProvinciasModel
                        {
                            id = (int)reader["id"],
                            nome = (string)reader["nome"],
                            activo = (bool)reader["activo"],
                            id_pais=(int)reader["id_pais"]
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
                        cmd.CommandText = "delete from tb_provincia where id=@id";
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

                        cmd.CommandText = "insert into tb_provincia (nome, activo,id_pais)values (@nome,@activo,@id_pais); select convert(int, SCOPE_IDENTITY())";
                        cmd.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.nome;
                        cmd.Parameters.Add("@activo", SqlDbType.VarChar).Value = (this.activo ? 1 : 0);
                        cmd.Parameters.Add("@id_pais", SqlDbType.Int).Value = this.id_pais;

                        ret = (int)cmd.ExecuteScalar();

                    }
                    else
                    {
                        cmd.CommandText = "update tb_provincia set nome=@nome, activo=@activo,id_pais=@id_pais where id=@id";
                        cmd.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.nome;
                        cmd.Parameters.Add("@activo", SqlDbType.VarChar).Value = (this.activo ? 1 : 0);
                        cmd.Parameters.Add("@id_pais", SqlDbType.Int).Value = this.id_pais;
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