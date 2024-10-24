using Contabilidade.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Data;
using System.Diagnostics;
using System.Text;
using Microsoft.Data.Sqlite;
using Contabilidade.Classes;

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

        private class Lancamento
        {
            public string conta { get; set; }
            public string descricao { get; set; }
            public decimal debitos { get; set; }
            public decimal creditos { get; set; }
            public decimal saldo { get; set; }
        }

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificar se a conta foi preenchida
                if (string.IsNullOrWhiteSpace(txtConta.Text))
                {
                    MessageBox.Show("Selecione uma conta antes de gerar o relatório, dê um duplo clique na linha desejada.", "Uma conta não foi selecionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    var incluirSaldosZero = chkSaldosZero.Checked;

                    // Obter datas
                    var (dataInicial, dataInicialFormatada, dataFinal, dataFinalFormatada) = Contabilidade.Forms.Relatorios.frmRazaoAnalitico.ordenarDatasEObterStrings(dtpInicial.Value, dtpFinal.Value);

                    // Consulta de dados no período informado
                    // Verificar se é uma conta sintética (irá mostrar o grupo inteiro) ou uma conta analítica (mostrar apenas ela)
                    var sql = "";
                    if (nivel == "S")
                    {
                        // Verificar se deve incluir as contas com saldo 0
                        if (incluirSaldosZero)
                        {
                            sql = "SELECT l.conta, c.descricao, SUM(CASE WHEN l.valor > 0 THEN l.valor ELSE 0 END) AS credito, SUM(CASE WHEN l.valor < 0 THEN l.valor ELSE 0 END) AS debito, COALESCE((SELECT saldo FROM lancamentos WHERE conta = l.conta ORDER BY data DESC, id DESC LIMIT 1), 0) AS saldo FROM lancamentos l JOIN contas c ON l.conta = c.conta WHERE l.data BETWEEN @dataInicial AND @dataFinal AND l.conta LIKE @conta || '.%' GROUP BY l.conta ORDER BY l.conta;";
                        }
                        // Caso não deva incluir
                        else
                        {
                            sql = "WITH dados_contas AS (SELECT l.conta, c.descricao, SUM(CASE WHEN l.valor > 0 THEN l.valor ELSE 0 END) AS credito, SUM(CASE WHEN l.valor < 0 THEN l.valor ELSE 0 END) AS debito, COALESCE((SELECT saldo FROM lancamentos WHERE conta = l.conta ORDER BY data DESC, id DESC LIMIT 1), 0) AS saldo FROM lancamentos l JOIN contas c ON l.conta = c.conta WHERE l.data BETWEEN @dataInicial AND @dataFinal AND l.conta LIKE @conta || '.%' GROUP BY l.conta ORDER BY l.conta) SELECT * FROM dados_contas WHERE saldo != 0;";
                        }
                    }
                    else if (nivel == "A")
                    {
                        sql = "SELECT l.conta, c.descricao, SUM(CASE WHEN l.valor > 0 THEN l.valor ELSE 0 END) AS credito, SUM(CASE WHEN l.valor < 0 THEN l.valor ELSE 0 END) AS debito, COALESCE((SELECT saldo FROM lancamentos WHERE conta = l.conta ORDER BY data DESC, id DESC LIMIT 1), 0) AS saldo FROM lancamentos l JOIN contas c ON l.conta = c.conta WHERE l.data BETWEEN @dataInicial AND @dataFinal AND l.conta = @conta;";
                    }
                    else
                    {
                        throw new CustomException("Não foi possível obter o tipo da conta, por favor, tente novamente.");
                    }

                    using (var comando = new SqliteCommand(sql, con.conn))
                    {
                        comando.Parameters.AddWithValue("@dataInicial", dataInicial);
                        comando.Parameters.AddWithValue("@dataFinal", dataFinal);
                        comando.Parameters.AddWithValue("@conta", txtConta.Text);

                        List<Lancamento> listLancamentos = new List<Lancamento>();

                        // Obter dados
                        using (var reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Lancamento lancamento = new Lancamento
                                {
                                    conta = reader["conta"].ToString(),
                                    descricao = reader["descricao"].ToString(),
                                    debitos = Convert.ToDecimal(reader["debito"]),
                                    creditos = Convert.ToDecimal(reader["credito"]),
                                    saldo = Convert.ToDecimal(reader["saldo"])
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
                                            pdf.Add(new Paragraph($"BALANCETE DA CONTA: {txtConta.Text.PadRight(15)}            PERÍODO: {dataInicialFormatada} A {dataFinalFormatada}                                PÁGINA: {(pdf.PageNumber + 1).ToString("D3")}", fonte));
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
                                            var (conta, descricao, debitos, creditos, saldo_anterior, saldo_atualizado) = (lancamento.conta, lancamento.descricao, lancamento.debitos, lancamento.creditos, (lancamento.saldo - (lancamento.creditos + lancamento.debitos)), lancamento.saldo);

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
                                                var linhasHistorico = Contabilidade.Forms.Relatorios.frmSaldo.QuebrarLinhaString(descricao, 50);
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
            }
            catch (CustomException ex)
            {
                MessageBox.Show($"{ex.Message?.ToString()}", "Erro ao gerar o relatório", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao gerar o relatório", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
