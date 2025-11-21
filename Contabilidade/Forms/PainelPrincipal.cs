using Contabilidade.Forms.Lancamentos;
using Contabilidade.Models;
using System.Runtime.InteropServices;

namespace Contabilidade
{
    public partial class frmPainelPrincipal : Form
    {
        private Button botaoAtual;
        private Form formularioAtivo;
        Conexao con;
        public static string usuarioAtual = "";

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        public frmPainelPrincipal(string nomeBD, string usuario, Conexao conexaoBanco)
        {
            InitializeComponent();

            // Salvar informações da conexão
            con = conexaoBanco;
            usuarioAtual = usuario;
            lblUsuario.Text = usuarioAtual;
            lblBanco.Text = nomeBD;

            btnFecharFormFilho.Visible = false;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;

            timerRelogio.Start();

            TemaCores.Selecionar("padrão");
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            DialogResult input = MessageBox.Show("Deseja realmente fechar o programa?", "Você está prestes a sair", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (input == DialogResult.Yes)
            {
                con.Desconectar();
                this.Dispose();
                Application.Exit();
            }
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void selecionarBotao(object btnSender, bool exibirBotao = true)
        {
            if (btnSender != null)
            {
                if (botaoAtual != (Button)btnSender)
                {
                    List<Panel> listaPaineisMenu = [
                        pnlMenuLateral,
                        pnlCadastros,
                        pnlRelatorios,
                        pnlBackup,
                    ];

                    List<Button> listaBotoesMenu = [
                        btnCadastro,
                        btnLancamentos,
                        btnRelatorios,
                        btnSistema,
                        btnLogoff,
                    ];

                    List<Button> listaBotoesSubMenu = [
                        btnContas,
                        btnHistoricos,
                        btnUsuarios,
                        btnRelDiario,
                        btnRelAnalitico,
                        btnBalanceteGeral,
                        btnBalanceteConta,
                        btnSaldo,
                        btnBackup,
                        btnRestaurar,
                        btnRecalcular,
                    ];

                    // Testar se um botão já foi pressionado anteriormente
                    if (botaoAtual != null)
                    {
                        // Botão anterior: Voltar fonte ao padrão
                        botaoAtual.Font = new Font("Lucida Sans", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
                        botaoAtual.ForeColor = Color.Gainsboro;
                    }

                    // Atualizar botão selecionado
                    botaoAtual = (Button)btnSender;

                    // Aplicar cores do tema
                    // Botões de menu
                    foreach (Button botao in listaBotoesMenu)
                    {
                        botao.BackColor = TemaCores.CorBotaoMenu;
                    }
                    // Botões dropdown
                    foreach (Button botao in listaBotoesSubMenu)
                    {
                        botao.BackColor = TemaCores.CorBotaoSubMenu;
                    }
                    // Painel título
                    pnlTitulo.BackColor = TemaCores.CorBotaoSelecionado;
                    // Painel logo
                    pnlLogo.BackColor = TemaCores.CorPainel;
                    // Painéis do menu lateral
                    foreach (Panel painel in listaPaineisMenu)
                    {
                        painel.BackColor = TemaCores.CorPainel;
                    }
                    // Botão selecionado
                    botaoAtual.BackColor = TemaCores.CorBotaoSelecionado;
                    botaoAtual.ForeColor = Color.White;
                    botaoAtual.Font = new Font("Lucida Sans", 11.25F, FontStyle.Regular, GraphicsUnit.Point, 0);

                    if (exibirBotao)
                    {
                        btnFecharFormFilho.Visible = true;
                    }
                }
            }
        }

        private void desselecionarBotao()
        {
            List<Panel> listaPaineis = [
                pnlMenuLateral,
                pnlCadastros,
                pnlRelatorios,
                pnlBackup,
            ];

            List<Button> listaBotoesMenu = [
                btnCadastro,
                btnLancamentos,
                btnRelatorios,
                btnSistema,
                btnLogoff,
            ];

            List<Button> listaBotoesSubMenu = [
                btnContas,
                btnHistoricos,
                btnUsuarios,
                btnRelDiario,
                btnRelAnalitico,
                btnBalanceteGeral,
                btnBalanceteConta,
                btnSaldo,
                btnBackup,
                btnRestaurar,
                btnRecalcular,
            ];

            // Botão anterior: Voltar fonte ao padrão
            botaoAtual.Font = new Font("Lucida Sans", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            botaoAtual.ForeColor = Color.Gainsboro;

            // Desselecionar botaoAtual
            botaoAtual = null;

            // Selecionar tema
            TemaCores.Selecionar("padrão");

            // Aplicar cores do tema
            // Botões de menu
            foreach (Button botao in listaBotoesMenu)
            {
                botao.BackColor = TemaCores.CorBotaoMenu;
            }
            // Botões dropdown
            foreach (Button botao in listaBotoesSubMenu)
            {
                botao.BackColor = TemaCores.CorBotaoSubMenu;
            }
            // Painel título
            pnlTitulo.BackColor = Color.FromArgb(0, 150, 136);
            // Painel logo
            pnlLogo.BackColor = TemaCores.CorPainel;
            // Painéis restantes
            foreach (Panel painel in listaPaineis)
            {
                painel.BackColor = TemaCores.CorPainel;
            }
        }

        private void abrirFormulario(Form formularioFilho, object btnSender, string tema)
        {
            // Se haver um formulário ativo => fechar
            if (formularioAtivo != null)
            {
                formularioAtivo.Dispose();
            }

            // Selecionar tema e botão
            TemaCores.Selecionar(tema);
            selecionarBotao(btnSender);

            // Abrir novo formulário
            formularioAtivo = formularioFilho;
            formularioFilho.Owner = this;

            formularioFilho.TopLevel = false;
            formularioFilho.FormBorderStyle = FormBorderStyle.None;
            formularioFilho.Dock = DockStyle.Fill;

            this.pnlDesktop.Controls.Add(formularioFilho);
            this.pnlDesktop.Tag = formularioFilho;

            formularioFilho.BringToFront();
            formularioFilho.Show();

            lblTitulo.Text = formularioFilho.Text;
        }

        public void Reset()
        {
            desselecionarBotao();
            esconderSubMenus();

            lblTitulo.Text = "Painel Principal";

            botaoAtual = null;
            btnFecharFormFilho.Visible = false;
        }
        private void esconderSubMenus()
        {
            pnlCadastros.Visible = false;
            pnlRelatorios.Visible = false;
            pnlBackup.Visible = false;
        }

        private void exibirSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                esconderSubMenus();
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;
            }
        }

        private void pnlTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void lblTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void btnFecharFormFilho_Click(object sender, EventArgs e)
        {
            if (formularioAtivo != null)
            {
                if (formularioAtivo.Text == "Usuários")
                {
                    lblUsuario.Text = usuarioAtual;
                }
                formularioAtivo.Dispose();
                Reset();
            }
        }

        private void btnCadastro_Click(object sender, EventArgs e)
        {
            exibirSubMenu(pnlCadastros);
        }

        private void btnContas_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.Cadastros.frmContas(con), sender, "cadastros");
        }

