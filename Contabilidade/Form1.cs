using Contabilidade.Models;
using System.Data;
using System.Data.SQLite;
using System;
using System.IO;
using System.Windows.Forms;

namespace Contabilidade
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void splitContainer1_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            if (cbbBD.SelectedItem == null)
            {
                MessageBox.Show("Nenhum banco de dados selecionado!", "Formulário Incompleto", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbbBD.Focus();
            }
            else if (txtSenha.Text == "")
            {
                MessageBox.Show("Senha não informada!", "Formulário Incompleto", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSenha.Focus();
            }
            else if (txtNome.Text == "")
            {
                MessageBox.Show("Usuário não informado!", "Formulário Incompleto", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNome.Focus();
            }
            else
            {
                try
                {
                    Conexao con = new Conexao(cbbBD.SelectedItem.ToString());

                    con.conectar();

                    string sql = "SELECT * FROM usuarios WHERE nome = '" + txtNome.Text + "' AND senha = '" + txtSenha.Text + "'";

                    SQLiteDataAdapter dados = new SQLiteDataAdapter(sql, con.conn); // Consultar usuários com os parâmetros informados
                    DataTable usuario = new DataTable();

                    dados.Fill(usuario); // Passando os dados encontrados pelo DataAdapter para o DataTable

                    if (usuario.Rows.Count < 0)
                    {
                        MessageBox.Show("Usuário ou senha inválido(s)!", "Registro não encontrado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtSenha.Clear();
                        txtNome.Focus();
                    }
                    else
                    {
                        string nome = usuario.Rows[0]["nome"].ToString();

                        MessageBox.Show("Bem Vindo(a) " + nome + ".", "Login", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            string pastaDatabases = @".\databases";

            if (Directory.Exists(pastaDatabases))
            {
                string[] arquivosSgb = Directory.GetFiles(pastaDatabases, "*.sgb");

                foreach (string arquivo in arquivosSgb)
                {
                    string nomeArquivo = Path.GetFileName(arquivo);
                    cbbBD.Items.Add(nomeArquivo);
                }
            }
            else
            {
                Directory.CreateDirectory(pastaDatabases);
            }
        }
    }
}
