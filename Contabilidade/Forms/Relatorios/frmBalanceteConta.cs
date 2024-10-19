using Contabilidade.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Data;
using System.Diagnostics;
using System.Text;
using Microsoft.Data.Sqlite;

namespace Contabilidade.Forms.Relatorios
{
    public partial class frmBalanceteConta : Form
    {
        Conexao con;
        static DataTable dtDados = new DataTable();
        DataView dv = dtDados.DefaultView;
        private string nivel = "";
        public frmBalanceteConta(Conexao conexaoBanco)
        {
            InitializeComponent();

            con = conexaoBanco;

            atualizarDataGrid();
        }

        public void atualizarDataGrid()
        {
            // Query de pesquisa
            string sql = "SELECT * FROM contas ORDER BY conta;";
            using (var command = new SqliteCommand(sql, con.conn))
            {
                dtDados.Clear();
                using (var reader = command.ExecuteReader())
                {
                    dtDados.Load(reader);
                }

                dgvContas.DataSource = dtDados;

                dv.RowFilter = $"conta LIKE '{txtFiltrar.Text}%'";
                dgvContas.DataSource = dv;

                cbbFiltrar.SelectedIndex = 0;
            }
        }

        private void cbbFiltrar_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Filtar por nível 
            if (cbbFiltrar.SelectedIndex == 2)
            {
                cbbNivel.Visible = true;
                txtFiltrar.Visible = false;
            }
            else
            {
                cbbNivel.Visible = false;
                txtFiltrar.Visible = true;
                txtFiltrar.Width = 238;
            }

            txtFiltrar_TextChanged(sender, e);
        }

        private void txtFiltrar_TextChanged(object sender, EventArgs e)
        {
            // Conta
            if (cbbFiltrar.SelectedIndex == 0)
            {
                dv.RowFilter = $"conta LIKE '{txtFiltrar.Text}%'";
                dgvContas.DataSource = dv;
            }
            // Descrição
            else if (cbbFiltrar.SelectedIndex == 1)
            {
                dv.RowFilter = $"descricao LIKE '{txtFiltrar.Text}%'";
                dgvContas.DataSource = dv;
            }
            // Nível
            else if (cbbFiltrar.SelectedIndex == 2)
            {
                // Analitico
                if (cbbNivel.SelectedIndex == 0)
                {
                    dv.RowFilter = "nivel = 'A'";
                }
                // Sintetico
                else if (cbbNivel.SelectedIndex == 1)
                {
                    dv.RowFilter = "nivel = 'S'";
                }
                // Ambos
                else if (cbbNivel.SelectedIndex == 2)
                {
                    dv.RowFilter = "";
                }

                dgvContas.DataSource = dv;
            }
        }

        public static (int, int) ObterMenorEMaior(int valor1, int valor2)
        {
            if (valor1 < valor2)
            {
                return (valor1, valor2);
            }
            else
            {
                return (valor2, valor1);
            }
        }

        private bool IsNumeric(string text)
        {
            return int.TryParse(text, out _);
        }

