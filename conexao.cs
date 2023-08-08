using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projeto001
{
    internal class conexao
    {
        //string de conexão local
        public string cn = "server=localhost;database=projeto001;uid=root;pwd=;port=;";

        //variável que carrega a conexão
        public MySqlConnection con = null;

        //abrir conexão
        public void AbrirConexao()
        {
            //testar
            try
            {
                con = new MySqlConnection(cn);
                con.Open();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Erro no servidor" + ex.Message);
            }
          
        }
         
        //fechar conexão
        public void FecharConexao() 
        {
            try
            {
                con = new MySqlConnection(cn);
                con.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show("Erro no servidor" + ex.Message);
            }

        }
    }
}
