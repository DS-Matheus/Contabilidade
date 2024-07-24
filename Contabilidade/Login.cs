using Contabilidade.Models;
using System.Data;
using System.Data.SQLite;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;

namespace Contabilidade
{
    public partial class frmLogin : Form
    {
        private string usuarioBD = "";
        private string senhaUsuarioBD = "";
        private string nomeBD = "";

        private string pastaDatabases = Path.Combine(Application.StartupPath, "databases");
        private string caminhoBD = "";

        private List<string> nomesBDs = new List<string>();

        // Fun��es usadas para permitir que a janela se movimente atrav�s da barra superior customizada
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        public frmLogin()
        {
            InitializeComponent();
        }

        private void carregarBDs()
        {
            string[] caminhosBDs = Directory.GetFiles(pastaDatabases, "*.sqlite");
            cbbBD.Items.Clear();
            nomesBDs.Clear();

            foreach (string arquivo in caminhosBDs)
            {
                string nomeArquivo = Path.GetFileNameWithoutExtension(arquivo);
                cbbBD.Items.Add(nomeArquivo);
                nomesBDs.Add($"{nomeArquivo.ToLower()}.sqlite");
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

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pnlBarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
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
            caminhoBD = $"{pastaDatabases}\\{nomeBD}";

            try
            {
                // Criar banco
                SQLiteConnection.CreateFile(caminhoBD);

                // Conectar ao banco
                Conexao con = new Conexao(caminhoBD);
                con.conectar();

                // Criar tabela de usu�rios
                string sql = "CREATE TABLE IF NOT EXISTS usuarios (id INTEGER PRIMARY KEY, nome VARCHAR(20) NOT NULL UNIQUE, senha VARCHAR(30) NOT NULL);";
                SQLiteCommand comando = new SQLiteCommand(sql, con.conn);
                comando.ExecuteNonQuery();

                // Inserir usu�rio na tabela
                comando.CommandText = "INSERT INTO usuarios (nome, senha) VALUES(@nome, @senha);";
                comando.Parameters.AddWithValue("@nome", usuarioBD);
                comando.Parameters.AddWithValue("@senha", senhaUsuarioBD);
                comando.ExecuteNonQuery();

                carregarBDs();

                // Fechar conex�es
                con.excluir();
                comando.Dispose();

                cbbBD.Text = nomeBD.Replace(".sqlite", "");

                MessageBox.Show("O banco de dados foi criado.", "Opera��o bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);

                resetarForm();
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

                resetarForm(true);
                cbbBD.Focus();
            }
        }

        public static bool verificarSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha) || senha.Contains(" "))
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

