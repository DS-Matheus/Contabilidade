using Contabilidade.Models;
using System.Data;
using System.Data.SQLite;
using Contabilidade.Classes;
using Contabilidade.Forms.Relatorios;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Diagnostics;

namespace Contabilidade.Forms.Cadastros
{
    public partial class frmContas : Form
    {
        Conexao con;
        static DataTable dtDados = new DataTable();
        DataView dv = dtDados.DefaultView;
        public static string conta { get; set; } = "";
        public static string descricao { get; set; } = "";
        public static string nivel { get; set; } = "";
        public static bool alterouNivel { get; set; } = false;

        public frmContas(Conexao conexaoBanco)
        {
            InitializeComponent();

            con = conexaoBanco;

            atualizarDataGrid();

            txtFiltrar.Select();
        }
        private void atualizarDataGrid()
        {
            // Query de pesquisa
            string sql = "SELECT * FROM contas ORDER BY conta;";
            using (var command = new SQLiteCommand(sql, con.conn))
            {
                dtDados.Clear();
                using (var reader = command.ExecuteReader())
                {
                    dtDados.Load(reader);
                }

                dgvContas.DataSource = dtDados;

                dv.RowFilter = $"conta LIKE '{txtFiltrar.Text}%'";
                dgvContas.DataSource = dv;

                cbbFiltrar.SelectedIndex = 1;
                cbbNivel.SelectedIndex = 0;
                txtFiltrar.MaxLength = 15;
            }
        }

        private void cbbFiltrar_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFiltrar.Text = "";
            cbbNivel.SelectedIndex = 0;

            // Filtar por nível 
            if (cbbFiltrar.SelectedIndex == 2)
            {
                cbbNivel.Visible = true;
                txtFiltrar.Visible = false;
                cbbNivel_SelectedIndexChanged(sender, e);
            }
            // Filtrar por conta ou descrição 
            else
            {
                cbbNivel.Visible = false;
                txtFiltrar.Visible = true;
                txtFiltrar_TextChanged(sender, e);
            }
        }

        private void cbbNivel_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ambos
            if (cbbNivel.SelectedIndex == 0)
            {
                dv.RowFilter = "";
            }
            // Analitico
            else if (cbbNivel.SelectedIndex == 1)
            {
                dv.RowFilter = "nivel = 'A'";
            }
            // Sintetico
            else if (cbbNivel.SelectedIndex == 2)
            {
                dv.RowFilter = "nivel = 'S'";
            }

