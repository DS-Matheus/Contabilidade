using Contabilidade.Models;
using System.Data;
using System.Data.SQLite;
using System;
using System.IO;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text.RegularExpressions;
using System.Text;

namespace Contabilidade
{
    public partial class frmLogin : Form
    {
        protected string textoCbbBD = "";
        public string pastaDatabases = Directory.GetCurrentDirectory() + "\\databases";
        public string caminhoBD = "";
        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ReleaseCapture();

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
                MessageBox.Show("Nenhum banco de dados selecionado!", "Formul�rio Incompleto", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                cbbBD.Focus();
            }
            else if (txtSenha.Text == "")
            {
                MessageBox.Show("Senha n�o informada!", "Formul�rio Incompleto", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSenha.Focus();
            }
            else if (txtNome.Text == "")
            {
                MessageBox.Show("Usu�rio n�o informado!", "Formul�rio Incompleto", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNome.Focus();
            }
            else
            {
                try
                {
                    Conexao con = new Conexao(cbbBD.SelectedItem.ToString());

                    con.conectar();

                    string sql = "SELECT * FROM usuarios WHERE nome = '" + txtNome.Text + "' AND senha = '" + txtSenha.Text + "'";

                    SQLiteDataAdapter dados = new SQLiteDataAdapter(sql, con.conn); // Consultar usu�rios com os par�metros informados
                    DataTable usuario = new DataTable();

                    dados.Fill(usuario); // Passando os dados encontrados pelo DataAdapter para o DataTable

                    if (usuario.Rows.Count < 0)
                    {
                        MessageBox.Show("Usu�rio ou senha inv�lido(s)!", "Registro n�o encontrado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

        private void carregarBDs()
        {
            string[] arquivosSgb = Directory.GetFiles(pastaDatabases, "*.sqlite");

            foreach (string arquivo in arquivosSgb)
            {
                string nomeArquivo = Path.GetFileName(arquivo);
                cbbBD.Items.Add(nomeArquivo);
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            if (Directory.Exists(pastaDatabases))
            {
                carregarBDs();
            }
            else
            {
                Directory.CreateDirectory(pastaDatabases);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void panel2_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void chbVisibilidadeSenha_CheckedChanged(object sender, EventArgs e)
        {
            if (chbVisibilidadeSenha.Checked)
            {
                // Se o checkbox estiver marcado, torna a senha vis�vel
                txtSenha.UseSystemPasswordChar = false;
            }
            else
            {
                // Se o checkbox n�o estiver marcado, oculta a senha
                txtSenha.UseSystemPasswordChar = true;
            }
        }

        private bool ValidarNomeBD(string input)
        {
            // Remove acentos e caracteres especiais
            string stringNormalizada = input.Normalize(NormalizationForm.FormD);
            string padrao = @"[^a-zA-Z0-9]";
            string stringLimpa = Regex.Replace(stringNormalizada, padrao, string.Empty);

            // Verifica se a string � composta apenas por letras e n�meros (se o tamanho permanecer o mesmo (n�o houve altera��es) � v�lida)
            if (stringLimpa.Length == input.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void btnCriarBD_Click(object sender, EventArgs e)
        {
            // Verifica se � nulo
            if (textoCbbBD == "" || textoCbbBD == null)
            {
                MessageBox.Show("N�o foi informado um nome para o banco!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Text = "";
                cbbBD.Focus();
            }
            // Verifica se come�a com n�mero
            else if (char.IsDigit(textoCbbBD[0]))
            {
                MessageBox.Show("O nome informado n�o pode come�ar com um n�mero!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Text = "";
                cbbBD.Focus();
            }
            // Verifica se utiliza apenas letras e n�meros
            else if (!ValidarNomeBD(textoCbbBD))
            {
                MessageBox.Show("O nome �nformado deve conter apenas letras e n�meros (sem espa�os)!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Text = "";
                cbbBD.Focus();
            }
            else
            {
                try
                {
                    caminhoBD = pastaDatabases + $"\\{textoCbbBD}.sqlite";
                    if (!File.Exists(caminhoBD))
                    {
                        SQLiteConnection.CreateFile(caminhoBD);
                        carregarBDs();
                        MessageBox.Show("O banco de dados foi criado.", "Opera��o bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("J� existe um banco de dados com o nome informado!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cbbBD.Text = "";
                        cbbBD.Focus();
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message.ToString(), "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void cbbBD_TextChanged(object sender, EventArgs e)
        {
            textoCbbBD = cbbBD.Text;
        }
    }
}
