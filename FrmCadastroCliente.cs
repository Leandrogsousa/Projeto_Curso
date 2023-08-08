using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Projeto001
{
    public partial class FrmCadastroCliente : Form
    {
        conexao conexao = new conexao();
        string sql;
        MySqlCommand cmd;

        //variável que pega o ID do registro
        string Id;
        //variavel que recebe a imagem
        string foto;
        //variavel para alterar foto
        string alterouFoto = "nao";
        //variavel para verificar cpf antigo
        string cpfAntigo;
        public FrmCadastroCliente()
        {
            InitializeComponent();
        }
        private void FormatarGD()
        {
            grid.Columns[0].HeaderText = "Código";
            grid.Columns[1].HeaderText = "Nome";
            grid.Columns[2].HeaderText = "Endereco";
            grid.Columns[3].HeaderText = "CPF";
            grid.Columns[4].HeaderText = "Telefone";
            grid.Columns[5].HeaderText = "Foto";

            grid.Columns[5].Visible = false;
        }
        private void FrmPrincipal_Load(object sender, EventArgs e)
        {
            LimparFoto();
            ListarGrid();
        }
        private void ListarGrid()
        {
            conexao.AbrirConexao();
            sql = "SELECT * FROM cliente ORDER BY nome ASC";
            cmd = new MySqlCommand(sql, conexao.con);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            da.SelectCommand = cmd;
            DataTable dt = new DataTable(); 
            da.Fill(dt);
            grid.DataSource = dt;
            conexao.FecharConexao();

            FormatarGD();
        }
        private void btnNovo_Click(object sender, EventArgs e)
        {
           txtNome.Focus();
            HabilitarCampos();
            HabilitarBotoes();
            LimparFoto();
            btnNovo.Enabled = false;
            btnAlterar.Enabled = false;
            btnExcluir.Enabled = false;
            
        }
        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (txtNome.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Campo 'Nome' é obrigatório");
                LimparCampos();
                txtNome.Focus();
                return;
            }
            if (txtCPF.Text == "   .   .   -" || txtCPF.Text.Length <14)
            {
                MessageBox.Show("Digite o CPF");
                txtCPF.Focus();
                return;
            }
            conexao.AbrirConexao();
            sql = "INSERT INTO cliente (nome, endereco,CPF,fone,imagem) VALUES (@nome,@endereco,@CPF,@fone,@imagem) ";
            cmd = new MySqlCommand(sql,conexao.con);

            cmd.Parameters.AddWithValue("@nome",txtNome.Text);
            cmd.Parameters.AddWithValue("@endereco", txtEndereco.Text);
            cmd.Parameters.AddWithValue("@CPF", txtCPF.Text);   
            cmd.Parameters.AddWithValue("@fone",txtFone.Text);
            cmd.Parameters.AddWithValue("@imagem",IMG());//método  IMG

            //verificar se CPF já existe
            MySqlCommand cmdVerificar;
            cmdVerificar = new MySqlCommand("SELECT * FROM cliente WHERE CPF=@CPF", conexao.con);
            MySqlDataAdapter da = new MySqlDataAdapter();
            da.SelectCommand = cmdVerificar;
            cmdVerificar.Parameters.AddWithValue("@CPF", txtCPF.Text);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                MessageBox.Show("O CPF já está cadastrado!", "CPF Existente", MessageBoxButtons.OK,MessageBoxIcon.Stop);
                txtCPF.Text = "";
                txtCPF.Focus();
                return;
            }


            cmd.ExecuteNonQuery();
            conexao.FecharConexao();

            
            LimparCampos();
            DesabilatarBotoes();
            DesabilitarCampos();
            btnNovo.Enabled = true;

            //Atualiza a grid
            LimparFoto();
            ListarGrid();
            MessageBox.Show("Registro salvo com sucesso!","Salvar", MessageBoxButtons.OK,MessageBoxIcon.Information);

        }
        private void btnAlterar_Click(object sender, EventArgs e)
        {
            if (txtNome.Text.ToString().Trim() == "")
            {
                MessageBox.Show("Campo 'Nome' é obrigatório");
                LimparCampos();
                txtNome.Focus();
                return;
            }
            if (txtCPF.Text == "   .   .   -" || txtCPF.Text.Length < 14)
            {
                MessageBox.Show("Digite o CPF");
                txtCPF.Focus();
                return;
            }

            conexao.AbrirConexao();
            if (alterouFoto == "sim")
            {
                sql = "UPDATE cliente SET nome=@nome, endereco=@endereco, CPF=@CPF, fone=@fone, imagem=@imagem WHERE id=@id";
                cmd = new MySqlCommand(sql, conexao.con);
                cmd.Parameters.AddWithValue("id", Id);
                cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                cmd.Parameters.AddWithValue("@endereco", txtEndereco.Text);
                cmd.Parameters.AddWithValue("@CPF", txtCPF.Text);
                cmd.Parameters.AddWithValue("@fone", txtFone.Text);
                cmd.Parameters.AddWithValue("imagem", IMG());
            }
            else if (alterouFoto == "nao")
            {
                sql = "UPDATE cliente SET nome=@nome, endereco=@endereco, CPF=@CPF, fone=@fone WHERE id=@id";
                cmd = new MySqlCommand(sql, conexao.con);
                cmd.Parameters.AddWithValue("id", Id);
                cmd.Parameters.AddWithValue("@nome", txtNome.Text);
                cmd.Parameters.AddWithValue("@endereco", txtEndereco.Text);
                cmd.Parameters.AddWithValue("@CPF", txtCPF.Text);
                cmd.Parameters.AddWithValue("@fone", txtFone.Text);
            }
            if (txtCPF.Text != cpfAntigo)
            {
                MySqlCommand cmdVerificar;
                cmdVerificar = new MySqlCommand("SELECT * FROM cliente WHERE CPF=@CPF", conexao.con);
                MySqlDataAdapter da = new MySqlDataAdapter();
                da.SelectCommand = cmdVerificar;
                cmdVerificar.Parameters.AddWithValue("@CPF", txtCPF.Text);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    MessageBox.Show("O CPF já está cadastrado!", "CPF Existente", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                    txtCPF.Text = "";
                    txtCPF.Focus();
                    return;
                }
            }

            cmd.ExecuteNonQuery();
            conexao.FecharConexao();

            LimparCampos();
            DesabilatarBotoes();
            DesabilitarCampos();
            btnNovo.Enabled = true;

            ListarGrid();
            LimparFoto();

            MessageBox.Show("Registro alterado com sucesso!","Alterar",MessageBoxButtons.OK,MessageBoxIcon.Information);
            
        }
        private void btnExcluir_Click(object sender, EventArgs e)
        {
            //Fazer pergunta
            var  resposta = MessageBox.Show("Deseja realmente excluir esse registro?", "Deletar",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (resposta == DialogResult.Yes)
            {
                conexao.AbrirConexao();
                sql = "DELETE FROM cliente WHERE id=@id";
                cmd = new MySqlCommand(sql, conexao.con);
                cmd.Parameters.AddWithValue("id", Id);
                //
                conexao.FecharConexao();


                LimparCampos();
                DesabilitarCampos();
                DesabilatarBotoes();
                btnNovo.Enabled = true;
                ListarGrid();
                MessageBox.Show("Registro excluído com sucesso!","Deletar",MessageBoxButtons.OK,MessageBoxIcon.Information);

            }

            

            
            
        }
        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DesabilatarBotoes();
            DesabilitarCampos();
            LimparCampos();
            btnNovo.Enabled = true;

            ListarGrid();
            alterouFoto = "nao";
        }
        //método desabilita botões
        private void DesabilatarBotoes()
        {
            btnNovo.Enabled=false;
            btnSalvar.Enabled=false;
            btnAlterar.Enabled=false;
            btnExcluir.Enabled=false;
            btnCancelar.Enabled=false;
        }
        //método habilitar botões
        private void HabilitarBotoes()
        {
            btnNovo.Enabled = true;
            btnSalvar.Enabled = true;
            btnAlterar.Enabled=true;
            btnExcluir.Enabled = true;
            btnCancelar.Enabled = true;
        }

        //método habilitar campos
        private void HabilitarCampos()
        {
            txtNome.Enabled = true;
            txtEndereco.Enabled = true;
            txtCPF.Enabled = true;
            txtFone.Enabled = true;
         
        }

        //método desabilitar campos
        private void DesabilitarCampos()
        {
            txtNome.Enabled = false;
            txtEndereco.Enabled = false;
            txtCPF.Enabled = false;
            txtFone.Enabled = false;
        }

        //método limpar camnpos
        private void LimparCampos()
        {
            txtNome.Text = "";
            txtEndereco.Text = "";
            txtCPF.Text = "";
            txtFone.Text = "";
        }

        private void grid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >-1)
            {
                LimparFoto();
                HabilitarBotoes();
                btnNovo.Enabled = false;
                btnSalvar.Enabled = false;
                HabilitarCampos();

                alterouFoto = "nao";

                Id = grid.CurrentRow.Cells[0].Value.ToString();
                txtNome.Text = grid.CurrentRow.Cells[1].Value.ToString();
                txtEndereco.Text = grid.CurrentRow.Cells[2].Value.ToString();
                txtCPF.Text = grid.CurrentRow.Cells[3].Value.ToString();
                txtFone.Text = grid.CurrentRow.Cells[4].Value.ToString();

                cpfAntigo = grid.CurrentRow.Cells[3].Value.ToString();

                //Pegar foto
                if (grid.CurrentRow.Cells[5].Value != DBNull.Value)
                {
                    byte[] imagem = (byte[])grid.Rows[e.RowIndex].Cells[5].Value;
                    MemoryStream ms = new MemoryStream(imagem);

                    Cx_Imagem.Image = Image.FromStream(ms);
                }
                else
                {
                    Cx_Imagem.Image = Properties.Resources.FotoProjeto001;
                }
            }
            else
            {
                return;
            }

        }
        //método buscar por nome
        private void BuscarPorNome()
        {
            conexao.AbrirConexao();
            sql = "SELECT * FROM cliente WHERE nome LIKE @nome ORDER BY nome ASC";  //LIKE, faz busca por aproximação.
            cmd = new MySqlCommand(sql, conexao.con);
            cmd.Parameters.AddWithValue("@nome",txtBuscar.Text + "%"); 
            MySqlDataAdapter da= new MySqlDataAdapter();
            da.SelectCommand = cmd;
            DataTable dt = new DataTable();
            da.Fill(dt);
            grid.DataSource = dt;
            conexao.FecharConexao();
            FormatarGD();

        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            BuscarPorNome();
        }

        private void btnImg_Click(object sender, EventArgs e)
        {
            alterouFoto = "sim";
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "Imagens(*.jpg;*.png) | *.jpg; *.png; *.bmp;"; //Mostra somente PNG, JPG e BMP
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                foto = dlg.FileName.ToString();  //Pega o caminho da imagem no pc
                Cx_Imagem.ImageLocation = foto;

                alterouFoto = "sim";
            }
            else
            {
                alterouFoto = "nao";
            }
        }
        //Método para enviar imagem para o banco de dados
        private byte[] IMG()
        {
            byte[] imagem_byte = null;
            if (foto == "")
            {
                return null;
            }
            FileStream fs = new FileStream(foto,FileMode.Open,FileAccess.Read); //Padrão
            BinaryReader br = new BinaryReader(fs);
            imagem_byte = br.ReadBytes((int)fs.Length);
            return imagem_byte;
        }
        //Método para limpar campo foto
        private void LimparFoto()
        {
            Cx_Imagem.Image = Properties.Resources.FotoProjeto001;
            foto = "img/FotoProjeto001.png";
        }

     
    }//fim
}
