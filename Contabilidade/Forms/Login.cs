using Contabilidade.Models;
using System.Data;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SQLite;
using Contabilidade.Classes;
using Contabilidade.Forms.Cadastros;
using Contabilidade.Forms;

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

            txtNome.Select();
        }

        private void carregarBDs()
        {
            string[] caminhosBDs = Directory.GetFiles(pastaDatabases, "*.sqlite");
            // Ordenar os itens de forma alfabética inversa
            Array.Sort(caminhosBDs, (x, y) => string.Compare(y, x));
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

                // Verifica se o ComboBox possui algum item
                if (cbbBD.Items.Count > 0)
                {
                    // Seleciona o primeiro item
                    cbbBD.SelectedIndex = 0;
                }
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

        public static bool verificarNomeBD(string input)
        {
            string padrao = @"^[a-zA-Z0-9-_]+$";

            if (Regex.IsMatch(input, padrao))
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
                        comando.CommandText = "CREATE TABLE IF NOT EXISTS historicos (id INTEGER PRIMARY KEY AUTOINCREMENT, historico VARCHAR(300) NOT NULL UNIQUE);";
                        resultado = comando.ExecuteNonQuery();
                        testarResultadoComando(resultado, "Erro ao criar a tabela de históricos.");

                        // Criar tabela de lançamentos
                        comando.CommandText = "CREATE TABLE IF NOT EXISTS lancamentos (id INTEGER PRIMARY KEY AUTOINCREMENT, conta VARCHAR(15) NOT NULL, valor INTEGER NOT NULL, data DATE DEFAULT (DATE('now')), id_historico INTEGER NOT NULL, saldo INTEGER NOT NULL, FOREIGN KEY (conta) REFERENCES contas(conta) ON UPDATE CASCADE ON DELETE RESTRICT, FOREIGN KEY (id_historico) REFERENCES historicos(id) ON UPDATE CASCADE ON DELETE RESTRICT);";
                        resultado = comando.ExecuteNonQuery();
                        testarResultadoComando(resultado, "Erro ao criar a tabela de lançamentos.");

                        // Criar tabela para registro dos saldos do caixa
                        comando.CommandText = "CREATE TABLE registros_caixa (data DATE PRIMARY KEY NOT NULL UNIQUE, saldo INTEGER NOT NULL);";
                        resultado = comando.ExecuteNonQuery();
                        testarResultadoComando(resultado, "Erro ao criar a tabela para registros do caixa");

                        // Carregar tabela com o novo banco e desconectar
                        carregarBDs();

                        cbbBD.Text = nomeBD.Replace(".sqlite", "");

                        MessageBox.Show("O banco de dados foi criado.", "Operação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        resetarForm();
                        txtNome.Select();
                    }
                }
            }
            catch (CustomException error)
            {
                MessageBox.Show($"{error.Message?.ToString()}", "Não foi possível criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Error);

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
                cbbBD.Select();
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
                cbbBD.Select();
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
                MessageBox.Show("O nome de usuário deve ser composto apenas de letras e números (sem espaços ou acentos)!", "Erro ao registrar usuário", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }
            else
            {
                return true;
            }
        }

        private void btnCriarBD_Click(object sender, EventArgs e)
        {
            // Criar uma instância do formulário de dados e aguarda o retorno OK
            using (var frmDados = new frmCriarBD(pastaDatabases))
            {
                // O usuário apertou o botão de salvar
                if (frmDados.ShowDialog() == DialogResult.OK)
                {
                    // Obter dados do formulário filho
                    (nomeBD, usuarioBD, senhaUsuarioBD) = (frmDados.nomeBD, frmDados.usuario, frmDados.senha);

                    criarBD();
                }
            }
        }

        public static string validarExtensaoBD(string nomeBanco)
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
                cbbBD.Select();

                return false;
            }
            // Verifica se o arquivo não existe no comboBox
            else if (!nomesBDs.Contains(nomeBD.ToLower()))
            {
                MessageBox.Show("Não foi possivel localizar o banco de dados ou ele não existe!", tituloMensagem, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbBD.Text = "";
                cbbBD.Select();

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
                cbbBD.Select();

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
                        cbbBD.Select();
                    }
                }
            }
        }

        private void renomearBD(string nomeBDNovo)
        {
            caminhoBD = $"{pastaDatabases}\\{nomeBD}";
            var caminhoNovoBD = $"{Path.GetDirectoryName(caminhoBD)}\\{nomeBDNovo}";

            try
            {
                // Renomear
                File.Move(caminhoBD, caminhoNovoBD);

                // Verificar se o arquivo foi movido
                if (File.Exists(caminhoNovoBD))
                {
                    // Escrever o nome do banco (menos os últimos 7 dígitos relativos a extensão .sqlite) no cbbBD
                    cbbBD.Text = "";
                    cbbBD.Text = nomeBDNovo[..^7];

                    MessageBox.Show("O banco de dados foi renomeado com sucesso!", "Operação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    throw new Exception("Não foi possível renomear o arquivo, tente novamente com outro nome.");
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
            // Verificar se o banco existe
            if (verificarExistenciaBD("Erro ao localizar banco de dados"))
            {
                // Criar uma instância do formulário de dados e aguarda o retorno OK
                using (var frmDados = new frmRenomearBD(pastaDatabases, cbbBD.Text))
                {
                    // O usuário apertou o botão de salvar
                    if (frmDados.ShowDialog() == DialogResult.OK)
                    {
                        // Obter dados do formulário filho e passar para a função de renomear
                        renomearBD(frmDados.nomeBD);
                    }
                }
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
                txtNome.Select();

            }
            // Verifica se o usuário foi informado
            else if (txtSenha.Text == "")
            {
                MessageBox.Show("Senha não informada!", "Formulário Incompleto", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtSenha.Select();
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
                        txtSenha.Select();
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
                        cbbBD.Select();
                        this.Visible = true;
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show($"Não foi possível fazer login, anote a mensagem de erro: \n\n{error.Message.ToString()}", "Erro ao fazer login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void resetarForm(bool completo = false)
        {
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

        private void txtNome_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtSenha.Select();
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                txtNome.Text = "";
                txtSenha.Text = "";
                txtNome.Select();
            }
            else
            {
                ImpedirPressionarBarraEspaco(e);
            }
        }

        private void txtSenha_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnEntrar.PerformClick();
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                txtNome.Text = "";
                txtSenha.Text = "";
                txtNome.Select();
            }
            else
            {
                ImpedirPressionarBarraEspaco(e);
            }
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
                    cbbBD.Select();
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
                    cbbBD.Select();
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

        private void btnTransferir_Click(object sender, EventArgs e)
        {
            // Verificar se o banco existe
            if (verificarExistenciaBD("Erro ao localizar banco de dados"))
            {
                // Criar uma instância do formulário de dados e aguarda o retorno OK
                using (var frmDados = new frmTransferirBD(pastaDatabases, cbbBD.Text))
                {
                    // O usuário apertou o botão de transferir
                    if (frmDados.ShowDialog() == DialogResult.OK)
                    {
                        // Obter dados do formulário filho e passar para a função de transferir
                        transferirBD(frmDados.nomeBD);
                    }
                }
            }
            else
            {
                nomeBD = "";
            }
        }

        private void testarExistenciaTabelaBD(bool resultado, bool resultadoSucesso, string mensagem)
        {
            if (resultado != resultadoSucesso)
            {
                throw new CustomException(mensagem);
            }
        }

        private bool verificarExistenciaTabelaBD(SQLiteCommand comando, string tabelaBD)
        {
            comando.CommandText = $"SELECT name FROM sqlite_master WHERE type='table' AND name='{tabelaBD}';";
            comando.Parameters.AddWithValue("@tabela", tabelaBD);
            var resultado = comando.ExecuteScalar();

            comando.Parameters.Clear();

            if (resultado == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private void transferirBD(string nomeBDNovo)
        {
            caminhoBD = $"{pastaDatabases}\\{nomeBD}";
            var caminhoNovoBD = $"{Path.GetDirectoryName(caminhoBD)}\\{nomeBDNovo}";

            try
            {
                // Copiar o banco com o novo nome
                File.Copy(caminhoBD, caminhoNovoBD);

                // Verificar se o arquivo foi movido
                if (File.Exists(caminhoNovoBD))
                {
                    try
                    {
                        // Abrir conexão com o novo banco
                        using (var conexaoBanco = new SQLiteConnection("Data Source=" + caminhoNovoBD))
                        {
                            conexaoBanco.Open();

                            using (var comando = new SQLiteCommand("", conexaoBanco))
                            {
                                // Excluir a tabela antiga de lançamentos
                                comando.CommandText = "DROP TABLE IF EXISTS lancamentos;";
                                comando.ExecuteNonQuery();
                                // Comandos para verificar se a tabela ainda existe
                                var resultado = verificarExistenciaTabelaBD(comando, "lancamentos");
                                testarExistenciaTabelaBD(resultado, false, "Erro ao excluir a tabela antiga de lançamentos.");

                                // Excluir a tabela antiga de registros do caixa
                                comando.CommandText = "DROP TABLE IF EXISTS registros_caixa;";
                                comando.ExecuteNonQuery();
                                // Comandos para verificar se a tabela ainda existe
                                resultado = verificarExistenciaTabelaBD(comando, "registros_caixa");
                                testarExistenciaTabelaBD(resultado, false, "Erro ao excluir a tabela antiga de registros do caixa.");

                                // Criar novamente as tabelas excluídas
                                // Criar tabela de lançamentos
                                comando.CommandText = "CREATE TABLE IF NOT EXISTS lancamentos (id INTEGER PRIMARY KEY AUTOINCREMENT, conta VARCHAR(15) NOT NULL, valor INTEGER NOT NULL, data DATE DEFAULT (DATE('now')), id_historico INTEGER NOT NULL, saldo INTEGER NOT NULL, FOREIGN KEY (conta) REFERENCES contas(conta) ON UPDATE CASCADE ON DELETE RESTRICT, FOREIGN KEY (id_historico) REFERENCES historicos(id) ON UPDATE CASCADE ON DELETE RESTRICT);";
                                comando.ExecuteNonQuery();
                                resultado = verificarExistenciaTabelaBD(comando, "lancamentos");
                                testarExistenciaTabelaBD(resultado, true, "Erro ao recriar a tabela de lançamentos.");

                                // Criar tabela para registro dos saldos do caixa
                                comando.CommandText = "CREATE TABLE registros_caixa (data DATE PRIMARY KEY NOT NULL UNIQUE, saldo INTEGER NOT NULL);";
                                comando.ExecuteNonQuery();
                                resultado = verificarExistenciaTabelaBD(comando, "registros_caixa");
                                testarExistenciaTabelaBD(resultado, true, "Erro ao recriar a tabela para registros do caixa");

                                // Carregar tabela com o novo banco e desconectar
                                carregarBDs();

                                cbbBD.Text = nomeBDNovo.Replace(".sqlite", "");

                                MessageBox.Show("O banco de dados foi transferido com sucesso!.", "Operação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                resetarForm();
                                txtNome.Select();
                            }
                        }
                    }
                    catch (CustomException error)
                    {
                        MessageBox.Show($"{error.Message?.ToString()}", "Não foi possível transferir o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        // Se der algum erro mas o arquivo de banco já foi criado: excluir
                        if (File.Exists(caminhoNovoBD))
                        {
                            try
                            {
                                File.Delete(caminhoNovoBD);
                            }
                            catch (Exception erro)
                            {
                                MessageBox.Show($"Não foi possível excluir o banco de dados com erro, por favor, remova ele manualmente na pasta databases (dentro da instalação do programa) e anote o código de erro: \n\n{erro.Message.ToString()}", "Erro ao excluir o banco de dados com defeito", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        resetarForm(true);
                        cbbBD.Select();
                    }
                    catch (Exception error)
                    {
                        MessageBox.Show($"Houve um erro ao transferir o banco de dados, anote a mensagem de erro: \n\n{error.Message?.ToString()}", "Não foi possível transferir o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        // Se der algum erro mas o arquivo de banco já foi criado: excluir
                        if (File.Exists(caminhoNovoBD))
                        {
                            try
                            {
                                File.Delete(caminhoNovoBD);
                            }
                            catch (Exception erro)
                            {
                                MessageBox.Show($"Não foi possível excluir o banco de dados com erro, por favor, remova ele manualmente na pasta databases (dentro da instalação do programa) e anote o código de erro: \n\n{erro.Message.ToString()}", "Erro ao excluir o banco de dados com defeito", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }

                        resetarForm(true);
                        cbbBD.Select();
                    }
                }
                else
                {
                    throw new Exception("Não foi possível transferir o arquivo, tente novamente com outro nome.");
                }
            }
            catch (IOException ex)
            {
                // Resetar o formulário e carregar os bancos de dados
                resetarForm();
                carregarBDs();

                MessageBox.Show($"Erro de IO, anote a mensagem de erro: \n\n{ex.Message}", "Erro ao transferir o arquivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Resetar o formulário e carregar os bancos de dados
                resetarForm();
                carregarBDs();

                MessageBox.Show($"{ex.Message}", "Erro ao transferir o arquivo", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cbbBD_KeyPress(object sender, KeyPressEventArgs e)
        {
            ImpedirPressionarBarraEspaco(e);
        }

        public static void ImpedirPressionarBarraEspaco(KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Space)
            {
                e.Handled = true;
            }
        }
    }
}
