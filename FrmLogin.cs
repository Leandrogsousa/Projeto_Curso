using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projeto001
{
    public partial class FrmLogin : Form
    {
        conexao con = new conexao();
        string sql;
        MySqlCommand cmd;

        public FrmLogin()
        {
            InitializeComponent();
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            //login
            if (txtNomeLogin.Text.ToString().Trim() == "" || txtSenhaLogin.Text.ToString().Trim() =="")
            {
                MessageBox.Show(" Digite os dados!");
                txtNomeLogin.Text = "";
                txtNomeLogin.Focus();
                return;
            }
            try
            {
                con.AbrirConexao();
                MySqlCommand cmdverificar;
                MySqlDataReader reader;
                cmdverificar = new MySqlCommand("SELECT * FROM login WHERE nome=@nome AND senha=@senha",con.con);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmdverificar;
                cmdverificar.Parameters.AddWithValue("@nome", txtNomeLogin.Text);
                cmdverificar.Parameters.AddWithValue("@senha", txtSenhaLogin.Text);
                reader = cmdverificar.ExecuteReader();
                if (reader.HasRows)
                {
                    FrmMenu frm = new FrmMenu();
                    frm.ShowDialog();

                    
                }
                else
                {
                    MessageBox.Show(" Login Inválido.");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