        private void btnHistoricos_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.Cadastros.frmHistoricos(con), sender, "cadastros");
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.Cadastros.frmUsuarios(lblUsuario.Text, con), sender, "cadastros");
        }

        private void btnLancamentos_Click(object sender, EventArgs e)
        {
            esconderSubMenus();
            abrirFormulario(new Forms.Lancamentos.frmLancamentos(con), sender, "lançamentos");
        }

        private void btnRelatorios_Click(object sender, EventArgs e)
        {
            exibirSubMenu(pnlRelatorios);
        }

        private void btnRelDiario_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.Relatorios.frmRelDiario(con), sender, "relatórios");
        }

        private void btnRelAnalitico_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.Relatorios.frmRazaoAnalitico(con), sender, "relatórios");
        }

        private void btnBalanceteGeral_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.Relatorios.frmBalanceteGeral(con), sender, "relatórios");
        }

        private void btnBalanceteConta_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.Relatorios.frmBalanceteConta(con), sender, "relatórios");
        }

        private void btnSaldo_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.Relatorios.frmSaldo(con), sender, "relatórios");
        }

        private void btnLogoff_Click(object sender, EventArgs e)
        {
            DialogResult input = MessageBox.Show("Deseja realmente voltar à tela de login?", "Você está prestes a sair", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (input == DialogResult.Yes)
            {
                con.Desconectar();

                this.Owner.Show(); // Exibe o Formulário de Login
                this.Dispose(); // Fecha o formulário atual
            }
            else
            {
                btnFecharFormFilho.PerformClick();
            }
        }

        private void timerRelogio_Tick(object sender, EventArgs e)
        {
            lblRelogio.Text = System.DateTime.Now.ToString("HH:mm:ss");
            lblData.Text = System.DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void btnSistema_Click(object sender, EventArgs e)
        {
            exibirSubMenu(pnlBackup);
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            // Seleciona o botão sem mudar o tema, pois pode haver algum form já aberto
            selecionarBotao(sender, false);

            string pastaBDs = Path.Combine(Application.StartupPath, "databases");
            string nomeBD = lblBanco.Text;

            // Pergunta inicial ao usuário sobre o tipo de backup
            DialogResult result = MessageBox.Show("Deseja fazer backup apenas do banco de dados atual?", "Backup", MessageBoxButtons.YesNoCancel);

            if (result == DialogResult.Yes)
            {
                FazerBackupBancoAtual(pastaBDs, nomeBD);
            }
            else if (result == DialogResult.No)
            {
                if (frmLogin.verificarExistenciaBancosSqlite(pastaBDs))
                {
                    FazerBackupTodosBancos(pastaBDs);
                }
            }
            // Se result for DialogResult.Cancel, não faz nada (cancela a operação)
        }

        public static bool IsSubDiretorio(string diretorioPai, string subdiretorio)
        {
            // Obtém os caminhos absolutos
            var diretorioPaiInfo = new DirectoryInfo(diretorioPai);
            var subdiretorioInfo = new DirectoryInfo(subdiretorio);

            // Verifica se o caminho absoluto do subDiretório começa com o caminho absoluto do parentDir
            return subdiretorioInfo.FullName.StartsWith(diretorioPaiInfo.FullName, StringComparison.OrdinalIgnoreCase);
        }

        public static void FazerBackupBancoAtual(string pastaBDs, string nomeBD)
        {
            // Seleção da pasta de destino
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Selecione a pasta de destino para o backup do banco de dados.";
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string destino = dialog.SelectedPath;
                    string nomeArquivo = $"{nomeBD}.sqlite";

                    // Verificar se a pasta selecionada não está nas dependências do programa
                    if (IsSubDiretorio(Application.StartupPath, destino))
                    {
                        MessageBox.Show("Não é possível fazer backup para as pastas do programa, tente novamente e selecione outro diretório.", "Erro ao realizar backup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string caminhoCompleto = Path.Combine(destino, nomeArquivo);

                    // Verifica se o arquivo já existe no destino
                    if (File.Exists(caminhoCompleto))
                    {
                        // Pergunta ao usuário se deseja sobrescrever o arquivo existente
                        DialogResult overwriteResult = MessageBox.Show($"Já existe um arquivo \"{nomeArquivo}\" na pasta selecionada. Deseja sobrescrevê-lo?", "Sobrescrever arquivo", MessageBoxButtons.YesNo);

                        if (overwriteResult == DialogResult.Yes)
                        {
                            File.Copy($"{pastaBDs}\\{nomeArquivo}", caminhoCompleto, true);
                            MessageBox.Show("Arquivo sobrescrito com sucesso!", "Sucesso");
                        }
                        // Se o usuário escolher não sobrescrever, não faz nada
                    }
                    else
                    {
                        File.Copy($"{pastaBDs}\\{nomeArquivo}", caminhoCompleto, false);
                        MessageBox.Show("Arquivo de backup criado com sucesso!", "Sucesso");
                    }
                }
            }
        }

        public static void FazerBackupTodosBancos(string pastaBDs)
        {
            // Seleção da pasta de destino
            using (var dialog = new FolderBrowserDialog())
            {
                dialog.Description = "Selecione a pasta de destino para o backup de todos os bancos de dados.";
                DialogResult result = dialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    string destino = dialog.SelectedPath;

                    // Verificar se a pasta selecionada não está nas dependências do programa
                    if (IsSubDiretorio(Application.StartupPath, destino))
                    {
                        MessageBox.Show("Não é possível fazer backup para as pastas do programa, tente novamente e selecione outro diretório.", "Erro ao realizar backup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Verificar se há arquivos na pasta de origem (pastaBDs)
                    if (Directory.Exists(pastaBDs))
                    {
                        // Filtrar apenas arquivos .sqlite na pasta de origem
                        string[] arquivosOrigem = Directory.GetFiles(pastaBDs, "*.sqlite");

                        // Variável para controlar se houve sobrescrita de arquivos
                        bool fezBackup = false;

                        // Perguntar ao usuário se deseja sobrescrever todos os arquivos de destino
                        DialogResult overwriteAllResult = MessageBox.Show("Deseja sobrescrever todos os arquivos de destino?", "Sobrescrever todos", MessageBoxButtons.YesNoCancel);

                        if (overwriteAllResult == DialogResult.Yes)
                        {
                            // Sobrescrever todos os arquivos sem perguntar
                            foreach (string arquivoOrigem in arquivosOrigem)
                            {
                                string nomeArquivo = Path.GetFileName(arquivoOrigem);
                                string caminhoCompletoDestino = Path.Combine(destino, nomeArquivo);

                                if (File.Exists(caminhoCompletoDestino))
                                {
                                    File.Copy(arquivoOrigem, caminhoCompletoDestino, true);
                                    fezBackup = true;
                                }
                                else
                                {
                                    File.Copy(arquivoOrigem, caminhoCompletoDestino, false);
                                    fezBackup = true;
                                }
                            }
                        }
                        else if (overwriteAllResult == DialogResult.No)
                        {
                            // Perguntar ao usuário se deseja escolher individualmente
                            DialogResult chooseIndividualResult = MessageBox.Show("Deseja escolher individualmente quais arquivos sobrescrever?", "Escolher individualmente", MessageBoxButtons.YesNo);

                            if (chooseIndividualResult == DialogResult.Yes)
                            {
                                // Perguntar ao usuário para cada arquivo existente no destino
                                foreach (string arquivoOrigem in arquivosOrigem)
                                {
                                    string nomeArquivo = Path.GetFileName(arquivoOrigem);
                                    string caminhoCompletoDestino = Path.Combine(destino, nomeArquivo);

                                    if (File.Exists(caminhoCompletoDestino))
                                    {
                                        DialogResult overwriteResult = MessageBox.Show($"Já existe um arquivo \"{nomeArquivo}\" na pasta de destino. Deseja sobrescrevê-lo?", "Sobrescrever arquivo", MessageBoxButtons.YesNo);

                                        if (overwriteResult == DialogResult.Yes)
                                        {
                                            File.Copy(arquivoOrigem, caminhoCompletoDestino, true);
                                            fezBackup = true;
                                        }
                                        // Se o usuário escolher não sobrescrever, não faz nada para este arquivo
                                    }
                                    else
                                    {
                                        File.Copy(arquivoOrigem, caminhoCompletoDestino, false);
                                    }
                                }
                            }
                            else if (chooseIndividualResult == DialogResult.No)
                            {
                                // Não sobrescrever nenhum arquivo, apenas listar os que não foram copiados
                                List<string> naoCopiados = new List<string>();

                                foreach (string arquivoOrigem in arquivosOrigem)
                                {
                                    string nomeArquivo = Path.GetFileName(arquivoOrigem);
                                    string caminhoCompletoDestino = Path.Combine(destino, nomeArquivo);

                                    if (File.Exists(caminhoCompletoDestino))
                                    {
                                        naoCopiados.Add(nomeArquivo.Replace(".sqlite", ""));
                                    }
                                    else
                                    {
                                        File.Copy(arquivoOrigem, caminhoCompletoDestino, false);
                                    }
                                }

                                if (naoCopiados.Count > 0)
                                {
                                    MessageBox.Show($"Os seguintes arquivos não foram copiados porque já existem na pasta de destino:\n\n{string.Join("\n", naoCopiados)}", "Arquivos não copiados");
                                }
                            }
                        }

                        // Exibir mensagem de sucesso apenas se fez o backup
                        if (fezBackup)
                        {
                            MessageBox.Show("Todos os arquivos possíveis foram copiados com sucesso!", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    else
                    {
                        MessageBox.Show("A pasta de banco de dados do programa não existe ou está vazia.", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
        }

        public static bool arquivoEstaNoDiretorio(string diretorio, string arquivoCamihno)
        {
            // Obtém os caminhos absolutos
            var diretorioInfo = new DirectoryInfo(diretorio);
            var arquivoInfo = new FileInfo(arquivoCamihno);

            // Verifica se o caminho absoluto do arquivo começa com o caminho absoluto do diretório pai
            return arquivoInfo.FullName.StartsWith(diretorioInfo.FullName, StringComparison.OrdinalIgnoreCase);
        }

        private void btnRestaurar_Click(object sender, EventArgs e)
        {
            // Seleciona o botão sem mudar o tema, pois pode haver algum form já aberto
            selecionarBotao(sender, false);

            string pastaBDs = Path.Combine(Application.StartupPath, "databases");

            var dialogResult = MessageBox.Show("Deseja restaurar apenas um banco de dados?", "Restauração", MessageBoxButtons.YesNoCancel);

            if (dialogResult == DialogResult.Yes)
            {
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Arquivos SQLite (*.sqlite)|*.sqlite";
                    openFileDialog.Title = "Selecione o arquivo do banco de dados";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string arquivoSelecionado = openFileDialog.FileName;
                        string nomeArquivo = Path.GetFileNameWithoutExtension(arquivoSelecionado);
                        string caminhoDestino = $"{pastaBDs}\\{nomeArquivo}.sqlite";

                        // Verificar se o arquivo não está nas dependências do programa
                        if (arquivoEstaNoDiretorio(Application.StartupPath, arquivoSelecionado))
                        {
                            MessageBox.Show("Não é possível restaurar um arquivo que está nas pastas do programa, tente novamente e selecione outro diretório.", "Erro ao realizar backup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        if (File.Exists(caminhoDestino))
                        {
                            if (nomeArquivo == lblBanco.Text)
                            {
                                var confirmResult = MessageBox.Show("Você está prestes a sobrescrever o banco de dados atual. Após a restauração você será enviado para a tela de login. Confirma a restauração?", "Confirmação", MessageBoxButtons.YesNo);
                                if (confirmResult == DialogResult.Yes)
                                {
                                    con.Desconectar();
                                    File.Copy(arquivoSelecionado, caminhoDestino, true);
                                    MessageBox.Show("Arquivo restaurado com sucesso!");

                                    this.Owner.Show(); // Exibe o Formulário de Login
                                    this.Dispose(); // Fecha o formulário atual
                                }
                            }
                            else
                            {
                                var confirmResult = MessageBox.Show("O arquivo selecionado já existe e será sobrescrito. Confirma a restauração?", "Confirmação", MessageBoxButtons.YesNo);
                                if (confirmResult == DialogResult.Yes)
                                {
                                    File.Copy(arquivoSelecionado, caminhoDestino, true);
                                    MessageBox.Show("Arquivo restaurado com sucesso!");
                                }
                            }
                        }
                        else
                        {
                            File.Copy(arquivoSelecionado, caminhoDestino);
                            MessageBox.Show("Arquivo restaurado com sucesso!");
                        }
                    }
                }
            }
            else if (dialogResult == DialogResult.No)
            {
                // Controla se o form voltará para tela de Login no final ou não (se substituir o arquivo do banco atual ele voltará).
                bool voltarTelaLogin = false;

                // Restaurar vários bancos de dados
                using (var folderBrowserDialog = new FolderBrowserDialog())
                {
                    folderBrowserDialog.Description = "Selecione a pasta com os bancos de dados a serem restaurados";

                    if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                    {
                        string pastaSelecionada = folderBrowserDialog.SelectedPath;
                        int contadorSucessos = 0;
                        List<string> arquivosNaoRestaurados = new List<string>();

                        // Verificar se a pasta selecionada não está nas dependências do programa
                        if (IsSubDiretorio(Application.StartupPath, pastaSelecionada))
                        {
                            MessageBox.Show("Não é possível restaurar a partir de arquivos que estão nas pastas do programa, tente novamente e selecione outro diretório.", "Erro ao realizar backup", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        // Verificar se há arquivos .sqlite na pasta selecionada
                        string[] arquivosSQLite = Directory.GetFiles(pastaSelecionada, "*.sqlite");
                        if (arquivosSQLite.Length == 0)
                        {
                            MessageBox.Show("Não foi possível encontrar nenhum banco de dados na pasta informada.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        var confirmResult = MessageBox.Show("Deseja sobrescrever todos os arquivos de banco de dados existentes?", "Confirmação", MessageBoxButtons.YesNoCancel);

                        // Cancelar caso o usuário aperte Cancel ou feche de alguma forma
                        if (confirmResult != DialogResult.Yes && confirmResult != DialogResult.No)
                        {
                            return;
                        }

                        bool sobrescreverTodos = (confirmResult == DialogResult.Yes);

                        bool perguntarAntes = false;

                        if (!sobrescreverTodos)
                        {
                            var perguntarResult = MessageBox.Show("Deseja então escolher quais serão sobrescritos e quais não? \n\nEscolher \"não\" fará com que nenhum arquivo existente seja sobrescrito.", "Confirmação", MessageBoxButtons.YesNoCancel);
                            perguntarAntes = (perguntarResult == DialogResult.Yes);

                            // Cancelar caso o usuário aperte Cancel ou feche de alguma forma
                            if (perguntarResult != DialogResult.Yes && perguntarResult != DialogResult.No)
                            {
                                return;
                            }
                        }

                        // Percorrer todos os arquivos na pasta selecionada
                        foreach (string arquivoSelecionado in arquivosSQLite)
                        {
                            string nomeArquivo = Path.GetFileNameWithoutExtension(arquivoSelecionado);
                            string caminhoDestino = $"{pastaBDs}\\{nomeArquivo}.sqlite";

                            if (File.Exists(caminhoDestino))
                            {
                                if (sobrescreverTodos)
                                {
                                    // Verificar se é o mesmo banco de dados atual
                                    if (nomeArquivo == lblBanco.Text)
                                    {
                                        con.Desconectar();
                                        File.Copy(arquivoSelecionado, caminhoDestino, true);
                                        voltarTelaLogin = true;
                                        MessageBox.Show("Arquivo do banco de dados atual restaurado, você será enviado para a tela de Login após a conclusão.");
                                    }
                                    else
                                    {
                                        // Sobrescrever sem perguntar
                                        File.Copy(arquivoSelecionado, caminhoDestino, true);
                                    }
                                }
                                else
                                {
                                    // Caso o arquivo exista mas o usuário optou por perguntar antes se deseja sobreescrever
                                    if (perguntarAntes)
                                    {
                                        // Se o arquivo for igual ao do banco atual
                                        if (nomeArquivo == lblBanco.Text)
                                        {
                                            confirmResult = MessageBox.Show($"ATENÇÃO: O arquivo \"{nomeArquivo}\" já existe e corresponde ao banco de dados atual. Se o arquivo for restaurado, você será enviado para a tela de login. Confirma a restauração?", "Confirmação", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                                            if (confirmResult == DialogResult.Yes)
                                            {
                                                con.Desconectar();
                                                File.Copy(arquivoSelecionado, caminhoDestino, true);
                                                voltarTelaLogin = true;
                                                MessageBox.Show("Arquivo do banco de dados atual restaurado, você será enviado para a tela de Login após a conclusão.");
                                            }
                                        }
                                        // Se o arquivo não for o do banco atual
                                        else
                                        {
                                            confirmResult = MessageBox.Show($"O arquivo \"{nomeArquivo}\" já existe. Deseja sobrescrevê-lo?", "Confirmação", MessageBoxButtons.YesNo);

                                            if (confirmResult == DialogResult.Yes)
                                            {
                                                File.Copy(arquivoSelecionado, caminhoDestino, true);
                                            }
                                        }
                                    }
                                    // Caso ele tenha optado por não sobreescrever nada, contar o arquivo não restaurado
                                    {
                                        arquivosNaoRestaurados.Add(nomeArquivo);
                                    }
                                }
                            }
                            else
                            {
                                // Arquivo não existe, apenas copiar
                                File.Copy(arquivoSelecionado, caminhoDestino);
                                contadorSucessos++;
                            }
                        }

                        if (sobrescreverTodos)
                        {
                            MessageBox.Show($"Restauração concluída com sucesso! Todos os arquivo(s) foram restaurado(s).", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (!sobrescreverTodos && perguntarAntes)
                        {
                            MessageBox.Show($"Restauração concluída com sucesso! Todos os arquivo(s) confirmados foram restaurado(s).", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else if (!sobrescreverTodos && !perguntarAntes)
                        {
                            // Final
                            if (contadorSucessos > 0)
                            {
                                MessageBox.Show($"Restauração concluída com sucesso! Todos os arquivo(s) foram restaurado(s).", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            else if (arquivosNaoRestaurados.Count > 0)
                            {
                                MessageBox.Show($"Os seguintes arquivos não foram restaurados porque já existem na pasta do programa:\n\n{string.Join("\n", arquivosNaoRestaurados)}", "Arquivos não copiados");
                            }
                            else
                            {
                                MessageBox.Show("Nenhum arquivo foi restaurado, todos já existem na pasta do programa.", "Informação", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                }

                if (voltarTelaLogin)
                {
                    this.Owner.Show(); // Exibe o Formulário de Login
                    this.Dispose(); // Fecha o formulário atual
                }
            }
        }

        private void btnMaximizar_Click(object sender, EventArgs e)
        {
            alterarVisibilidade();
        }

        private void btnMinimizar2_Click(object sender, EventArgs e)
        {
            alterarVisibilidade();
        }

        private void alterarVisibilidade()
        {
            if (this.WindowState == FormWindowState.Maximized)
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }

        private void btnRecalcular_Click(object sender, EventArgs e)
        {
            // Seleciona o botão sem mudar o tema, pois pode haver algum form já aberto
            selecionarBotao(sender, false);

            frmLancamentos.recalcularLancamentos(con);
        }
    }
}