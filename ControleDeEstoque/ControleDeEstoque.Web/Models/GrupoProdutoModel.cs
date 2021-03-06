using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
namespace ControleDeEstoque.Web.Models
{
    public class GrupoProdutoModel
    {        
        public int id { get; set; }
        [Required(ErrorMessage="Informe o nome")]
        public string nome { get; set; }
        public bool activo { get; set; }


        public static List<GrupoProdutoModel> RecuperarLista(int pagina, int tamPagina, string filtro = "")
        {
                
            var ret = new  List<GrupoProdutoModel>();

            using (var conn = new  SqlConnection())
                {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["Principal"].ConnectionString;
                conn.Open();
                using ( var cmd = new SqlCommand())
                {
                   
                    var pos = (pagina - 1) * tamPagina;

                    var filtroWhere = "";
                    if (!string.IsNullOrEmpty(filtro))
                    {
                        filtroWhere = string.Format(" where lower(nome) like '%{0}%'", filtro.ToLower());
                    }

                    cmd.Connection = conn;

                    cmd.CommandText = string.Format(
                        "Select * from tb_grupo_produto "+
                         filtroWhere +
                        "order by nome offset {0} rows fetch next {1} rows only",
                        pos > 0 ? pos - 1 : 0, tamPagina);

                   var  reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        ret.Add(new GrupoProdutoModel
                        {
                            id = (int)reader["id"],
                            nome =(string)reader["nome"],
                            activo = (bool)reader["activo"]
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

                    cmd.CommandText = "Select count(*) from tb_grupo_produto";
                       
                   ret = (int)cmd.ExecuteScalar();                  
                }
            }
            return ret;
        }



        public static GrupoProdutoModel RecuperarPeloId(int id)
        {
            GrupoProdutoModel ret =null;

            using (var conn = new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["Principal"].ConnectionString;
                conn.Open();
                using (var cmd = new SqlCommand())
                {
                    cmd.Connection = conn;

                    cmd.CommandText = "Select * from tb_grupo_produto where id=@id";
                    cmd.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    var reader = cmd.ExecuteReader();
                    if (reader.Read())
                    {
                        ret = new GrupoProdutoModel
                        {
                            id = (int)reader["id"],
                            nome = (string)reader["nome"],
                            activo = (bool)reader["activo"]
                        };


                    }
                }


            }
            return ret;
        }

        public static bool ExcluirPeloId(int id)
        {
         
            var ret = false;
            if(RecuperarPeloId(id) != null)
            {
                using (var conn = new SqlConnection())
                {
                    conn.ConnectionString = ConfigurationManager.ConnectionStrings["Principal"].ConnectionString;
                    conn.Open();

                    using (var cmd = new SqlCommand())
                    {
                        cmd.Connection = conn;
                        cmd.CommandText ="delete from tb_grupo_produto where id=@id";
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

            using (var conn =new SqlConnection())
            {
                conn.ConnectionString = ConfigurationManager.ConnectionStrings["Principal"].ConnectionString;
                conn.Open();
                using (var cmd = new SqlCommand())
                {

                    cmd.Connection = conn;

                    if (model == null)
                    {

                        cmd.CommandText = "insert into tb_grupo_produto (nome, activo)values (@nome,@activo); select convert(int, SCOPE_IDENTITY())";
                        cmd.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.nome;
                        cmd.Parameters.Add("@activo", SqlDbType.VarChar).Value = (this.activo ? 1 : 0);
                               
                        ret = (int)cmd.ExecuteScalar();

                    }
                    else
                    {
                        cmd.CommandText = "update tb_grupo_produto set nome=@nome, activo=@activo where id=@id";
                        cmd.Parameters.Add("@nome", SqlDbType.VarChar).Value = this.nome;
                        cmd.Parameters.Add("@activo", SqlDbType.VarChar).Value = (this.activo ? 1 : 0);
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