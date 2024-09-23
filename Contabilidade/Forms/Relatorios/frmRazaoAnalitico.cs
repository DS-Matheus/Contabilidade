using Contabilidade.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Contabilidade.Forms.Relatorios
{
    public partial class frmRazaoAnalitico : Form
    {
        Conexao con;
        static DataTable dtDados = new DataTable();
        DataView dv = dtDados.DefaultView;
        private string nivel = "";
        public frmRazaoAnalitico(Conexao conexaoBanco)
        {
            InitializeComponent();

            con = conexaoBanco;

            atualizarDataGrid();
        }

        public void atualizarDataGrid()
        {
            // Query de pesquisa
            string sql = "SELECT * FROM contas ORDER BY conta;";
            using (var command = new SQLiteCommand(sql, con.conn))
            {
                SQLiteDataAdapter sqlDA = new SQLiteDataAdapter(sql, con.conn);
                dtDados.Clear();
                sqlDA.Fill(dtDados);

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
                txtFiltrar2.Visible = false;
            }
            // Filtrar por Saldo entre
            else if (cbbFiltrar.SelectedIndex == 5)
            {
                cbbNivel.Visible = false;
                txtFiltrar.Visible = true;
                txtFiltrar2.Visible = true;
                txtFiltrar.Width = 115;
            }
            else
            {
                cbbNivel.Visible = false;
                txtFiltrar.Visible = true;
                txtFiltrar2.Visible = false;
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
            // Saldo menor que
            else if (cbbFiltrar.SelectedIndex == 3)
            {
                if (IsNumeric(txtFiltrar.Text))
                {
                    dv.RowFilter = $"saldo <= {txtFiltrar.Text}";
                    dgvContas.DataSource = dv;
                }
            }
            // Saldo maior que
            else if (cbbFiltrar.SelectedIndex == 4)
            {
                if (IsNumeric(txtFiltrar.Text))
                {
                    dv.RowFilter = $"saldo >= {txtFiltrar.Text}";
                    dgvContas.DataSource = dv;
                }
            }
            // Saldo entre
            else if (cbbFiltrar.SelectedIndex == 5)
            {
                if (IsNumeric(txtFiltrar.Text) && IsNumeric(txtFiltrar2.Text))
                {
                    var valores = ObterMenorEMaior(int.Parse(txtFiltrar.Text), int.Parse(txtFiltrar2.Text));
                    dv.RowFilter = $"saldo BETWEEN {valores.Item1} AND {valores.Item2}";
                    dgvContas.DataSource = dv;
                }
            }
        }

        private void txtFiltrar2_TextChanged(object sender, EventArgs e)
        {
            txtFiltrar_TextChanged(sender, e);
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
            public DateTime data {  get; set; }
            public string historico { get; set; }
            public decimal valor { get; set; }
            public decimal saldo {  get; set; }
        }
        
        public static (string, string) ordenarDatasEObterStrings(DateTime data1, DateTime data2)
        {
            // Se a data1 for menor ou igual a data2
            if (data1 <= data2)
            {
                return (data1.ToString("yyyy-MM-dd"), data2.ToString("yyyy-MM-dd"));
            }
            // Se a data2 for menor que a data1
            else
            {
                return (data2.ToString("yyyy-MM-dd"), data1.ToString("yyyy-MM-dd"));
            }
        }

        public static (string, string, string, string) ordenarDatasEObterTodasStrings(DateTime data1, DateTime data2)
        {
            // Se a data1 for menor ou igual a data2
            if (data1 <= data2)
            {
                return (data1.ToString("yyyy-MM-dd"), data1.ToString("dd/MM/yyyy"), data2.ToString("yyyy-MM-dd"), data2.ToString("dd/MM/yyyy"));
            }
            // Se a data2 for menor que a data1
            else
            {
                return (data2.ToString("yyyy-MM-dd"), data2.ToString("dd/MM/yyyy"), data1.ToString("yyyy-MM-dd"), data1.ToString("dd/MM/yyyy"));
            }
        }

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            // Verificar se a conta foi preenchida
            if (string.IsNullOrWhiteSpace(txtConta.Text))
            {
                MessageBox.Show("Selecione uma conta antes de gerar o relatório, dê um duplo clique na linha desejada.", "Uma conta não foi selecionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (nivel == "S")
            {
                MessageBox.Show("Não é possível gerar um razão analítico de uma conta sintética, por favor selecione uma conta analítica.", "Tipo de conta inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Obter datas
                var (dataInicial, dataInicialFormatada, dataFinal, dataFinalFormatada) = ordenarDatasEObterTodasStrings(dtpInicial.Value, dtpFinal.Value);

                // Consulta de lançamentos para a conta no período informado
                var sql = "SELECT l.data, h.historico, l.valor, l.saldo_atualizado FROM lancamentos l JOIN historicos h ON l.id_historico = h.id WHERE l.conta = @conta AND l.data BETWEEN @dataInicial AND @dataFinal ORDER BY l.data ASC, l.id ASC;";
                using (var comando = new SQLiteCommand(sql, con.conn))
                {
                    var conta = txtConta.Text;
                    comando.Parameters.AddWithValue("@conta", conta);
                    comando.Parameters.AddWithValue("@dataInicial", dataInicial);
                    comando.Parameters.AddWithValue("@dataFinal", dataFinal);

                    List<Lancamento> listLancamentos = new List<Lancamento>();

                    // Obter dados
                    using (SQLiteDataReader reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Lancamento lancamento = new Lancamento
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
                        comando.Parameters.AddWithValue("@conta", conta);
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

                                    // Função local para adicionar o rodapé de total do mês
                                    void adicionarRodapeMes(string mesAno, decimal totalDebitos, decimal totalCreditos)
                                    {
                                        // Adicionando parágrafos ao documento
                                        pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                        pdf.Add(new Paragraph($"{$"TOTAL DO MÊS: {mesAno}".PadLeft(68)}{totalDebitos.ToString("#,##0.00").PadLeft(14)}{totalCreditos.ToString("#,##0.00").PadLeft(14)}{(totalCreditos + totalDebitos).ToString("#,##0.00").PadLeft(14)}", fonte));
                                        pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));

                                        // Contar linhas usadas após adição do cabeçalho
                                        linhasDisponiveis -= 3;
                                    }

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

                                        // Verificar quantas linhas serão necessárias para cada uso - Não contar o espaço entre as colunas
                                        var linhasNecessarias = lancamento.historico.Length >= 57 ? 2 : 1;

                                        // Verificar se há linhas nessa página para incluir os registros, caso não haja: criar nova página com o cabeçalho
                                        if ((linhasDisponiveis - linhasNecessarias) < 0)
                                        {
                                            pdf.NewPage();
                                            linhasDisponiveis = 57;
                                            adicionarCabecalho(subtitulo);
                                        }

                                        // Iniciar criação de linha
                                        var linha = new StringBuilder();

                                        // Obter data desse lançamento
                                        var dataLancamento = data.ToString("dd/MM/yyyy");

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
                                            linhasDisponiveis -= 1;
                                        }
                                    }
                                    // Verificar se existe espaço para o rodapé, senão: criar nova página
                                    if ((linhasDisponiveis - 3) < 0)
                                    {
                                        pdf.NewPage();
                                        linhasDisponiveis = 57;
                                        adicionarCabecalho(subtitulo);
                                    }

                                    // Verificar se existe local para um espaço antes do rodapé
                                    if ((linhasDisponiveis - 4) >= 0)
                                    {
                                        pdf.Add(new Paragraph($"   ", fonte));
                                    }

                                    // Inserindo rodapé
                                    pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                    pdf.Add(new Paragraph($"{$"TOTAL DO PERÍODO: {dataInicialFormatada} A {dataFinalFormatada}".PadLeft(68)}{totalDebitos.ToString("#,##0.00").PadLeft(14)}{totalCreditos.ToString("#,##0.00").PadLeft(14)}{(totalCreditos + totalDebitos).ToString("#,##0.00").PadLeft(14)}", fonte));
                                    pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));

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
    }
}