            dgvContas.DataSource = dv;
        }

        private void txtFiltrar_TextChanged(object sender, EventArgs e)
        {
            // Conta
            if (cbbFiltrar.SelectedIndex == 0)
            {
                // Impedir da máscara ser aplicada quando não se tem dados inseridos (o que ocasiona erro)
                if (txtFiltrar.Text.Length > 0)
                {
                    TextBox textBox = sender as TextBox;
                    textBox.Text = frmContasDados.AplicarMascara(textBox.Text);
                    textBox.SelectionStart = textBox.Text.Length; // Mantém o cursor no final
                }

                txtFiltrar.MaxLength = 15;

                dv.RowFilter = $"conta LIKE '%{txtFiltrar.Text}%'";
                dgvContas.DataSource = dv;
            }
            // Descrição
            else if (cbbFiltrar.SelectedIndex == 1)
            {
                txtFiltrar.MaxLength = 100;
                dv.RowFilter = $"descricao LIKE '%{txtFiltrar.Text}%'";
                dgvContas.DataSource = dv;
            }
        }

        public static bool verificarExistenciaConta(string conta)
        {
            return dtDados.AsEnumerable().Any(row => string.Equals(conta, row.Field<string>("conta"), StringComparison.OrdinalIgnoreCase));

        }

        public static bool verificarExistenciaConta(string conta, string contaAntiga)
        {
            return dtDados.AsEnumerable().Any(row => string.Equals(conta, row.Field<string>("conta"), StringComparison.OrdinalIgnoreCase) && !string.Equals(row.Field<string>("conta"), contaAntiga, StringComparison.OrdinalIgnoreCase));

        }

        public static bool verificarContaSintetica(string conta)
        {
            // Remove o último grupo de caracteres após o último ponto (assim se obtêm a conta sintética associada)
            int ultimoPonto = conta.LastIndexOf('.');
            string contaSintetica = conta;
            if (ultimoPonto != -1)
            {
                contaSintetica = conta.Substring(0, ultimoPonto);
            }

            // Verificar se a conta existe e retornar
            return verificarExistenciaConta(contaSintetica);
        }

        public static string obterNivelConta(string conta) {
            var linha = dtDados.AsEnumerable().FirstOrDefault(row => conta == row.Field<string>("conta"));
            return linha != null ? linha.Field<string>("nivel") : null;
        }

        public static bool verificarContaPai(string conta)
        {
            // Remove o último grupo de caracteres após o último ponto (assim se obtêm a conta sintética associada)
            int ultimoPonto = conta.LastIndexOf('.');
            string contaAnalitica = conta;
            if (ultimoPonto != -1)
            {
                contaAnalitica = conta.Substring(0, ultimoPonto);
            }

            var contaExiste = verificarExistenciaConta(contaAnalitica);
            if (contaExiste)
            {
                var nivel = obterNivelConta(contaAnalitica);

                // Verificar se a conta é analítica
                if (nivel == "A")
                {
                    return true;
                }
            }

            return false;
        }

        private void btnCriar_Click(object sender, EventArgs e)
        {
            try
            {
                // Criar uma instância do formulário de dados e aguardar um retorno
                using (var frmDados = new frmContasDados("Criar conta", "", "", "A"))
                {
                    // O usuário apertou o botão de salvar
                    if (frmDados.ShowDialog() == DialogResult.OK)
                    {
                        using (var transacao = con.conn.BeginTransaction())
                        {
                            try
                            {
                                // Criar conta
                                string sql = "INSERT INTO contas (conta, descricao, nivel) VALUES(@conta, @descricao, @nivel);";
                                using (var comando = new SQLiteCommand(sql, con.conn))
                                {
                                    comando.Transaction = transacao;
                                    comando.Parameters.AddWithValue("@conta", conta);
                                    comando.Parameters.AddWithValue("@descricao", descricao);
                                    comando.Parameters.AddWithValue("@nivel", nivel);

                                    int retornoBD = comando.ExecuteNonQuery();

                                    // Verificar se houve a criação da linha (0 = negativo)
                                    if (retornoBD > 0)
                                    {
                                        // Adicionar dados na tabela
                                        DataRow row = dtDados.NewRow();
                                        row["conta"] = conta;
                                        row["descricao"] = descricao;
                                        row["nivel"] = nivel;
                                        dtDados.Rows.Add(row);

                                        dgvContas.Refresh();

                                        // Remover dados das variáveis
                                        conta = "";
                                        descricao = "";
                                        nivel = "";

                                        // Efetivar alterações
                                        transacao.Commit();

                                        MessageBox.Show("Conta criada com sucesso!", "Criação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        throw new CustomException("Não foi possível criar a nova conta.");
                                    }
                                }
                            }
                            catch (CustomException ex)
                            {
                                transacao.Rollback();
                                MessageBox.Show($"{ex.Message?.ToString()}", "Erro ao criar a conta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            catch (Exception ex)
                            {
                                transacao.Rollback();
                                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao criar a conta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao criar a conta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                // Exibir caixa de diálogo para o usuário definir o cabeçalho
                string titulo, subtitulo = "";

                using (var formDados = new frmDefinirCabecalho($"Contas cadastradas em {DateTime.Today.ToString("dd/MM/yyyy")}"))
                {
                    var resultado = formDados.ShowDialog();

                    // Se clicar em salvar
                    if (resultado == DialogResult.OK)
                    {
                        (titulo, subtitulo) = (formDados.titulo, formDados.subtitulo);
                    }
                    // Se cancelar ou fechar de alguma forma
                    else
                    {
                        return;
                    }
                }

                // Exibir caixa de diálogo para o usuário escolher onde salvar o arquivo PDF
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PDF Files|*.pdf";
                    saveFileDialog.Title = "Salvar relatório como";
                    saveFileDialog.FileName = "relatorio.pdf";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Caminho do arquivo PDF selecionado pelo usuário
                        string pdfPath = saveFileDialog.FileName;

                        // Criação do documento
                        var pdf = new iTextSharp.text.Document(PageSize.A4, 25, 25, 25, 25); // Margens padrão (36 pontos)

                        // Caminho do arquivo de fonte Consolas
                        string fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fontes", "consola.ttf");

                        // Verifica se o arquivo de fonte existe
                        if (!File.Exists(fontPath))
                        {
                            MessageBox.Show("Arquivo de fonte não encontrado, faça backup dos bancos de dados e reinstale o programa", "Erro ao buscar aquivo de fonte", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Usar `using` para garantir que o arquivo será liberado após ser usado
                        using (FileStream fileStream = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            // Criação do escritor de PDF
                            PdfWriter writer = PdfWriter.GetInstance(pdf, fileStream);

                            // Abrindo o documento
                            pdf.Open();

                            // Configuração da fonte Consola
                            BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                            var fonte = new iTextSharp.text.Font(bf, 9);

                            var linhasDisponiveis = 57;

                            // Função local para adicionar o cabeçalho
                            void adicionarCabecalho(string titulo, string subtitulo)
                            {
                                // Adicionando parágrafos ao documento
                                pdf.Add(new Paragraph($"{titulo?.PadRight(98)} PÁGINA: {writer.PageNumber.ToString("D3")}", fonte));
                                if (!string.IsNullOrWhiteSpace(subtitulo))
                                {
                                    pdf.Add(new Paragraph($"{subtitulo}", fonte));
                                    linhasDisponiveis--;
                                }
                                pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                pdf.Add(new Paragraph("(TIPO) NÚMERO - DESCRIÇÃO                                                                                     ", fonte));
                                pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                pdf.Add(new Paragraph("    ", fonte));

                                // Contar linhas usadas após adição do cabeçalho
                                linhasDisponiveis -= 5;
                            };

                            // Operações com o subtítulo
                            if (!string.IsNullOrWhiteSpace(subtitulo))
                            {
                                subtitulo = frmSaldo.CentralizarString(subtitulo, 110);
                            }

                            // Adicionar cabeçalho da primeira página
                            adicionarCabecalho(titulo, subtitulo);

                            var qtdLinhas = dgvContas.Rows.Count;

                            for (int i = 0; i < qtdLinhas; i++)
                            {
                                // Testar se possuí uma linha disponível para usar
                                if (linhasDisponiveis - 1 < 0)
                                {
                                    pdf.NewPage();
                                    linhasDisponiveis = 57;
                                    adicionarCabecalho(titulo, subtitulo);
                                }

                                var conta = dgvContas.Rows[i].Cells["Conta"].Value?.ToString();
                                var nivel = dgvContas.Rows[i].Cells["Nível"].Value?.ToString();
                                var descricao = dgvContas.Rows[i].Cells["Descrição"].Value?.ToString();

                                // Verificar quantas linhas serão necessárias para essa conta
                                var espacosDisponiveis = 110 - conta.Length - 7;
                                var linhasNecessarias = (descricao.Length > espacosDisponiveis) ? 2 : 1;

                                if (linhasNecessarias > 1)
                                {
                                    // Dividir a descrição considerando o tamanho máximo que pode ter
                                    var linhasDescricao = frmSaldo.QuebrarLinhaString(descricao, espacosDisponiveis);

                                    // Adicionar primeira linha
                                    pdf.Add(new Paragraph($"({nivel}) {conta} - {linhasDescricao[0]}", fonte));

                                    // Adicionar segunda linha (com espaço vázio referente a conta)
                                    pdf.Add(new Paragraph($"{"   ".PadRight(conta.Length + 7)}{linhasDescricao[1]}", fonte));

                                    // Contabilizar linhas
                                    linhasDisponiveis -= 2;
                                }
                                else
                                {
                                    pdf.Add(new Paragraph($"({nivel}) {conta} - {descricao}", fonte));
                                    linhasDisponiveis--;
                                }
                            }

                            // Fechando o documento
                            pdf.Close();
                        }
                        // Abrir o arquivo PDF gerado
                        Process.Start(new ProcessStartInfo(pdfPath) { UseShellExecute = true });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao gerar o relatório", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private (string conta, string descricao, string nivel) obterDadosDGV(int numLinha)
        {
            string conta = dgvContas.Rows[numLinha].Cells["Conta"].Value?.ToString();
            string descricao = dgvContas.Rows[numLinha].Cells["Descrição"].Value?.ToString();
            string nivel = dgvContas.Rows[numLinha].Cells["Nível"].Value?.ToString();

            return (conta, descricao, nivel);
        }

        public static string obterNovaConta(string contaSintetica, string contaAnalitica)
        {
            // Encontra o ponto de separação baseado nos pontos
            int index = contaSintetica.Length;

            // Monta a nova conta com a conta sintética e a parte remanescente da conta original
            string novaConta = contaSintetica + contaAnalitica.Substring(index);

            return novaConta;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                // Obter conta selecionada
                int numLinha = frmUsuarios.obterNumLinhaSelecionada(dgvContas);
                var (contaAntiga, descricaoAntiga, nivelAntigo) = obterDadosDGV(numLinha);

                // Criar uma instância do formulário de dados e aguardar um retorno
                using (var frmDados = new frmContasDados("Editar Conta", contaAntiga, descricaoAntiga, nivelAntigo, 1, con))
                {
                    // O usuário apertou o botão de salvar
                    if (frmDados.ShowDialog() == DialogResult.OK)
                    {
                        using (var transacao = con.conn.BeginTransaction())
                        {
                            try
                            {
                                using (var comando = new SQLiteCommand("", con.conn))
                                {
                                    comando.Transaction = transacao;

                                    // Se alterou o nível: excluir todos os registros associados
                                    if (alterouNivel)
                                    {
                                        // Se era analítico e alterou para sintético:
                                        if (nivel == "S")
                                        {
                                            // Excluir todos os lançamentos dessa conta apenas
                                            excluirTodosLancamentos(comando, contaAntiga);
                                        }
                                        // Se era sintético e alterou para analítico: excluir todas as contas no grupo de chave e os seus lançamentos
                                        else
                                        {
                                            // Obter relação de contas a serem excluidas
                                            comando.CommandText = "SELECT conta, nivel FROM contas WHERE conta LIKE @conta || '.%';";
                                            comando.Parameters.Clear();
                                            comando.Parameters.AddWithValue("@conta", contaAntiga);

                                            var listContas = new List<Contas>();

                                            // Ler e atribuir na lista as contas a serem excluídas
                                            using (var reader = comando.ExecuteReader())
                                            {
                                                while (reader.Read())
                                                {
                                                    var novaConta = new Contas(reader["conta"].ToString(), reader["nivel"].ToString());
                                                    listContas.Add(novaConta);
                                                }
                                            }

                                            // Excluir cada conta presente na lista
                                            foreach (var conta in listContas)
                                            {
                                                excluirConta(con, conta.Conta, conta.Nivel, transacao);
                                            }
                                        }
                                    }
                                    // Se não alterou o nível, testar se o número da conta mudou e se ela é sintética -> Se for: atualizar número de conta de todo o grupo
                                    else if (contaAntiga != conta && nivel == "S")
                                    {
                                        // obter relação de contas
                                        comando.CommandText = "SELECT conta FROM contas WHERE conta LIKE @conta || '.%';";
                                        comando.Parameters.Clear();
                                        comando.Parameters.AddWithValue("@conta", contaAntiga);

                                        var listContas = new List<string>();

                                        // Ler e atribuir na lista as contas a serem alteradas
                                        using (var reader = comando.ExecuteReader())
                                        {
                                            while (reader.Read())
                                            {
                                                var novaConta = reader["conta"].ToString();
                                                listContas.Add(novaConta);
                                            }
                                        }

                                        // Comando para atualizar as contas
                                        comando.CommandText = "UPDATE contas SET conta = @contaNova WHERE conta = @contaAntiga;";

                                        // Alterar cada conta presente na lista
                                        foreach (var contaFilha in listContas)
                                        {
                                            // Número de conta novo (var conta = novo número da conta sintética)
                                            var numeroNovo = obterNovaConta(conta, contaFilha);

                                            comando.Parameters.Clear();
                                            comando.Parameters.AddWithValue("@contaNova", numeroNovo);
                                            comando.Parameters.AddWithValue("@contaAntiga", contaFilha);
                                            comando.ExecuteNonQuery();
                                        }
                                    }

                                    // Atualizar conta (se o número dela alterou, os lançamentos também serão atualizados por causa do ON UPDATE da chave estrangeira)
                                    comando.CommandText = "UPDATE contas SET conta = @contaNova, descricao = @descricao, nivel = @nivel WHERE conta = @contaAntiga;";
                                    comando.Parameters.Clear();
                                    comando.Parameters.AddWithValue("@contaNova", conta);
                                    comando.Parameters.AddWithValue("@descricao", descricao);
                                    comando.Parameters.AddWithValue("@nivel", nivel);
                                    comando.Parameters.AddWithValue("@contaAntiga", contaAntiga);

                                    int retornoBD = comando.ExecuteNonQuery();

                                    // Verificar se houve a edição de alguma linha (0 = negativo)
                                    if (retornoBD > 0)
                                    {
                                        transacao.Commit();

                                        // Atualizar DataTable
                                        atualizarDataGrid();

                                        // Remover dados das variáveis
                                        conta = "";
                                        descricao = "";
                                        nivel = "";
                                        alterouNivel = false;

                                        MessageBox.Show("Conta editada com sucesso!", "Edição bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        throw new Exception("Não foi possível encontrar a conta ou ocorreu um erro na edição.");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                transacao.Rollback();
                                MessageBox.Show(ex.Message.ToString(), "Edição não realizada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao editar a conta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class TotalLancamentos
        {
            public string data { get; set; }
            public int saldo { get; set; }

            public TotalLancamentos(string data, int saldo)
            {
                this.data = data;
                this.saldo = saldo;
            }
        }

        public void excluirTodosLancamentos(SQLiteCommand comando, string conta)
        {
            // Obter a lista de datas que possuem lançamentos (com saldo diferente de 0)
            var listLancamentos = new List<TotalLancamentos>();
            comando.Parameters.Clear();
            comando.CommandText = "SELECT data, saldo FROM (SELECT data, saldo, ROW_NUMBER() OVER (PARTITION BY data ORDER BY id DESC) AS rn FROM lancamentos WHERE conta = @conta) WHERE rn = 1 and saldo != 0 ORDER BY data;";
            comando.Parameters.AddWithValue("@conta", conta);

            using (var reader = comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    listLancamentos.Add(
                        new TotalLancamentos(
                            reader["data"].ToString(),
                            Convert.ToInt32(reader["saldo"])
                        )
                    );
                }
            }

            // Verificar se foi encontrado algum lançamento
            if (listLancamentos.Any())
            {
                // Atualizar registros do caixa com os valores encontrados (reverter os lançamentos)
                comando.CommandText = "UPDATE registros_caixa SET saldo = (saldo - @valor) WHERE data = @data;";
                foreach (var registro in listLancamentos)
                {
                    comando.Parameters.Clear();
                    comando.Parameters.AddWithValue("@valor", registro.saldo);
                    comando.Parameters.AddWithValue("@data", registro.data);
                    comando.ExecuteNonQuery();
                }

                // Excluir todos os lançamentos da conta
                comando.CommandText = "DELETE FROM lancamentos WHERE conta = @conta;";
                comando.Parameters.Clear();
                comando.Parameters.AddWithValue("@conta", conta);
                comando.ExecuteNonQuery();
                
                comando.Parameters.Clear();
            }
        }

        public void excluirConta(Conexao con, string conta, string nivel, SQLiteTransaction transacao)
        {
            using (var comando = new SQLiteCommand("", con.conn))
            {
                comando.Transaction = transacao;

                // Se for uma conta analítica:
                if (nivel == "A")
                {
                    excluirTodosLancamentos(comando, conta);
                }

                // Excluir a conta
                comando.CommandText = "DELETE FROM contas WHERE conta = @conta;";
                comando.Parameters.Clear();
                comando.Parameters.AddWithValue("@conta", conta);
                var result = comando.ExecuteNonQuery();

                if (result == 0)
                {
                    throw new CustomException("Houve um erro ao excluir a conta");
                }
            }
        }

        private class Contas
        {
            public string Conta { get; set; }
            public string Nivel { get; set; }

            public Contas(string conta, string nivel)
            {
                this.Conta = conta;
                this.Nivel = nivel;
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                // Verifica se uma linha foi selecionada
                if (dgvContas.SelectedRows.Count > 0)
                {
                    // Obtem a linha selecionada
                    DataGridViewRow selectedRow = dgvContas.SelectedRows[0];

                    // Obtem a conta a ser excluida
                    var contaExcluir = selectedRow.Cells["Conta"].Value.ToString();
                    var nivelExcluir = selectedRow.Cells["Nível"].Value.ToString();

                    // Verifica se a conta selecionada não é o caixa
                    if (contaExcluir == "0")
                    {
                        MessageBox.Show("Não é possível excluir a conta do caixa", "Operação finalizada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var dialogResult = MessageBox.Show("Deseja excluir a conta selecionada? Esse processo não pode ser desfeito!", "Confirmação de exclusão da conta", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        using (var transacao = con.conn.BeginTransaction())
                        {
                            try
                            {
                                using (var comando = new SQLiteCommand("", con.conn))
                                {
                                    comando.Transaction = transacao;

                                    // Se o nível for sintético, avisar sobre a exclusão de todas as contas analiticas associadas
                                    if (nivelExcluir == "S")
                                    {
                                        dialogResult = MessageBox.Show("A conta selecionada é do tipo sintético, isso quer dizer que ao excluí-la todas as contas analíticas associadas também serão removidas, deseja continuar? Esse processo é irreversível!", "Confirmação de exclusão de conta sintética", MessageBoxButtons.YesNo);

                                        if (dialogResult != DialogResult.Yes)
                                        {
                                            return;
                                        }
                                    }

                                    // Confirmar uma última vez sobre a exclusão dos registros/lançamentos
                                    dialogResult = MessageBox.Show("Deseja realmente excluir a conta e, se existirem, todos os seus lancamentos associados? Este é o último aviso!", "Confirmação da exclusão da conta", MessageBoxButtons.YesNo);

                                    if (dialogResult == DialogResult.Yes)
                                    {
                                        // Verificar tipo da conta
                                        if (nivelExcluir == "S")
                                        {
                                            // Obter relação de contas a serem excluidas
                                            comando.CommandText = "SELECT conta, nivel FROM contas WHERE conta LIKE @conta || '.%';";
                                            comando.Parameters.AddWithValue("@conta", contaExcluir);

                                            var listContas = new List<Contas>();

                                            // Ler e atribuir na lista as contas a serem excluídas
                                            using (var reader = comando.ExecuteReader())
                                            {
                                                while (reader.Read())
                                                {
                                                    var novaConta = new Contas(reader["conta"].ToString(), reader["nivel"].ToString());
                                                    listContas.Add(novaConta);
                                                }
                                            }

                                            // Excluir cada conta presente na lista
                                            foreach (var conta in listContas)
                                            {
                                                excluirConta(con, conta.Conta, conta.Nivel, transacao);
                                            }
                                        }

                                        // Excluir a conta selecionada
                                        excluirConta(con, contaExcluir, nivelExcluir, transacao);

                                        // Efetivar alterações
                                        transacao.Commit();

                                        // Remover dados da tabela - Recarregar completamente
                                        atualizarDataGrid();

                                        MessageBox.Show("Conta excluida com sucesso.", "Operação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                            }
                            catch (CustomException ex)
                            {
                                transacao.Rollback();
                                MessageBox.Show($"{ex.Message?.ToString()}", "Erro ao excluir a conta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            catch (Exception ex)
                            {
                                transacao.Rollback();
                                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao excluir a conta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao excluir a conta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