        public static bool verificarUsuario(string usuario)
        {
            string padrao = @"^[a-zA-Z0-9]+$";

            if (string.IsNullOrWhiteSpace(usuario))
            {
                MessageBox.Show("O nome de usu�rio n�o pode ser v�zio ou conter espa�os!", "Erro ao registrar usu�rio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (usuario.Length >= 20)
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
            caminhoBD = $"{pastaDatabases}\\{validarExtensaoBD(cbbBD.Text)}";

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
                    MessageBox.Show("Qual ser� a senha desse usu�rio?", "Criar banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Question);

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
            // Sen�o: verifica se � nulo
            else if (cbbBD.Text == "" || cbbBD.Text == null || string.IsNullOrWhiteSpace(cbbBD.Text))
            {
                MessageBox.Show("N�o foi informado um nome para o banco!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Text = "";
                cbbBD.Focus();
            }
            // Verifica se come�a com n�mero
            else if (char.IsDigit(cbbBD.Text[0]))
            {
                MessageBox.Show("O nome informado n�o pode come�ar com um n�mero!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Text = "";
                cbbBD.Focus();
            }
            // Verifica se a string � maior que 30 caracteres
            else if (cbbBD.Text.Length >= 30)
            {
                MessageBox.Show("O nome do banco n�o deve conter mais que 30 caracteres!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Text = "";
                cbbBD.Focus();
            }
            // Verifica se utiliza apenas letras e n�meros
            else if (!validarNomeBD(cbbBD.Text))
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
                nomeBD = validarExtensaoBD(cbbBD.Text);
                MessageBox.Show("Qual o nome do usu�rio a se registrar?", "Criar banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Question);

                btnExcluirBD.Enabled = false;
                btnRenomearBD.Enabled = false;
                gpbInfoUsuario.Enabled = false;
                btnCriarBD.Text = "Salvar";
                cbbBD.Text = "";
                cbbBD.Focus();
            }
        }

        public void excluirBD(string caminho)
        {
            if (File.Exists(caminho))
            {
                try
                {
                    File.Delete(caminho);
                    MessageBox.Show($"O Banco de dados {nomeBD.Replace(".sqlite", "")} foi exclu�do.", "Opera��o bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message.ToString(), "Erro ao excluir o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"N�o foi possivel localizar o banco de dados ou ele n�o existe!", "Erro ao excluir o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public string validarExtensaoBD(string nomeBanco)
        {
            if (nomeBanco.EndsWith(".sqlite"))
                return nomeBanco;
            else
                return $"{nomeBanco}.sqlite";
        }

        public bool verificarExistenciaBD(string tituloMensagem)
        {
            // Salvar nome do banco para as opera��es futuras
            nomeBD = validarExtensaoBD(cbbBD.Text);

            // Verifica se � nulo
            if (cbbBD.Text == "" || cbbBD.Text == null || string.IsNullOrWhiteSpace(cbbBD.Text))
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
            // Exibe uma caixa de di�logo de confirma��o
            DialogResult resultado = MessageBox.Show("Voc� realmente deseja excluir o banco de dados? Esse processo � irrevers�vel.", "Excluir banco de dados", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            // Verifica a resposta do usu�rio
            if (resultado == DialogResult.Yes)
            {
                if (verificarExistenciaBD("Erro ao excluir banco de dados"))
                {
                    caminhoBD = $"{pastaDatabases}\\{nomeBD}";

                    excluirBD(caminhoBD);
                    carregarBDs();

                    cbbBD.Text = "";
                    cbbBD.Focus();
                }
            }
            else
            {
                Console.WriteLine("Opera��o de exclus�o cancelada pelo usu�rio.");
            }
        }

        private void renomearBD(string nomeBDNovo)
        {
            caminhoBD = $"{pastaDatabases}\\{nomeBD}";
            // Renomear
            File.Move(caminhoBD, $"{Path.GetDirectoryName(caminhoBD)}\\{nomeBDNovo}");

            MessageBox.Show("O banco de dados foi renomeado com sucesso!", "Opera��o bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);

            resetarForm();

            carregarBDs();
        }

        private void btnRenomearBD_Click(object sender, EventArgs e)
        {
            // Verificar se o bot�o est� no modo de salvamento (quando o usu�rio j� indicou o banco a ser renomeado)
            if (btnRenomearBD.Text == "Salvar")
            {
                string nomeBDNovo = validarExtensaoBD(cbbBD.Text);

                // Verifica se � nulo
                if (cbbBD.Text == "" || cbbBD.Text == null || string.IsNullOrWhiteSpace(cbbBD.Text))
                {
                    MessageBox.Show("N�o foi informado um nome para o banco!", "Erro ao renomear banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbbBD.Text = "";
                    cbbBD.Focus();
                }
                // Verifica se o arquivo existe no comboBox
                else if (nomesBDs.Contains(nomeBDNovo.ToLower()))
                {
                    // Verificar se o arquivo encontrado � o mesmo que deseja-se renomear (se sim: pode-se alterar a capitaliza��o das letras desde que n�o seja igual ao j� existente)
                    if (nomeBDNovo.ToLower() == nomeBD.ToLower())
                    {
                        // Se o nome for exatamente igual: erro
                        if (nomeBDNovo == nomeBD)
                        {
                            MessageBox.Show("O nome informado � exatamente igual ao anterior!", "Erro ao renomear banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            cbbBD.Text = "";
                            cbbBD.Focus();
                        }
                        // Sen�o: renomear
                        else
                        {
                            renomearBD(nomeBDNovo);
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
                else if (!nomesBDs.Contains(nomeBDNovo.ToLower()))
                {
                    // Verifica se come�a com n�mero
                    if (char.IsDigit(cbbBD.Text[0]))
                    {
                        MessageBox.Show("O nome informado n�o pode come�ar com um n�mero!", "Erro ao renomear banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cbbBD.Text = "";
                        cbbBD.Focus();
                    }
                    // Verifica se utiliza apenas letras e n�meros
                    else if (!validarNomeBD(cbbBD.Text))
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
                            renomearBD(nomeBDNovo);
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
                nomeBD = "";
            }
        }

        private void btnEntrar_Click(object sender, EventArgs e)
        {
            // Verifica se o banco de dados existe
            if (!verificarExistenciaBD("Erro ao localizar banco de dados"))
            {
                return;
            }
            // Verifica se a senha foi digitada
            else if (txtNome.Text == "")
            {
                MessageBox.Show("Usu�rio n�o informado!", "Formul�rio Incompleto", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNome.Focus();

            }
            // Verifica se o usu�rio foi informado
            else if (txtSenha.Text == "")
            {
                MessageBox.Show("Senha n�o informada!", "Formul�rio Incompleto", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSenha.Focus();
            }
            else
            {
                try
                {
                    // Conectar ao banco
                    caminhoBD = $"{pastaDatabases}\\{nomeBD}";
                    Conexao con = new Conexao(caminhoBD);
                    con.conectar();

                    // Query de pesquisa
                    string sql = "SELECT * FROM usuarios WHERE nome= @nome AND senha= @senha;";
                    SQLiteCommand comando = new SQLiteCommand(sql, con.conn);
                    // Par�metros
                    comando.Parameters.AddWithValue("@nome", txtNome.Text);
                    comando.Parameters.AddWithValue("@senha", txtSenha.Text);

                    // Consultar usu�rios com os par�metros informados
                    SQLiteDataAdapter dados = new SQLiteDataAdapter(comando);
                    DataTable dtUsuario = new DataTable();
                    // Passando os dados encontrados pelo DataAdapter para o DataTable
                    dados.Fill(dtUsuario);

                    // Liberar recursos;
                    comando.Dispose();

                    if (dtUsuario.Rows.Count <= 0)
                    {
                        MessageBox.Show("Usu�rio ou senha inv�lido(s)!", "Registro n�o encontrado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtSenha.Clear();
                        txtNome.Focus();
                    }
                    else
                    {
                        string usuario = dtUsuario.Rows[0]["nome"].ToString();

                        MessageBox.Show($"Bem Vindo(a) {usuario}.", "Login", MessageBoxButtons.OK, MessageBoxIcon.None);

                        this.Visible = false;
                        frmPainelPrincipal frmPainelPrincipal = new frmPainelPrincipal(nomeBD.Replace(".sqlite", ""), usuario, con); // Crie uma inst�ncia do frmPainelPrincipal
                        frmPainelPrincipal.Owner = this; // Definir o painel de Login como pai
                        frmPainelPrincipal.ShowDialog(); // Exibe o form como uma janela de di�logo modal

                        // Ap�s fechar o Painel Principal
                        carregarBDs();

                        resetarForm(true);
                        txtNome.Text = "";
                        txtSenha.Text = "";
                        cbbBD.Focus();
                        this.Visible = true;
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message.ToString(), "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void handleKeyPressCriarBD(KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                resetarForm(true);
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
                resetarForm(true);
                cbbBD.Focus();
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

        private void resetarForm(bool completo = false)
        {
            btnCriarBD.Enabled = true;
            btnExcluirBD.Enabled = true;
            btnRenomearBD.Enabled = true;
            gpbInfoUsuario.Enabled = true;

            btnCriarBD.Text = "Criar";
            btnExcluirBD.Text = "Excluir";
            btnRenomearBD.Text = "Renomear";

            usuarioBD = "";
            senhaUsuarioBD = "";
            nomeBD = "";
            caminhoBD = "";

            if (completo)
            {
                cbbBD.Text = "";
            }
        }

        private void label4_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void handleKeyPressEntrar(KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                txtNome.Text = "";
                txtSenha.Text = "";
                txtNome.Focus();
            }
            else if (e.KeyChar == (char)Keys.Enter)
            {
                btnEntrar.PerformClick();
            }
        }

        private void txtNome_KeyPress(object sender, KeyPressEventArgs e)
        {
            handleKeyPressEntrar(e);
        }

        private void txtSenha_KeyPress(object sender, KeyPressEventArgs e)
        {
            handleKeyPressEntrar(e);
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            // Pergunta inicial ao usu�rio sobre o tipo de backup
            DialogResult result = MessageBox.Show("Deseja fazer backup apenas do banco de dados selecionado?", "Backup", MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                var nomeBanco = cbbBD.Text;

                // Verifica se � nulo
                if (cbbBD.Text == "" || cbbBD.Text == null || string.IsNullOrWhiteSpace(cbbBD.Text))
                {
                    MessageBox.Show("N�o foi informado um nome para o banco!", "Erro ao fazer backup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbbBD.Text = "";
                    cbbBD.Focus();
                }
                // Se o arquivo do banco existir, fazer backup 
                else if (File.Exists($"{pastaDatabases}\\{validarExtensaoBD(nomeBanco)}"))
                {
                    frmPainelPrincipal.FazerBackupBancoAtual(pastaDatabases, nomeBanco);
                }
                else
                {
                    MessageBox.Show("N�o foi encontrado o arquivo do banco de dados selecionado", "Erro ao fazer backup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbbBD.Text = "";
                    cbbBD.Focus();
                }

            }
            else if (result == DialogResult.No)
            {
                frmPainelPrincipal.FazerBackupTodosBancos(pastaDatabases);
            }
            // Se result for DialogResult.Cancel, n�o faz nada (cancela a opera��o)
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            // Pergunta inicial ao usu�rio
            DialogResult respostaInicial = MessageBox.Show("Deseja restaurar apenas um banco de dados?",
                                                           "Restaurar banco de dados",
                                                           MessageBoxButtons.YesNoCancel,
                                                           MessageBoxIcon.Question);

            if (respostaInicial == DialogResult.Yes)
            {
                // Selecionar arquivo .sqlite para restaura��o individual
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Arquivos SQLite (*.sqlite)|*.sqlite";
                openFileDialog.Title = "Selecionar Arquivo SQLite para Backup";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string arquivoSelecionado = openFileDialog.FileName;

                    // Verificar se o arquivo j� existe na pastaDatabases
                    string nomeArquivo = Path.GetFileNameWithoutExtension(arquivoSelecionado);
                    string destino = Path.Combine(pastaDatabases, Path.GetFileName(arquivoSelecionado));

                    if (File.Exists(destino))
                    {
                        // Arquivo existe, perguntar se deseja sobrescrever
                        DialogResult respostaSobrescrever = MessageBox.Show($"O arquivo \"{nomeArquivo}\" j� existe. Deseja sobrescrev�-lo?",
                                                                              "Sobrescrever Arquivo",
                                                                              MessageBoxButtons.YesNo,
                                                                              MessageBoxIcon.Question);

                        if (respostaSobrescrever == DialogResult.Yes)
                        {
                            // Sobrescrever o arquivo
                            File.Copy(arquivoSelecionado, destino, true);
                            MessageBox.Show($"Arquivo \"{nomeArquivo}\" sobrescrito com sucesso.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        // Se respostaSobrescrever for No, n�o faz nada (cancela a opera��o)
                    }
                    else
                    {
                        // Arquivo n�o existe, copiar para a pastaDatabases
                        File.Copy(arquivoSelecionado, destino);
                        MessageBox.Show($"Arquivo '{nomeArquivo}' copiado para a pastaDatabases.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                // Se o usu�rio cancelar a sele��o do arquivo, n�o faz nada
            }
            else if (respostaInicial == DialogResult.No)
            {
                // Selecionar pasta para backup de m�ltiplos arquivos .sqlite
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                folderBrowserDialog.Description = "Selecione a pasta que cont�m os arquivos SQLite para backup.";

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string pastaSelecionada = folderBrowserDialog.SelectedPath;
                    string[] arquivosSQLite = Directory.GetFiles(pastaSelecionada, "*.sqlite");

                    if (arquivosSQLite.Length == 0)
                    {
                        MessageBox.Show("N�o foram encontrados arquivos de banco de dados na pasta selecionada.",
                                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Cancela a fun��o, pois n�o h� arquivos para copiar
                    }

                    // Verificar se deseja sobrescrever todos os arquivos na pastaDatabases
                    DialogResult respostaSobrescreverTodos = MessageBox.Show("Deseja sobrescrever todos os arquivos que j� existem na pasta do programa?",
                                                                              "Sobrescrever Todos",
                                                                              MessageBoxButtons.YesNo,
                                                                              MessageBoxIcon.Question);

                    if (respostaSobrescreverTodos == DialogResult.Yes)
                    {
                        // Sobrescrever todos os arquivos
                        foreach (string arquivo in arquivosSQLite)
                        {
                            string destino = Path.Combine(pastaDatabases, Path.GetFileName(arquivo));
                            File.Copy(arquivo, destino, true);
                        }
                        MessageBox.Show("Todos os arquivos foram copiados para pastaDatabases com sucesso.",
                                        "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (respostaSobrescreverTodos == DialogResult.No)
                    {
                        // Perguntar ao usu�rio se deseja selecionar quais arquivos ser�o sobrescritos
                        DialogResult respostaSelecionarArquivos = MessageBox.Show("Deseja ent�o selecionar quais arquivos ser�o sobrescritos?",
                                                                                   "Selecionar Arquivos",
                                                                                   MessageBoxButtons.YesNo,
                                                                                   MessageBoxIcon.Question);

                        if (respostaSelecionarArquivos == DialogResult.Yes)
                        {
                            // Escolher quais arquivos sobrescrever
                            foreach (string arquivo in arquivosSQLite)
                            {
                                string nomeArquivo = Path.GetFileNameWithoutExtension(arquivo);
                                string destino = Path.Combine(pastaDatabases, Path.GetFileName(arquivo));

                                // Verificar se o arquivo j� existe no destino
                                if (File.Exists(destino))
                                {
                                    // Perguntar ao usu�rio se deseja sobrescrever somente se o arquivo existir
                                    DialogResult respostaSobrescrever = MessageBox.Show($"O arquivo '{nomeArquivo}' j� existe na pasta de destino. Deseja sobrescrev�-lo?",
                                                                                         "Sobrescrever Arquivo",
                                                                                         MessageBoxButtons.YesNo,
                                                                                         MessageBoxIcon.Question);

                                    if (respostaSobrescrever == DialogResult.Yes)
                                    {
                                        File.Copy(arquivo, destino, true);
                                    }
                                    // Se respostaSobrescrever for No, n�o faz nada (n�o sobrescreve o arquivo)
                                }
                                else
                                {
                                    // Arquivo n�o existe no destino, copiar diretamente
                                    File.Copy(arquivo, destino);
                                }
                            }

                            MessageBox.Show($"Todos os arquivos confirmados foram restaurados com sucesso.",
                                            "Restaura��o bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (respostaSelecionarArquivos == DialogResult.No)
                        {
                            // Copiar todos os arquivos sem sobrescrever
                            List<string> bancosNaoCopiados = new List<string>();
                            int contadorSucessos = 0;

                            foreach (string arquivo in arquivosSQLite)
                            {
                                string nomeArquivo = Path.GetFileNameWithoutExtension(arquivo);
                                string destino = Path.Combine(pastaDatabases, Path.GetFileName(arquivo));

                                // Verificar se o arquivo j� existe na pastaDatabases
                                if (File.Exists(destino))
                                {
                                    // Arquivo j� existe, adicionar � lista de bancos n�o copiados
                                    bancosNaoCopiados.Add(nomeArquivo);
                                }
                                else
                                {
                                    // Copiar o arquivo para a pastaDatabases
                                    File.Copy(arquivo, destino);
                                    contadorSucessos++;
                                }
                            }

                            if (contadorSucessos == 0)
                            {
                                MessageBox.Show("Nenhum arquivo foi copiado pois todos j� existem na pasta do programa.",
                                                "Restaura��o conclu�da", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else if (bancosNaoCopiados.Count == 0)
                            {
                                MessageBox.Show($"Todos os arquivos foram restaurados.",
                                                "Restaura��o bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                MessageBox.Show($"Os seguintes arquivos n�o foram restaurados porque j� existem na pasta do programa:\n\n{string.Join("\n", bancosNaoCopiados)}", "Arquivos n�o copiados");
                            }
                        }
                        // Se o usu�rio cancelar, n�o faz nada
                    }
                    // Se o usu�rio cancelar, n�o faz nada
                }
                // Se o usu�rio cancelar a sele��o da pasta, n�o faz nada
            }
            // Se respostaInicial for Cancel, n�o faz nada
        }
    }
}
