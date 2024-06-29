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
        private string usuarioBD = "";
        private string senhaUsuarioBD = "";
        private string nomeBDFuncao = "";
        private string textoCbbBD = "";
        public string pastaDatabases = Directory.GetCurrentDirectory() + "\\databases";
        private string caminhoBD = "";
        private string[] caminhosBDs = new string[0];
        private List<string> nomesBDs = new List<string>();

        // Vari�veis usadas para permitir que a janela se movimente atrav�s da barra superior customizada
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
            caminhosBDs = Directory.GetFiles(pastaDatabases, "*.sqlite");
            cbbBD.Items.Clear();
            nomesBDs.Clear();

            foreach (string arquivo in caminhosBDs)
            {
                string nomeArquivo = Path.GetFileNameWithoutExtension(arquivo);
                cbbBD.Items.Add(nomeArquivo);
                nomesBDs.Add(nomeArquivo.ToLower() + ".sqlite");
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
            // Verifica��es para permitir a movimenta��o da janela atrav�s da barra superior customizada
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

        private bool validarNomeBD(string input)
        {
            // Remove acentos e caracteres especiais
            string stringNormalizada = input.Normalize(NormalizationForm.FormD);
            string padrao = @"[^a-zA-Z0-9-_]";
            string stringLimpa = Regex.Replace(stringNormalizada, padrao, string.Empty);

            // Verifica se a string � composta apenas por letras, n�meros, "-" e "_"
            // (se o tamanho permanecer o mesmo, n�o houve altera��es, e � v�lida)
            if (stringLimpa.Length == input.Length)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void criarBD()
        {
            caminhoBD = pastaDatabases + $"\\{nomeBDFuncao}";

            try
            {
                // Criar banco
                SQLiteConnection.CreateFile(caminhoBD);

                // Conectar ao banco
                Conexao con = new Conexao(caminhoBD);
                con.conectar();

                // Criar tabela de usu�rios
                string sql = "CREATE TABLE IF NOT EXISTS usuarios (id INTEGER PRIMARY KEY, nome VARCHAR(20) NOT NULL, senha VARCHAR(30) NOT NULL);";
                SQLiteCommand comando = new SQLiteCommand(sql, con.conn);
                comando.ExecuteNonQuery();

                // Inserir usu�rio na tabela
                comando.CommandText = "INSERT INTO usuarios (nome, senha) VALUES(@nome, @senha);";
                comando.Parameters.AddWithValue("@nome", usuarioBD);
                comando.Parameters.AddWithValue("@senha", senhaUsuarioBD);
                comando.ExecuteNonQuery();

                carregarBDs();
                MessageBox.Show("O banco de dados foi criado.", "Opera��o bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnExcluirBD.Enabled = true;
                btnRenomearBD.Enabled = true;
                gpbInfoUsuario.Enabled = true;
                btnCriarBD.Text = "Criar";

                senhaUsuarioBD = "";
                usuarioBD = "";
                nomeBDFuncao = "";
                caminhoBD = "";

                txtNome.Focus();
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message.ToString(), "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Se der erro mas o arquivo de banco foi criado sem usu�rio: excluir
                if (File.Exists(caminhoBD))
                {
                    try
                    {
                        File.Delete(caminhoBD);
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show(erro.Message.ToString(), "Erro ao excluir o banco de dados incompleto", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                btnExcluirBD.Enabled = true;
                btnRenomearBD.Enabled = true;
                gpbInfoUsuario.Enabled = true;
                btnCriarBD.Text = "Criar";

                senhaUsuarioBD = "";
                usuarioBD = "";
                nomeBDFuncao = "";
                caminhoBD = "";

                cbbBD.Text = "";
                cbbBD.Focus();
            }
        }

        private bool verificarSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha))
            {
                MessageBox.Show("A senha n�o pode ser v�zia ou conter espa�os!", "Erro ao registrar senha", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (senha.Length >= 30)
            {
                MessageBox.Show("A senha n�o pode ter mais que 30 caracteres!", "Erro ao registrar senha", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }

        private bool verificarUsuario(string usuario)
        {
            string padrao = @"^[a-zA-Z0-9]+$";

            if (string.IsNullOrWhiteSpace(usuario))
            {
                MessageBox.Show("O nome de usu�rio n�o pode ser v�zio ou conter espa�os!", "Erro ao registrar usu�rio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (cbbBD.Text.Length >= 20)
            {
                MessageBox.Show("O nome de usu�rio n�o pode ter mais que 20 caracteres!", "Erro ao registrar usu�rio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            // Verifica se o nome de usu�rio cont�m apenas letras e n�meros (segue o padr�o)
            else if (!Regex.IsMatch(usuario, padrao))
            {
                MessageBox.Show("O nome de usu�rio deve ser composto apenas de letras e n�meros!", "Erro ao registrar usu�rio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnCriarBD_Click(object sender, EventArgs e)
        {
            // Verifica se est� em modo de registro de senha
            if (btnCriarBD.Text == "Concluir")
            {
                // Verifica a senha e se for v�lida: cria o BD
                if (verificarSenha(cbbBD.Text))
                {
                    senhaUsuarioBD = cbbBD.Text;
                    criarBD();
                }
                else
                {
                    cbbBD.Text = "";
                    cbbBD.Focus();
                }
            }
            // Verifica se est� em modo de registro de usu�rio
            else if (btnCriarBD.Text == "Salvar")
            {
                // Verifica se o usu�rio � v�lido
                if (verificarUsuario(cbbBD.Text))
                {
                    usuarioBD = cbbBD.Text;
                    MessageBox.Show("Insira agora uma senha para finalizar", "Criar banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    btnCriarBD.Text = "Concluir";
                    cbbBD.Text = "";
                    cbbBD.Focus();
                }
                else
                {
                    cbbBD.Text = "";
                    cbbBD.Focus();
                }
            }
            // Verifica se � nulo
            else if (textoCbbBD == "" || textoCbbBD == null || string.IsNullOrWhiteSpace(textoCbbBD))
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
            // Verifica se a string � maior que 30 caracteres
            else if (textoCbbBD.Length >= 30)
            {
                MessageBox.Show("O nome do banco n�o deve conter mais que 30 caracteres!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Text = "";
                cbbBD.Focus();
            }
            // Verifica se utiliza apenas letras e n�meros
            else if (!validarNomeBD(textoCbbBD))
            {
                MessageBox.Show("O nome �nformado deve conter apenas letras e n�meros (sem espa�os)!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Text = "";
                cbbBD.Focus();
            }
            // Verifica se o arquivo j� existe
            else if (File.Exists(caminhoBD))
            {
                MessageBox.Show("J� existe um banco de dados com o nome informado!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Text = "";
                cbbBD.Focus();
            }
            // Sen�o: entra em modo de registro de usu�rio
            else
            {
                nomeBDFuncao = validarExtensaoBD(cbbBD.Text);
                MessageBox.Show("Insira um nome de usu�rio (apenas letras e n�meros)", "Criar banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Information);

                btnExcluirBD.Enabled = false;
                btnRenomearBD.Enabled = false;
                gpbInfoUsuario.Enabled = false;
                btnCriarBD.Text = "Salvar";
                cbbBD.Text = "";
                cbbBD.Focus();
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
                    MessageBox.Show($"O Banco de dados {nomeBD} foi excluido.", "Opera��o bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message.ToString(), "Erro ao excluir o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"N�o foi possivel localizar o banco de dados ou ele n�o existe!", "Erro ao excluir o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            // Verifica se � nulo
            if (textoCbbBD == "" || textoCbbBD == null || string.IsNullOrWhiteSpace(textoCbbBD))
            {
                MessageBox.Show("N�o foi informado um nome para o banco!", tituloMensagem, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Text = "";
                cbbBD.Focus();

                return false;
            }
            // Verifica se o arquivo n�o existe no comboBox
            else if (!nomesBDs.Contains(nomeBD.ToLower()))
            {
                MessageBox.Show("N�o foi possivel localizar o banco de dados ou ele n�o existe!", tituloMensagem, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Text = "";
                cbbBD.Focus();

                return false;
            }
            else if (nomesBDs.Contains(nomeBD.ToLower()))
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
                MessageBox.Show("Por gentileza, anote o nome informado do banco, salve um print da tela de Login e contate o desenvolvedor do programa.", "Erro n�o tratado!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void renomearBD()
        {
            caminhoBD = pastaDatabases + "\\" + nomeBDFuncao;
            // Renomear
            File.Move(caminhoBD, Path.Combine(Path.GetDirectoryName(caminhoBD) + "\\", validarExtensaoBD(cbbBD.Text)));

            MessageBox.Show("O banco de dados foi renomeado com sucesso!", "Opera��o bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnCriarBD.Enabled = true;
            btnExcluirBD.Enabled = true;
            gpbInfoUsuario.Enabled = true;
            btnRenomearBD.Text = "Renomear";

            nomeBDFuncao = "";
            caminhoBD = "";

            carregarBDs();
        }

        private void btnRenomearBD_Click(object sender, EventArgs e)
        {
            // Verificar se o bot�o est� no modo de salvamento (quando o usu�rio j� indicou o banco a ser renomeado)
            if (btnRenomearBD.Text == "Salvar")
            {
                string nomeBD = validarExtensaoBD(textoCbbBD);

                // Verifica se � nulo
                if (textoCbbBD == "" || textoCbbBD == null || string.IsNullOrWhiteSpace(textoCbbBD))
                {
                    MessageBox.Show("N�o foi informado um nome para o banco!", "Erro ao renomear banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbbBD.Text = "";
                    cbbBD.Focus();
                }
                // Verifica se o arquivo existe no comboBox
                else if (nomesBDs.Contains(nomeBD.ToLower()))
                {
                    // Verificar se o arquivo encontrado � o mesmo que deseja-se renomear (se sim: pode-se alterar a capitaliza��o das letras desde que n�o seja igual ao j� existente)
                    if (nomeBD.ToLower() == nomeBDFuncao.ToLower())
                    {
                        // Se o nome for exatamente igual: erro
                        if (nomeBD == nomeBDFuncao)
                        {
                            MessageBox.Show("O nome informado � exatamente igual ao anterior!", "Erro ao renomear banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            cbbBD.Text = "";
                            cbbBD.Focus();
                        }
                        // Sen�o: renomear
                        else
                        {
                            renomearBD();
                        }
                    }
                    else
                    {
                        MessageBox.Show("J� existe um banco de dados com esse nome!", "Erro ao renomear banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cbbBD.Text = "";
                        cbbBD.Focus();
                    }
                }
                // Verifica se o arquivo n�o existe no comboBox
                else if (!nomesBDs.Contains(nomeBD.ToLower()))
                {
                    // Verifica se come�a com n�mero
                    if (char.IsDigit(textoCbbBD[0]))
                    {
                        MessageBox.Show("O nome informado n�o pode come�ar com um n�mero!", "Erro ao renomear banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cbbBD.Text = "";
                        cbbBD.Focus();
                    }
                    // Verifica se utiliza apenas letras e n�meros
                    else if (!validarNomeBD(textoCbbBD))
                    {
                        MessageBox.Show("O nome �nformado deve conter apenas letras e n�meros (sem espa�os)!", "Erro ao renomear banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cbbBD.Text = "";
                        cbbBD.Focus();
                    }
                    // Passou nas verifica��es, tentar renomear
                    else
                    {
                        try
                        {
                            renomearBD();
                        }
                        catch (Exception error)
                        {
                            MessageBox.Show(error.Message.ToString(), "Erro ao renomear banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Por gentileza, anote o nome informado do banco, salve um print da tela de Login e contate o desenvolvedor do programa.", "Erro n�o tratado!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            // Verificar se o banco existe e se existir: travar demais bot�es, exibir mensagem e aguardar o usu�rio inserir um novo nome v�lido.
            else if (verificarExistenciaBD("Erro ao localizar banco de dados"))
            {
                // Salvar nome do banco a ser renomeado
                nomeBDFuncao = validarExtensaoBD(textoCbbBD);

                MessageBox.Show("Banco de dados selecionado. Insira um novo nome e confirme ou tecle ESC para cancelar", "Renomear arquivo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Entrar em modo de salvamento
                btnCriarBD.Enabled = false;
                btnExcluirBD.Enabled = false;
                gpbInfoUsuario.Enabled = false;
                btnRenomearBD.Text = "Salvar";

                cbbBD.Text = "";
                cbbBD.Focus();
            }
            else
            {
                nomeBDFuncao = "";
            }
        }

        private void handleKeyPressCriarBD(KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                btnExcluirBD.Enabled = true;
                btnRenomearBD.Enabled = true;
                gpbInfoUsuario.Enabled = true;
                btnCriarBD.Text = "Criar";

                senhaUsuarioBD = "";
                usuarioBD = "";
                nomeBDFuncao = "";
                caminhoBD = "";

                cbbBD.Text = "";
                cbbBD.Focus();
            }
            else if (e.KeyChar == (char)Keys.Enter)
            {
                btnCriarBD.PerformClick();
            }
        }

        private void handleKeyPressRenomearBD(KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                btnCriarBD.Enabled = true;
                btnExcluirBD.Enabled = true;
                gpbInfoUsuario.Enabled = true;
                btnRenomearBD.Text = "Renomear";
                cbbBD.Text = "";
                nomeBDFuncao = "";
                caminhoBD = "";
            }
            else if (e.KeyChar == (char)Keys.Enter)
            {
                btnRenomearBD.PerformClick();
            }
        }

        private void cbbBD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (btnRenomearBD.Text == "Salvar")
            {
                handleKeyPressRenomearBD(e);
            }
            if (btnCriarBD.Text == "Salvar" || btnCriarBD.Text == "Concluir")
            {
                handleKeyPressCriarBD(e);
            }
        }

        private void btnRenomearBD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (btnRenomearBD.Text == "Salvar")
            {
                handleKeyPressRenomearBD(e);
            }
        }

        private void btnCriarBD_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (btnCriarBD.Text == "Salvar" || btnCriarBD.Text == "Concluir")
            {
                handleKeyPressCriarBD(e);
            }
        }
    }
}
