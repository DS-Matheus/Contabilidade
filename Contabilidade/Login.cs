using Contabilidade.Models;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SQLite;
using Contabilidade.Classes;

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

        // Funções usadas para permitir que a janela se movimente através da barra superior customizada
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

        public static void testarResultadoComando(int resultado, string mensagem)
        {
            if (resultado == 0)
            {
                throw new CustomException(mensagem);
            }
        }

        private void criarBD()
        {
            string caminhoBD = $"{pastaDatabases}\\{nomeBD}";

            // Criar banco de dados
            SQLiteConnection.CreateFile(caminhoBD);

            try
            {
                // Conectar ao banco criado
                using (var conexaoBanco = new SQLiteConnection("Data Source=" + caminhoBD))
                {
                    conexaoBanco.Open();

                    using (var comando = new SQLiteCommand("", conexaoBanco))
                    {
                        // Criar tabela de usuários
                        comando.CommandText = "CREATE TABLE IF NOT EXISTS usuarios (id INTEGER PRIMARY KEY AUTOINCREMENT, nome VARCHAR(20) NOT NULL UNIQUE, senha VARCHAR(30) NOT NULL);";
                        comando.ExecuteNonQuery();

                        // Inserir usuário na tabela
                        comando.CommandText = "INSERT INTO usuarios (nome, senha) VALUES(@nome, @senha);";
                        comando.Parameters.AddWithValue("@nome", usuarioBD);
                        comando.Parameters.AddWithValue("@senha", senhaUsuarioBD);
                        var resultado = comando.ExecuteNonQuery();
                        comando.Parameters.Clear();
                        testarResultadoComando(resultado, "Erro ao criar o usuário no banco de dados.");

                        // Criar tabela de contas
                        comando.CommandText = "CREATE TABLE IF NOT EXISTS contas (conta VARCHAR(15) PRIMARY KEY, descricao VARCHAR(100) NOT NULL, nivel CHAR(1) NOT NULL CHECK (nivel IN ('S', 'A')));";
                        comando.ExecuteNonQuery();

                        // INSERIR CONTA 0 (CAIXA)
                        comando.CommandText = "INSERT INTO contas (conta, descricao, nivel) VALUES ('0', 'Valores no caixa', 'A');";
                        resultado = comando.ExecuteNonQuery();
                        testarResultadoComando(resultado, "Erro ao criar a conta 0 (referênte ao caixa)");

                        // Criar tabela de históricos
                        comando.CommandText = "CREATE TABLE IF NOT EXISTS historicos (id INTEGER PRIMARY KEY AUTOINCREMENT, historico VARCHAR(100) NOT NULL UNIQUE);";
                        resultado = comando.ExecuteNonQuery();
                        testarResultadoComando(resultado, "Erro ao criar a tabela de históricos.");

                        // Criar tabela de lançamentos
                        comando.CommandText = "CREATE TABLE IF NOT EXISTS lancamentos (id INTEGER PRIMARY KEY AUTOINCREMENT, conta VARCHAR(15) NOT NULL, valor NUMERIC(8,2) NOT NULL, data DATE DEFAULT (DATE('now')), id_historico INTEGER NOT NULL, saldo NUMERIC(8,2) NOT NULL, FOREIGN KEY (conta) REFERENCES contas(conta) ON UPDATE CASCADE ON DELETE RESTRICT, FOREIGN KEY (id_historico) REFERENCES historicos(id) ON UPDATE CASCADE ON DELETE RESTRICT);";
                        resultado = comando.ExecuteNonQuery();
                        testarResultadoComando(resultado, "Erro ao criar a tabela de lançamentos.");

                        // Criar tabela para registro dos saldos do caixa
                        comando.CommandText = "CREATE TABLE registros_caixa (data DATE PRIMARY KEY NOT NULL UNIQUE, saldo NUMERIC (8, 2) NOT NULL);";
                        resultado = comando.ExecuteNonQuery();
                        testarResultadoComando(resultado, "Erro ao criar a tabela para registros do caixa");

                        // Carregar tabela com o novo banco e desconectar
                        carregarBDs();

                        cbbBD.Text = nomeBD.Replace(".sqlite", "");

                        MessageBox.Show("O banco de dados foi criado.", "Operação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        resetarForm();
                        txtNome.Focus();
                    }
                }    
            }
            catch (Exception error)
            {
                MessageBox.Show($"Houve um erro ao criar o banco de dados, anote a mensagem de erro: \n\n{error.Message?.ToString()}", "Não foi possível criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Error);

                // Se der algum erro mas o arquivo de banco já foi criado: excluir
                if (File.Exists(caminhoBD))
                {
                    try
                    {
                        File.Delete(caminhoBD);
                    }
                    catch (Exception erro)
                    {
                        MessageBox.Show($"Não foi possível excluir o banco de dados, por favor, remova ele manualmente na pasta databases (dentro da instalação do programa) e anote o código de erro: \n\n{erro.Message.ToString()}", "Erro ao excluir o banco de dados com defeito", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                resetarForm(true);
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
                    MessageBox.Show($"O Banco de dados {nomeBD.Replace(".sqlite", "")} foi excluído.", "Operação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message.ToString(), "Erro ao excluir o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"Não foi possivel localizar o banco de dados ou ele não existe!", "Erro ao excluir o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        public static bool verificarSenha(string senha)
        {
            if (string.IsNullOrWhiteSpace(senha) || senha.Contains(" "))
            {
                MessageBox.Show("A senha não pode ser vázia ou conter espaços!", "Erro ao registrar senha", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (senha.Length > 30)
            {
                MessageBox.Show("A senha não pode ter mais que 30 caracteres!", "Erro ao registrar senha", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("O nome de usuário não pode ser vázio ou conter espaços!", "Erro ao registrar usuário", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else if (usuario.Length > 20)
            {
                MessageBox.Show("O nome de usuário não pode ter mais que 20 caracteres!", "Erro ao registrar usuário", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            // Verifica se o nome de usuário contém apenas letras e números (segue o padrão)
            else if (!Regex.IsMatch(usuario, padrao))
            {
                MessageBox.Show("O nome de usuário deve ser composto apenas de letras e números!", "Erro ao registrar usuário", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

            // Verifica se está em modo de registro de senha
            if (btnCriarBD.Text == "Concluir")
            {
                // Verifica a senha e se for válida: cria o BD
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
            // Verifica se está em modo de registro de usuário
            else if (btnCriarBD.Text == "Salvar")
            {
                // Verifica se o usuário é válido
                if (verificarUsuario(cbbBD.Text))
                {
                    usuarioBD = cbbBD.Text;
                    MessageBox.Show("Qual será a senha desse usuário?", "Criar banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Question);

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
            // Senão: verifica se é nulo
            else if (cbbBD.Text == "" || cbbBD.Text == null || string.IsNullOrWhiteSpace(cbbBD.Text))
            {
                MessageBox.Show("Não foi informado um nome para o banco!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Text = "";
                cbbBD.Focus();
            }
            // Verifica se começa com número
            else if (char.IsDigit(cbbBD.Text[0]))
            {
                MessageBox.Show("O nome informado não pode começar com um número!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Text = "";
                cbbBD.Focus();
            }
            // Verifica se a string é maior que 30 caracteres
            else if (cbbBD.Text.Length > 30)
            {
                MessageBox.Show("O nome do banco não deve conter mais que 30 caracteres!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Text = "";
                cbbBD.Focus();
            }
            // Verifica se utiliza apenas letras e números
            else if (!validarNomeBD(cbbBD.Text))
            {
                MessageBox.Show("O nome ínformado deve conter apenas letras e números (sem espaços)!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Text = "";
                cbbBD.Focus();
            }
            // Verifica se o arquivo já existe
            else if (File.Exists(caminhoBD))
            {
                MessageBox.Show("Já existe um banco de dados com o nome informado!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Text = "";
                cbbBD.Focus();
            }
            // Senão: entra em modo de registro de usuário
            else
            {
                nomeBD = validarExtensaoBD(cbbBD.Text);
                MessageBox.Show("Qual o nome do usuário a se registrar?", "Criar banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Question);

                btnExcluirBD.Enabled = false;
                btnRenomearBD.Enabled = false;
                btnBackup.Enabled = false;
                btnRestaurar.Enabled = false;
                gpbInfoUsuario.Enabled = false;
                btnCriarBD.Text = "Salvar";
                cbbBD.Text = "";
                cbbBD.Focus();
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
            // Salvar nome do banco para as operações futuras
            nomeBD = validarExtensaoBD(cbbBD.Text);

            // Verifica se é nulo
            if (cbbBD.Text == "" || cbbBD.Text == null || string.IsNullOrWhiteSpace(cbbBD.Text))
            {
                MessageBox.Show("Não foi informado um nome para o banco!", tituloMensagem, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Text = "";
                cbbBD.Focus();

                return false;
            }
            // Verifica se o arquivo não existe no comboBox
            else if (!nomesBDs.Contains(nomeBD.ToLower()))
            {
                MessageBox.Show("Não foi possivel localizar o banco de dados ou ele não existe!", tituloMensagem, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show("Por gentileza, anote o nome informado do banco, salve um print da tela de Login e contate o desenvolvedor do programa.", "Erro não tratado!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnExcluirBD_Click(object sender, EventArgs e)
        {
            // Exibe uma caixa de diálogo de confirmação
            DialogResult resultado = MessageBox.Show("Você deseja excluir o banco de dados? Esse processo é irreversível e todos os dados armazenados serão perdidos!", "Excluir banco de dados", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            // Verifica a resposta do usuário
            if (resultado == DialogResult.Yes)
            {
                resultado = MessageBox.Show("Você realmente DESEJA EXCLUIR o banco de dados? (dupla checagem)", "Excluir banco de dados", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                // Verifica a resposta do usuário
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
            }
        }

        private void renomearBD(string nomeBDNovo)
        {
            caminhoBD = $"{pastaDatabases}\\{nomeBD}";

            try
            {
                // Renomear
                File.Move(caminhoBD, $"{Path.GetDirectoryName(caminhoBD)}\\{nomeBDNovo}");

                // Verificar se o arquivo foi movido
                if (File.Exists($"{Path.GetDirectoryName(caminhoBD)}\\{nomeBDNovo}") && !File.Exists(caminhoBD))
                {
                    MessageBox.Show("O banco de dados foi renomeado com sucesso!", "Operação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    throw new Exception("Não foi possível renomear o arquivo, tente novamente com outro nome ou em outra pasta.");
                }

                // Resetar o formulário e carregar os bancos de dados
                resetarForm();
                carregarBDs();
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Erro de IO, anote a mensagem de erro: \n\n{ex.Message}", "Erro ao renomear o arquivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"{ex.Message}", "Erro ao renomear o arquivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnRenomearBD_Click(object sender, EventArgs e)
        {
            // Verificar se o botão está no modo de salvamento (quando o usuário já indicou o banco a ser renomeado)
            if (btnRenomearBD.Text == "Salvar")
            {
                string nomeBDNovo = validarExtensaoBD(cbbBD.Text);

                // Verifica se é nulo
                if (cbbBD.Text == "" || cbbBD.Text == null || string.IsNullOrWhiteSpace(cbbBD.Text))
                {
                    MessageBox.Show("Não foi informado um nome para o banco!", "Erro ao renomear banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbbBD.Text = "";
                    cbbBD.Focus();
                }
                // Verifica se o arquivo existe no comboBox
                else if (nomesBDs.Contains(nomeBDNovo.ToLower()))
                {
                    // Verificar se o arquivo encontrado é o mesmo que deseja-se renomear (se sim: pode-se alterar a capitalização das letras desde que não seja igual ao já existente)
                    if (nomeBDNovo.ToLower() == nomeBD.ToLower())
                    {
                        // Se o nome for exatamente igual: erro
                        if (nomeBDNovo == nomeBD)
                        {
                            MessageBox.Show("O nome informado é exatamente igual ao anterior!", "Erro ao renomear banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            cbbBD.Text = "";
                            cbbBD.Focus();
                        }
                        // Senão: renomear
                        else
                        {
                            renomearBD(nomeBDNovo);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Já existe um banco de dados com esse nome!", "Erro ao renomear banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cbbBD.Text = "";
                        cbbBD.Focus();
                    }
                }
                // Verifica se o arquivo não existe no comboBox
                else if (!nomesBDs.Contains(nomeBDNovo.ToLower()))
                {
                    // Verifica se começa com número
                    if (char.IsDigit(cbbBD.Text[0]))
                    {
                        MessageBox.Show("O nome informado não pode começar com um número!", "Erro ao renomear banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cbbBD.Text = "";
                        cbbBD.Focus();
                    }
                    // Verifica se utiliza apenas letras e números
                    else if (!validarNomeBD(cbbBD.Text))
                    {
                        MessageBox.Show("O nome ínformado deve conter apenas letras e números (sem espaços)!", "Erro ao renomear banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        cbbBD.Text = "";
                        cbbBD.Focus();
                    }
                    // Passou nas verificações: renomear
                    else
                    {
                        renomearBD(nomeBDNovo);
                    }
                }
                else
                {
                    MessageBox.Show("Por gentileza, anote o nome informado do banco, salve um print da tela de Login e contate o desenvolvedor do programa.", "Erro não tratado!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            // Verificar se o banco existe e se existir: travar demais botões, exibir mensagem e aguardar o usuário inserir um novo nome válido.
            else if (verificarExistenciaBD("Erro ao localizar banco de dados"))
            {
                MessageBox.Show("Banco de dados selecionado. Insira um novo nome e confirme ou tecle ESC para cancelar", "Renomear arquivo", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Entrar em modo de salvamento
                btnCriarBD.Enabled = false;
                btnExcluirBD.Enabled = false;
                btnBackup.Enabled = false;
                btnRestaurar.Enabled = false;
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
                MessageBox.Show("Usuário não informado!", "Formulário Incompleto", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNome.Focus();

            }
            // Verifica se o usuário foi informado
            else if (txtSenha.Text == "")
            {
                MessageBox.Show("Senha não informada!", "Formulário Incompleto", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSenha.Focus();
            }
            else
            {
                try
                {
                    // Conectar ao banco
                    caminhoBD = $"{pastaDatabases}\\{nomeBD}";
                    Conexao con = new Conexao(caminhoBD);
                    con.Conectar();

                    // Query de pesquisa
                    string sql = "SELECT * FROM usuarios WHERE nome= @nome AND senha= @senha;";
                    var comando = new SQLiteCommand(sql, con.conn);
                    // Parâmetros
                    comando.Parameters.AddWithValue("@nome", txtNome.Text);
                    comando.Parameters.AddWithValue("@senha", txtSenha.Text);

                    // Consultar usuários com os parâmetros informados
                    DataTable dtUsuario = new DataTable();

                    using (var reader = comando.ExecuteReader())
                    {
                        dtUsuario.Load(reader);
                    }

                    // Liberar recursos;
                    comando.Dispose();

                    if (dtUsuario.Rows.Count <= 0)
                    {
                        MessageBox.Show("Usuário ou senha inválido(s)!", "Registro não encontrado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        txtSenha.Clear();
                        txtSenha.Focus();
                    }
                    else
                    {
                        string usuario = dtUsuario.Rows[0]["nome"].ToString();

                        MessageBox.Show($"Bem Vindo(a) {usuario}.", "Login", MessageBoxButtons.OK, MessageBoxIcon.None);

                        this.Visible = false;
                        frmPainelPrincipal frmPainelPrincipal = new frmPainelPrincipal(nomeBD.Replace(".sqlite", ""), usuario, con); // Crie uma instância do frmPainelPrincipal
                        frmPainelPrincipal.Owner = this; // Definir o painel de Login como pai
                        frmPainelPrincipal.ShowDialog(); // Exibe o form como uma janela de diálogo modal

                        // Após fechar o Painel Principal
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
                    MessageBox.Show($"Não foi possível fazer login, anote a mensagem de erro: \n\n{error.Message.ToString()}", "Erro ao fazer login", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            btnBackup.Enabled = true;
            btnRestaurar.Enabled = true;
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

        public static bool verificarExistenciaBancosSqlite(string caminhoDiretorio)
        {
            if (Directory.Exists(caminhoDiretorio))
            {
                // Obtém todos os arquivos .sqlite no diretório
                string[] sqliteFiles = Directory.GetFiles(caminhoDiretorio, "*.sqlite");

                // Verifica se há algum arquivo .sqlite
                return sqliteFiles.Length > 0;
            }
            else
            {
                throw new CustomException("O diretório especificado não existe.");
            }
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            // Pergunta inicial ao usuário sobre o tipo de backup
            DialogResult result = MessageBox.Show("Deseja fazer backup apenas do banco de dados selecionado?", "Backup", MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                var nomeBanco = cbbBD.Text;

                // Verifica se é nulo
                if (cbbBD.Text == "" || cbbBD.Text == null || string.IsNullOrWhiteSpace(cbbBD.Text))
                {
                    MessageBox.Show("Não foi informado um nome para o banco!", "Erro ao fazer backup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    MessageBox.Show("Não foi encontrado o arquivo do banco de dados selecionado", "Erro ao fazer backup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbbBD.Text = "";
                    cbbBD.Focus();
                }

            }
            else if (result == DialogResult.No)
            {
                if (verificarExistenciaBancosSqlite(pastaDatabases))
                {
                    frmPainelPrincipal.FazerBackupTodosBancos(pastaDatabases);
                }
                else
                {
                    MessageBox.Show("Não foi encontrado nenhum arquivo de banco de dados na pasta do programa, verifique se realmente existe algum antes de realizar o backup.", "Não é possível fazer o backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            // Se result for DialogResult.Cancel, não faz nada (cancela a operação)
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            // Pergunta inicial ao usuário
            DialogResult respostaInicial = MessageBox.Show("Deseja restaurar apenas um banco de dados?",
                                                           "Restaurar banco de dados",
                                                           MessageBoxButtons.YesNoCancel,
                                                           MessageBoxIcon.Question);

            if (respostaInicial == DialogResult.Yes)
            {
                // Selecionar arquivo .sqlite para restauração individual
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "Arquivos SQLite (*.sqlite)|*.sqlite";
                openFileDialog.Title = "Selecionar Arquivo SQLite para Backup";

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string arquivoSelecionado = openFileDialog.FileName;

                    string nomeArquivo = Path.GetFileNameWithoutExtension(arquivoSelecionado);
                    string destino = Path.Combine(pastaDatabases, Path.GetFileName(arquivoSelecionado));

                    // Verificar se o arquivo não está nas dependências do programa
                    if (frmPainelPrincipal.arquivoEstaNoDiretorio(Application.StartupPath, arquivoSelecionado))
                    {
                        MessageBox.Show("Não é possível restaurar um arquivo que está nas pastas do programa, tente novamente e selecione outro diretório.", "Erro ao realizar backup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Verificar se o arquivo já existe na pastaDatabases
                    if (File.Exists(destino))
                    {
                        // Arquivo existe, perguntar se deseja sobrescrever
                        DialogResult respostaSobrescrever = MessageBox.Show($"O arquivo \"{nomeArquivo}\" já existe. Deseja sobrescrevê-lo?",
                                                                              "Sobrescrever Arquivo",
                                                                              MessageBoxButtons.YesNo,
                                                                              MessageBoxIcon.Question);

                        if (respostaSobrescrever == DialogResult.Yes)
                        {
                            // Sobrescrever o arquivo
                            File.Copy(arquivoSelecionado, destino, true);
                            carregarBDs();
                            MessageBox.Show($"O arquivo do banco de dados \"{nomeArquivo}\" foi sobrescrito com sucesso.", "Restauração bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        // Se respostaSobrescrever for No, não faz nada (cancela a operação)
                    }
                    else
                    {
                        // Arquivo não existe, copiar para a pastaDatabases
                        File.Copy(arquivoSelecionado, destino);
                        carregarBDs();
                        MessageBox.Show($"O banco de dados \"{nomeArquivo}\" foi restaurado com sucesso.", "Restauração bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                // Se o usuário cancelar a seleção do arquivo, não faz nada
            }
            else if (respostaInicial == DialogResult.No)
            {
                // Selecionar pasta para backup de múltiplos arquivos .sqlite
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                folderBrowserDialog.Description = "Selecione a pasta que contém os arquivos SQLite para backup.";

                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    string pastaSelecionada = folderBrowserDialog.SelectedPath;
                    string[] arquivosSQLite = Directory.GetFiles(pastaSelecionada, "*.sqlite");

                    // Verificar se a pasta selecionada não está nas dependências do programa
                    if (frmPainelPrincipal.IsSubDiretorio(Application.StartupPath, pastaSelecionada))
                    {
                        MessageBox.Show("Não é possível restaurar a partir de arquivos que estão nas pastas do programa, tente novamente e selecione outro diretório.", "Erro ao realizar backup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (arquivosSQLite.Length == 0)
                    {
                        MessageBox.Show("Não foram encontrados arquivos de banco de dados na pasta selecionada.",
                                        "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return; // Cancela a função, pois não há arquivos para copiar
                    }

                    // Verificar se deseja sobrescrever todos os arquivos na pastaDatabases
                    DialogResult respostaSobrescreverTodos = MessageBox.Show("Deseja sobrescrever todos os arquivos que já existem na pasta do programa?",
                                                                              "Sobrescrever Todos",
                                                                              MessageBoxButtons.YesNoCancel,
                                                                              MessageBoxIcon.Question);

                    if (respostaSobrescreverTodos == DialogResult.Yes)
                    {
                        // Sobrescrever todos os arquivos
                        foreach (string arquivo in arquivosSQLite)
                        {
                            string destino = Path.Combine(pastaDatabases, Path.GetFileName(arquivo));
                            File.Copy(arquivo, destino, true);
                        }
                        carregarBDs();
                        MessageBox.Show("Todos os arquivos foram copiados para pastaDatabases com sucesso.",
                                        "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (respostaSobrescreverTodos == DialogResult.No)
                    {
                        // Perguntar ao usuário se deseja selecionar quais arquivos serão sobrescritos
                        DialogResult respostaSelecionarArquivos = MessageBox.Show("Deseja então selecionar quais arquivos serão sobrescritos?",
                                                                                   "Selecionar Arquivos",
                                                                                   MessageBoxButtons.YesNoCancel,
                                                                                   MessageBoxIcon.Question);

                        if (respostaSelecionarArquivos == DialogResult.Yes)
                        {
                            // Escolher quais arquivos sobrescrever
                            foreach (string arquivo in arquivosSQLite)
                            {
                                string nomeArquivo = Path.GetFileNameWithoutExtension(arquivo);
                                string destino = Path.Combine(pastaDatabases, Path.GetFileName(arquivo));

                                // Verificar se o arquivo já existe no destino
                                if (File.Exists(destino))
                                {
                                    // Perguntar ao usuário se deseja sobrescrever somente se o arquivo existir
                                    DialogResult respostaSobrescrever = MessageBox.Show($"O arquivo '{nomeArquivo}' já existe na pasta de destino. Deseja sobrescrevê-lo?",
                                                                                         "Sobrescrever Arquivo",
                                                                                         MessageBoxButtons.YesNo,
                                                                                         MessageBoxIcon.Question);

                                    if (respostaSobrescrever == DialogResult.Yes)
                                    {
                                        File.Copy(arquivo, destino, true);
                                    }
                                    // Se respostaSobrescrever for No, não faz nada (não sobrescreve o arquivo)
                                }
                                else
                                {
                                    // Arquivo não existe no destino, copiar diretamente
                                    File.Copy(arquivo, destino);
                                }
                            }
                            carregarBDs();
                            MessageBox.Show($"Todos os arquivos confirmados foram restaurados com sucesso.",
                                            "Restauração bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                                // Verificar se o arquivo já existe na pastaDatabases
                                if (File.Exists(destino))
                                {
                                    // Arquivo já existe, adicionar à lista de bancos não copiados
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
                                MessageBox.Show("Nenhum arquivo foi restaurado pois todos já existem na pasta do programa.",
                                                "Restauração concluída", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            else if (bancosNaoCopiados.Count == 0)
                            {
                                carregarBDs();
                                MessageBox.Show($"Todos os arquivos foram restaurados.",
                                                "Restauração bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else
                            {
                                carregarBDs();
                                MessageBox.Show($"Os seguintes arquivos não foram restaurados porque já existem na pasta do programa:\n\n{string.Join("\n", bancosNaoCopiados)}", "Arquivos não copiados");
                            }
                        }
                        // Se o usuário cancelar, não faz nada
                    }
                    // Se o usuário cancelar, não faz nada
                }
                // Se o usuário cancelar a seleção da pasta, não faz nada
            }
            // Se respostaInicial for Cancel, não faz nada
        }
    }
}
