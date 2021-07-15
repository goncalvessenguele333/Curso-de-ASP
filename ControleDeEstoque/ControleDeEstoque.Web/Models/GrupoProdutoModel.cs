using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using MySql.Data.MySqlClient;

namespace ControleDeEstoque.Web.Models
{
    public class GrupoProdutoModel
    {        public int id { get; set; }
        [Required(ErrorMessage="Informe o nome")]
        public string nome { get; set; }
        public Boolean activo { get; set; }


        public static List<GrupoProdutoModel> RecuperarLista()
        {
            MySqlDataReader reader = null;
            GrupoProdutoModel l;
          
            List<GrupoProdutoModel> ret = new  List<GrupoProdutoModel>();


                using (var conn = Conexao.getConnection.Connection())
                {
                    conn.Open();
                MySqlCommand cmd = new MySqlCommand();

                    
                        //cmd.Connection = conn;

                       string sqlselect = "Select * from tb_grupo_produto order by nome";
                    cmd = new MySqlCommand(sqlselect, conn);
                    reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                        l = new GrupoProdutoModel();

                    l.id = (int.Parse(reader["id"].ToString()));
                    l.nome = reader["nome"].ToString();
                    l.activo =Boolean.Parse(reader["activo"].ToString());

                    ret.Add(l);
                           
                        }
                    
                }
                return ret;
          }

        public static GrupoProdutoModel RecuperarPeloId(int id)
        {
            MySqlDataReader reader = null;
            GrupoProdutoModel l;
            
           GrupoProdutoModel ret = null;

            using (var conn = Conexao.getConnection.Connection())
            {
                conn.Open();
                MySqlCommand cmd = new MySqlCommand();


                //cmd.Connection = conn;

                string sqlselect = string.Format("Select * from tb_grupo_produto where id={0}",id);
                cmd = new MySqlCommand(sqlselect, conn);
                reader = cmd.ExecuteReader();
                if(reader.Read())
                {
                    l = new GrupoProdutoModel();

                    l.id = (int.Parse(reader["id"].ToString()));
                    l.nome = reader["nome"].ToString();
                    l.activo = Boolean.Parse(reader["activo"].ToString());

                    ret=l;

                }

            }
            return ret;           

        }

        public static bool ExcluirPeloId(int id)
        {
         
            var ret = false;
            if(RecuperarPeloId(id) != null)
            {
                using (var conn = Conexao.getConnection.Connection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();


                    //cmd.Connection = conn;

                    string sqlselect = string.Format("delete from tb_grupo_produto where id={0}", id);
                    cmd = new MySqlCommand(sqlselect, conn);
                    ret = (cmd.ExecuteNonQuery() > 0);

                }              

            }
            return ret;

        }

        public int Salvar()
        {
            var ret = 0;
            var model = RecuperarPeloId(this.id);
           
                using (var conn = Conexao.getConnection.Connection())
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand();

                if (model == null)
                {

                    string sqlselect = string.Format("insert into tb_grupo_produto (nome, activo)values ('{0}',{1}); LAST_INSERT_ID()", this.nome, this.activo ? 0 : 1 );
                    cmd = new MySqlCommand(sqlselect, conn);
                    ret = (int)cmd.ExecuteScalar();

                }
                else {
                    string sqlselect = string.Format("update tb_grupo_produto set nome='{1}', activo={2} where id={0}", this.id, this.nome, this.activo ? 0 : 1);
                    cmd = new MySqlCommand(sqlselect, conn);
                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        ret = this.id;
                    }
                }

            }
            return ret;

        }

    }
}