        private void dgvContas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvContas.Rows[e.RowIndex];
                txtConta.Text = row.Cells["Conta"].Value.ToString();
                nivel = row.Cells["Nível"].Value.ToString();
            }
        }
        private class LancamentoAnalitico
        {
            public DateTime data { get; set; }
            public string historico { get; set; }
            public decimal valor { get; set; }
            public decimal saldo { get; set; }
        }
        private class LancamentoSintetico
        {
            public string conta { get; set; }
            public string descricao { get; set; }
            public decimal debitos { get; set; }
            public decimal creditos { get; set; }
            public decimal saldo { get; set; }
        }

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            /*
            // Verificar se a conta foi preenchida
            if (string.IsNullOrWhiteSpace(txtConta.Text))
            {
                MessageBox.Show("Selecione uma conta antes de gerar o relatório, dê um duplo clique na linha desejada.", "Uma conta não foi selecionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Obter datas
                var (dataInicial, dataInicialFormatada, dataFinal, dataFinalFormatada) = Contabilidade.Forms.Relatorios.frmRazaoAnalitico.ordenarDatasEObterStrings(dtpInicial.Value, dtpFinal.Value);

                if (nivel == "S")
                {
                    // Consulta de dados no período informado
                    var sql = "WITH LancamentosOrdenados AS (SELECT l.conta, l.valor, l.saldo_anterior, l.saldo_atualizado, l.data, l.id, ROW_NUMBER() OVER (PARTITION BY l.conta ORDER BY l.data ASC, l.id ASC) AS rn_asc, ROW_NUMBER() OVER (PARTITION BY l.conta ORDER BY l.data DESC, l.id DESC) AS rn_desc FROM lancamentos l WHERE l.data BETWEEN @dataInicial AND @dataFinal AND l.conta LIKE @conta || '.%') SELECT c.conta, c.descricao, SUM(CASE WHEN lo.valor > 0 THEN lo.valor ELSE 0 END) AS creditos, SUM(CASE WHEN lo.valor < 0 THEN lo.valor ELSE 0 END) AS debitos, MAX(CASE WHEN lo.rn_asc = 1 THEN lo.saldo_anterior ELSE NULL END) AS saldo_anterior, MAX(CASE WHEN lo.rn_desc = 1 THEN lo.saldo_atualizado ELSE NULL END) AS saldo_atualizado FROM LancamentosOrdenados lo JOIN contas c ON lo.conta = c.conta GROUP BY c.conta, c.descricao ORDER BY MIN(lo.conta) ASC;";
                    using (var comando = new SqliteCommand(sql, con.conn))
                    {
                        comando.Parameters.AddWithValue("@dataInicial", dataInicial);
                        comando.Parameters.AddWithValue("@dataFinal", dataFinal);
                        comando.Parameters.AddWithValue("@conta", txtConta.Text);

                        List<LancamentoSintetico> listLancamentos = new List<LancamentoSintetico>();

                        // Obter dados
                        using (var reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                LancamentoSintetico lancamento = new LancamentoSintetico
                                {
                                    conta = reader["conta"].ToString(),
                                    descricao = reader["descricao"].ToString(),
                                    debitos = Convert.ToDecimal(reader["debitos"]),
                                    creditos = Convert.ToDecimal(reader["creditos"]),
                                    saldo_anterior = Convert.ToDecimal(reader["saldo_anterior"]),
                                    saldo_atualizado = Convert.ToDecimal(reader["saldo_atualizado"])
                                };
                                listLancamentos.Add(lancamento);
                            }
                        }

                        // Verificar se pelo menos 1 registro foi encontrado
                        if (listLancamentos.Count > 0)
                        {
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
                                        var fonte = new iTextSharp.text.Font(bf, 8.1f); // Padrão 9. Adicionou 12 caracteres por linha

                                        var linhasDisponiveis = 63; // Padrão 57, adicionou 6 linhas.

                                        // Operações com o subtítulo
                                        string subtitulo = "   ";
                                        if (!string.IsNullOrWhiteSpace(txtSubtitulo.Text))
                                        {
                                            subtitulo = Contabilidade.Forms.Relatorios.frmSaldo.CentralizarString(txtSubtitulo.Text, 122);
                                        }

                                        // Função local para adicionar o cabeçalho
                                        void adicionarCabecalho(string subtitulo)
                                        {
                                            // Adicionando parágrafos ao documento
                                            pdf.Add(new Paragraph($"BALANCETE                                      PERÍODO: {dataInicialFormatada} A {dataFinalFormatada}                                PÁGINA: {(pdf.PageNumber + 1).ToString("D3")}", fonte));
                                            pdf.Add(new Paragraph($"{subtitulo}", fonte));
                                            pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                            pdf.Add(new Paragraph("CONTA           DESCRIÇÃO                                         SALDO ANTERIOR       DEBITOS      CREDITOS   SALDO ATUAL", fonte));
                                            pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                            pdf.Add(new Paragraph($"   ", fonte));

                                            // Contar linhas usadas após adição do cabeçalho
                                            linhasDisponiveis -= 6;
                                        };

                                        // Adicionar cabeçalho da primeira página
                                        adicionarCabecalho(subtitulo);

                                        // Variáveis para controle
                                        decimal totalSaldoAnterior = 0;
                                        decimal totalDebitos = 0;
                                        decimal totalCreditos = 0;
                                        decimal totalSaldoAtualizado = 0;

                                        // Para cada lançamento
                                        foreach (var lancamento in listLancamentos)
                                        {
                                            // Obter dados
                                            var (conta, descricao, debitos, creditos, saldo_anterior, saldo_atualizado) = (lancamento.conta, lancamento.descricao, lancamento.debitos, lancamento.creditos, lancamento.saldo_anterior, lancamento.saldo_atualizado);

                                            // Iniciar criação de linha
                                            var linha = new StringBuilder();

                                            // Verificar quantas linhas serão necessárias para cada conta - Não contar o espaço entre as colunas
                                            var linhasNecessarias = lancamento.descricao.Length > 50 ? 2 : 1;

                                            // Verificar se há linhas nessa página para incluir os registros, caso não haja: criar nova página com o cabeçalho
                                            if ((linhasDisponiveis - linhasNecessarias) < 0)
                                            {
                                                pdf.NewPage();
                                                linhasDisponiveis = 63;
                                                adicionarCabecalho(subtitulo);
                                            }

                                            // Adicionar valores para o rodapé
                                            totalSaldoAnterior += saldo_anterior;
                                            totalCreditos += creditos;
                                            totalDebitos += debitos;
                                            totalSaldoAtualizado += saldo_atualizado;

                                            // Adicionar número da conta
                                            linha.Append(conta.PadRight(16));

                                            // Testar se serão necessárias 1 ou 2 linhas por causa do comprimento do histórico
                                            if (linhasNecessarias == 2)
                                            {
                                                // Dividir considerando o tamanho máximo que pode ter
                                                string[] linhasHistorico = Contabilidade.Forms.Relatorios.frmSaldo.QuebrarLinhaString(descricao, 50);
                                                linha.Append(linhasHistorico[0]);

                                                linha.Append(saldo_anterior.ToString("#,##0.00").PadLeft(14));
                                                linha.Append(debitos.ToString("#,##0.00").PadLeft(14));
                                                linha.Append(creditos.ToString("#,##0.00").PadLeft(14));
                                                linha.Append(saldo_atualizado.ToString("#,##0.00").PadLeft(14));

                                                // Adicionar primeira linha
                                                pdf.Add(new Paragraph(linha.ToString(), fonte));

                                                // Limpar o StringBuilder e iniciar a criação da segunda linha (com conta e valores vázios).
                                                linha.Clear();

                                                // Espaço vázio referente a conta
                                                linha.Append("   ".PadRight(16));

                                                // Adicionar segunda linha
                                                linha.Append(linhasHistorico[1]);
                                                pdf.Add(new Paragraph(linha.ToString(), fonte));

                                                // Contabilizar linhas
                                                linhasDisponiveis -= 2;
                                            }
                                            else
                                            {
                                                linha.Append(descricao.PadRight(50));
                                                linha.Append(saldo_anterior.ToString("#,##0.00").PadLeft(14));
                                                linha.Append(debitos.ToString("#,##0.00").PadLeft(14));
                                                linha.Append(creditos.ToString("#,##0.00").PadLeft(14));
                                                linha.Append(saldo_atualizado.ToString("#,##0.00").PadLeft(14));

                                                pdf.Add(new Paragraph(linha.ToString(), fonte));

                                                // Contabilizar linha
                                                linhasDisponiveis--;
                                            }
                                        }

                                        // ESSE RODAPÉ DEVE SER REMOVIDO E CRIADO PARA CADA GRUPO DE CONTAS
                                        // Verificar se existe espaço para o último rodapé, senão: criar nova página
                                        if (linhasDisponiveis < 1)
                                        {
                                            pdf.NewPage();
                                            linhasDisponiveis = 63; // Padrão 57
                                            adicionarCabecalho(subtitulo);
                                        }
                                        // Verificar se é possível inserir um espaço superior
                                        else if (linhasDisponiveis > 2)
                                        {
                                            pdf.Add(new Paragraph("    "));
                                            linhasDisponiveis--;
                                        }

                                        // Inserindo rodapé
                                        pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                        pdf.Add(new Paragraph($"{$"TOTAL DO PERÍODO ({dataInicialFormatada} A {dataFinalFormatada}):".PadLeft(66)}{totalSaldoAnterior.ToString("#,##0.00").PadLeft(14)}{totalDebitos.ToString("#,##0.00").PadLeft(14)}{totalCreditos.ToString("#,##0.00").PadLeft(14)}{totalSaldoAtualizado.ToString("#,##0.00").PadLeft(14)}", fonte));
                                        linhasDisponiveis -= 2;
                                        if (linhasDisponiveis > 0)
                                        {
                                            pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                        }

                                        // Fechando o documento
                                        pdf.Close();
                                    }

                                    // Abrir o arquivo PDF gerado
                                    Process.Start(new ProcessStartInfo(pdfPath) { UseShellExecute = true });
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Nenhum lançamento foi realizado entre as datas {dataInicialFormatada} e {dataFinalFormatada}", "O relatório não foi gerado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
                // Se for uma conta analítica - gerar um razão analítico
                else
                {
                    // Consulta de lançamentos para a conta no período informado
                    var sql = "SELECT l.data, h.historico, l.valor, l.saldo_atualizado FROM lancamentos l JOIN historicos h ON l.id_historico = h.id WHERE l.conta = @conta AND l.data BETWEEN @dataInicial AND @dataFinal ORDER BY l.data ASC, l.id ASC;";
                    using (var comando = new SqliteCommand(sql, con.conn))
                    {
                        var conta = txtConta.Text;
                        comando.Parameters.AddWithValue("@conta", conta);
                        comando.Parameters.AddWithValue("@dataInicial", dataInicial);
                        comando.Parameters.AddWithValue("@dataFinal", dataFinal);

                        List<LancamentoAnalitico> listLancamentos = new List<LancamentoAnalitico>();

                        // Obter dados
                        using (var reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                LancamentoAnalitico lancamento = new LancamentoAnalitico
                                {
                                    data = Convert.ToDateTime(reader["data"]),
                                    historico = reader["historico"].ToString(),
                                    valor = Convert.ToDecimal(reader["valor"]),
                                    saldo = Convert.ToDecimal(reader["saldo_atualizado"])
                                };
                                listLancamentos.Add(lancamento);
                            }
                        }

                        // Verificar se pelo menos 1 registro foi encontrado
                        if (listLancamentos.Count > 0)
                        {
                            // Obter a descrição da conta
                            comando.CommandText = "SELECT descricao FROM contas WHERE conta = @conta;";
                            var descricao = comando.ExecuteScalar()?.ToString();

                            // Obter saldo anterior (ou deixar como 0 se não possuir nenhum registro no período ou antes do período) - CONFERIR SE ESTA CORRETO !!!!!!!!!!!!!!!
                            comando.CommandText = "SELECT COALESCE((SELECT saldo_anterior FROM lancamentos WHERE conta = @conta AND data BETWEEN @dataInicial AND @dataFinal ORDER BY data ASC, id ASC LIMIT 1), (SELECT saldo_anterior FROM lancamentos WHERE conta = @conta AND data < @dataInicial ORDER BY data DESC, id DESC LIMIT 1), 0) AS saldo_anterior;";
                            var saldoAnterior = Convert.ToDecimal(comando.ExecuteScalar());

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

                                        // Operações com o subtítulo
                                        string subtitulo = "   ";
                                        if (!string.IsNullOrWhiteSpace(txtSubtitulo.Text))
                                        {
                                            subtitulo = Contabilidade.Forms.Relatorios.frmSaldo.CentralizarString(txtSubtitulo.Text, 110);
                                        }

                                        string[] linhasDescricao = Contabilidade.Forms.Relatorios.frmSaldo.QuebrarLinhaString(descricao, 78);

                                        // Função local para adicionar o cabeçalho
                                        void adicionarCabecalho(string subtitulo)
                                        {
                                            // Adicionando parágrafos ao documento
                                            pdf.Add(new Paragraph($"RAZÃO ANALÍTICO                          PERÍODO: {dataInicialFormatada} A {dataFinalFormatada}                          PÁGINA: {(pdf.PageNumber + 1).ToString("D3")}", fonte));
                                            pdf.Add(new Paragraph($"{subtitulo}", fonte));
                                            pdf.Add(new Paragraph($"CONTA: {conta.PadRight(15)}          {linhasDescricao[0]}", fonte));
                                            pdf.Add(new Paragraph($"SALDO ANTERIOR:{saldoAnterior.ToString("#,##0.00").PadLeft(14)}   {linhasDescricao[1]}", fonte));
                                            pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                            pdf.Add(new Paragraph("DATA       HISTÓRICO                                                       DÉBITOS      CRÉDITOS         SALDO", fonte));
                                            pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                            pdf.Add(new Paragraph($"   ", fonte));

                                            // Contar linhas usadas após adição do cabeçalho
                                            linhasDisponiveis -= 8;
                                        };

                                        // Adicionar cabeçalho da primeira página
                                        adicionarCabecalho(subtitulo);

                                        // Variáveis para controle
                                        string mesAnterior = "";
                                        string dataAnterior = "";
                                        decimal debitosMes = 0;
                                        decimal creditosMes = 0;
                                        decimal totalDebitos = 0;
                                        decimal totalCreditos = 0;

                                        // Para cada lançamento
                                        foreach (var lancamento in listLancamentos)
                                        {
                                            // Obter dados
                                            var (data, historico, valor, saldo) = (lancamento.data, lancamento.historico, lancamento.valor, lancamento.saldo);
                                            var dataLancamento = data.ToString("dd/MM/yyyy");

                                            // Iniciar criação de linha
                                            var linha = new StringBuilder();

                                            // Verificar se o mês anterior é diferente do atual 
                                            if (mesAnterior != data.ToString("MM/yyyy"))
                                            {
                                                // Verificar se é ou não a primeira execução, se não for (IF = TRUE): inserir rodapé com o total do mês anterior
                                                if (mesAnterior != "")
                                                {
                                                    // Verificar se há linhas nessa página para incluir o rodapé, caso não haja: criar nova página com o cabeçalho
                                                    if (linhasDisponiveis < 2)
                                                    {
                                                        pdf.NewPage();
                                                        linhasDisponiveis = 57;
                                                        adicionarCabecalho(subtitulo);
                                                    }

                                                    var linhasGastas = 2; // padrão do rodapé

                                                    // Verificar se é possível inserir com espaço superior
                                                    if (linhasDisponiveis >= 3)
                                                    {
                                                        pdf.Add(new Paragraph($"   ", fonte));
                                                        linhasGastas++;
                                                    }

                                                    // Inserir rodapé
                                                    pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                                    pdf.Add(new Paragraph($"{$"TOTAL DO MÊS {mesAnterior}:".PadLeft(68)}{debitosMes.ToString("#,##0.00").PadLeft(14)}{creditosMes.ToString("#,##0.00").PadLeft(14)}{(creditosMes + debitosMes).ToString("#,##0.00").PadLeft(14)}", fonte));

                                                    if (linhasDisponiveis >= 4)
                                                    {
                                                        pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                                        linhasGastas++;
                                                    }

                                                    // Verificar se é possível inserir com espaço inferior
                                                    if (linhasDisponiveis >= 5)
                                                    {
                                                        pdf.Add(new Paragraph($"   ", fonte));
                                                        linhasGastas++;
                                                    }

                                                    linhasDisponiveis -= linhasGastas;
                                                }
                                                // Transferir valores para o total
                                                totalDebitos += debitosMes;
                                                totalCreditos += creditosMes;

                                                // Zerar variáveis
                                                debitosMes = 0;
                                                creditosMes = 0;

                                                // Atualizar mês/ano
                                                mesAnterior = data.ToString("MM/yyyy");
                                            }

                                            // Verificar se a data anterior é igual a data desse lançamento, se for: não repetir e deixar o espaço vázio
                                            if (dataAnterior == dataLancamento)
                                            {
                                                linha.Append("   ".PadRight(11));
                                            }
                                            else
                                            {
                                                linha.Append(dataLancamento.PadRight(11));
                                                dataAnterior = dataLancamento;
                                            }

                                            // Verificar quantas linhas serão necessárias para cada uso - Não contar o espaço entre as colunas
                                            var linhasNecessarias = lancamento.historico.Length >= 57 ? 2 : 1;

                                            // Verificar se há linhas nessa página para incluir os registros, caso não haja: criar nova página com o cabeçalho
                                            if ((linhasDisponiveis - linhasNecessarias) < 0)
                                            {
                                                pdf.NewPage();
                                                linhasDisponiveis = 57;
                                                adicionarCabecalho(subtitulo);
                                            }

                                            // Testar se serão necessárias 1 ou 2 linhas por causa do comprimento do histórico
                                            if (linhasNecessarias == 2)
                                            {
                                                // Dividir considerando o tamanho máximo que pode ter
                                                string[] linhasHistorico = Contabilidade.Forms.Relatorios.frmSaldo.QuebrarLinhaString(historico, 57);
                                                linha.Append(linhasHistorico[0]);

                                                // Verificar se é um débito/crédito
                                                // Crédito
                                                if (valor > 0)
                                                {
                                                    // Contabilizar valor do lançamento
                                                    creditosMes += valor;

                                                    // String composta de caracteres vázios para ocupar o espaço do débito
                                                    linha.Append("   ".PadLeft(14));
                                                    // Espaçamento do crédito + 1 para a divisão entre as colunas
                                                    linha.Append(valor.ToString("#,##0.00").PadLeft(14));
                                                    // Valores referentes ao saldo_atualizado
                                                    linha.Append(saldo.ToString("#,##0.00").PadLeft(14));
                                                }
                                                // Débito
                                                else
                                                {
                                                    // Contabilizar valor do lançamento
                                                    debitosMes += valor;

                                                    // Espaçamento do débito + 1 para a divisão entre as colunas
                                                    linha.Append(valor.ToString("#,##0.00").PadLeft(14));
                                                    // String composta de caracteres vázios para ocupar o espaço do crédito
                                                    linha.Append("   ".PadLeft(14));
                                                    // Valores referentes ao saldo_atualizado
                                                    linha.Append(saldo.ToString("#,##0.00").PadLeft(14));
                                                }

                                                // Adicionar primeira linha
                                                pdf.Add(new Paragraph(linha.ToString(), fonte));

                                                // Limpar o StringBuilder e iniciar a criação da segunda linha (com conta e valores vázios).
                                                linha.Clear();

                                                // Espaço vázio referente a conta
                                                linha.Append("   ".PadRight(16));

                                                // Adicionar segunda linha
                                                linha.Append(linhasHistorico[1]);
                                                pdf.Add(new Paragraph(linha.ToString(), fonte));

                                                // Contabilizar linhas
                                                linhasDisponiveis -= 2;
                                            }
                                            else
                                            {
                                                linha.Append(historico.PadRight(57));

                                                // Verificar se é um débito/crédito
                                                // Crédito
                                                if (valor > 0)
                                                {
                                                    // Contabilizar valor do lançamento
                                                    creditosMes += valor;

                                                    // String composta de caracteres vázios para ocupar o espaço do débito
                                                    linha.Append("   ".PadLeft(14));
                                                    // Espaçamento do crédito + 1 para a divisão entre as colunas
                                                    linha.Append(valor.ToString("#,##0.00").PadLeft(14));
                                                    // Valores referentes ao saldo_atualizado
                                                    linha.Append(saldo.ToString("#,##0.00").PadLeft(14));
                                                }
                                                // Débito
                                                else
                                                {
                                                    // Contabilizar valor do lançamento
                                                    debitosMes += valor;

                                                    // Espaçamento do débito + 1 para a divisão entre as colunas
                                                    linha.Append(valor.ToString("#,##0.00").PadLeft(14));
                                                    // String composta de caracteres vázios para ocupar o espaço do crédito
                                                    linha.Append("   ".PadLeft(14));
                                                    // Valores referentes ao saldo_atualizado
                                                    linha.Append(saldo.ToString("#,##0.00").PadLeft(14));
                                                }

                                                pdf.Add(new Paragraph(linha.ToString(), fonte));

                                                // Contabilizar linha
                                                linhasDisponiveis--;
                                            }
                                        }

                                        // Verificar se há linhas nessa página para incluir o rodapé do último mês, caso não haja: criar nova página com o cabeçalho
                                        if (linhasDisponiveis < 2)
                                        {
                                            pdf.NewPage();
                                            linhasDisponiveis = 57;
                                            adicionarCabecalho(subtitulo);
                                        }

                                        var linhasUsadas = 2; // padrão do rodapé

                                        // Verificar se é possível inserir com espaço superior (já considerando o espaço do próximo/último rodapé)
                                        if (linhasDisponiveis != 4 && linhasDisponiveis >= 3)
                                        {
                                            pdf.Add(new Paragraph($"   ", fonte));
                                            linhasUsadas++;
                                        }

                                        // Inserir rodapé
                                        pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                        pdf.Add(new Paragraph($"{$"TOTAL DO MÊS {mesAnterior}:".PadLeft(68)}{debitosMes.ToString("#,##0.00").PadLeft(14)}{creditosMes.ToString("#,##0.00").PadLeft(14)}{(creditosMes + debitosMes).ToString("#,##0.00").PadLeft(14)}", fonte));

                                        if (linhasDisponiveis > 3)
                                        {
                                            pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                            linhasUsadas++;
                                        }

                                        linhasDisponiveis -= linhasUsadas;

                                        // Transferir valores para o total
                                        totalDebitos += debitosMes;
                                        totalCreditos += creditosMes;

                                        // Verificar se existe espaço para o último rodapé, senão: criar nova página
                                        if (linhasDisponiveis < 1)
                                        {
                                            pdf.NewPage();
                                            linhasDisponiveis = 57;
                                            adicionarCabecalho(subtitulo);
                                        }

                                        // Inserindo rodapé
                                        pdf.Add(new Paragraph($"{$"TOTAL DO PERÍODO: {dataInicialFormatada} A {dataFinalFormatada}".PadLeft(68)}{totalDebitos.ToString("#,##0.00").PadLeft(14)}{totalCreditos.ToString("#,##0.00").PadLeft(14)}{(totalCreditos + totalDebitos).ToString("#,##0.00").PadLeft(14)}", fonte));
                                        linhasDisponiveis--;
                                        if (linhasDisponiveis > 0)
                                        {
                                            pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                        }

                                        // Fechando o documento
                                        pdf.Close();
                                    }

                                    // Abrir o arquivo PDF gerado
                                    Process.Start(new ProcessStartInfo(pdfPath) { UseShellExecute = true });
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show($"Nenhum lançamento foi realizado entre as datas {dataInicialFormatada} e {dataFinalFormatada}", "O relatório não foi gerado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                }
            }
            */
        }
    }
}
