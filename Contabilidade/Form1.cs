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
        public string[] caminhosBDs = new string[0];
        public List<string> nomesBDs = new List<string>();

        // Variáveis usadas para permitir que a janela se movimente através da barra superior customizada
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

        private void carregarBDs()
        {
            caminhosBDs = Directory.GetFiles(pastaDatabases, "*.sqlite");
            cbbBD.Items.Clear();
            nomesBDs.Clear();

            foreach (string arquivo in caminhosBDs)
            {
                string nomeArquivo = Path.GetFileNameWithoutExtension(arquivo);
                cbbBD.Items.Add(nomeArquivo);
                nomesBDs.Add(nomeArquivo + ".sqlite");
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
            // Verificações para permitir a movimentação da janela através da barra superior customizada
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
                // Se o checkbox estiver marcado, torna a senha visível
                txtSenha.UseSystemPasswordChar = false;
            }
            else
            {
                // Se o checkbox não estiver marcado, oculta a senha
                txtSenha.UseSystemPasswordChar = true;
            }
        }

        private bool validarNomeBD(string input)
        {
            // Remove acentos e caracteres especiais
            string stringNormalizada = input.Normalize(NormalizationForm.FormD);
            string padrao = @"[^a-zA-Z0-9-_]";
            string stringLimpa = Regex.Replace(stringNormalizada, padrao, string.Empty);

            // Verifica se a string é composta apenas por letras, números, "-" e "_"
            // (se o tamanho permanecer o mesmo, não houve alterações, e é válida)
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
            // Verifica se é nulo
            if (textoCbbBD == "" || textoCbbBD == null || string.IsNullOrWhiteSpace(textoCbbBD))
            {
                MessageBox.Show("Não foi informado um nome para o banco!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Text = "";
                cbbBD.Focus();
            }
            // Verifica se começa com número
            else if (char.IsDigit(textoCbbBD[0]))
            {
                MessageBox.Show("O nome informado não pode começar com um número!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Text = "";
                cbbBD.Focus();
            }
            // Verifica se utiliza apenas letras e números
            else if (!validarNomeBD(textoCbbBD))
            {
                MessageBox.Show("O nome ínformado deve conter apenas letras e números (sem espaços)!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                        MessageBox.Show("O banco de dados foi criado.", "Operação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Já existe um banco de dados com o nome informado!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cbbBD.Text = "";
                        cbbBD.Focus();
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message.ToString(), "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cbbBD_TextChanged(object sender, EventArgs e)
        {
            textoCbbBD = cbbBD.Text;
        }

        public void excluirBD(string caminho, string nomeBD)
        {
            if (File.Exists(caminho))
            {
                try
                {
                    File.Delete(caminho);
                    MessageBox.Show($"O Banco de dados {nomeBD} foi excluido.", "Operação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message.ToString(), "Erro ao excluir o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"Não foi possivel localizar o banco de dados ou ele não existe!", "Erro ao excluir o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public string validarExtensaoBD(string nomeBanco)
        {
            if (nomeBanco.EndsWith(".sqlite"))
                return nomeBanco;
            else
                return nomeBanco + ".sqlite";
        }

        public bool verificarExistenciaBD(string tituloMensagem)
        {
            string nomeBD = validarExtensaoBD(textoCbbBD);

            // Verifica se é nulo
            if (textoCbbBD == "" || textoCbbBD == null || string.IsNullOrWhiteSpace(textoCbbBD))
            {
                MessageBox.Show("Não foi informado um nome para o banco!", tituloMensagem, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Text = "";
                cbbBD.Focus();

                return false;
            }
            // Verifica se o arquivo não existe no comboBox
            else if (!nomesBDs.Contains(nomeBD))
            {
                MessageBox.Show("Não foi possivel localizar o banco de dados ou ele não existe!", tituloMensagem, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Text = "";
                cbbBD.Focus();

                return false;
            }
            else if (nomesBDs.Contains(nomeBD))
            {
                return true;
            }
            // Verifica se nenhum banco foi selecionado
            else if (cbbBD.SelectedItem == null)
            {
                MessageBox.Show("Nenhum banco de dados selecionado!", tituloMensagem, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Focus();

                return false;
            }
            else
            {
                MessageBox.Show("Por gentileza, anote o nome informado do banco, salve um print da tela de Login e contate o desenvolvedor do programa.", "Erro não tratado!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnExcluirBD_Click(object sender, EventArgs e)
        {
            string nomeBD = validarExtensaoBD(textoCbbBD);

            if (verificarExistenciaBD("Erro ao excluir banco de dados"))
            {
                caminhoBD = pastaDatabases + "\\" + nomeBD;

                excluirBD(caminhoBD, nomeBD);
                carregarBDs();

                cbbBD.Text = "";
                cbbBD.Focus();
            }
        }
    }
}
