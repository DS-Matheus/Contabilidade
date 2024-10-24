using Contabilidade.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Data;
using System.Text;
using System.Diagnostics;
using Microsoft.Data.Sqlite;
using static System.Windows.Forms.LinkLabel;

namespace Contabilidade.Forms.Relatorios
{
    public partial class frmSaldo : Form
    {
        Conexao con;
        static DataTable dtDados = new DataTable();
        DataView dv = dtDados.DefaultView;
        private string nivel = "";
        public frmSaldo(Conexao conaxaoBanco)
        {
            InitializeComponent();

            con = conaxaoBanco;

            // Defina a propriedade MaxDate para az data atual
            dtpData.MaxDate = DateTime.Today;

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

        private void txtFiltrar2_TextChanged(object sender, EventArgs e)
        {
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
            public decimal saldo { get; set; }
        }

        // Função para dividir a descrição
        public static List<string> QuebrarLinhaString(string texto, int limiteCaracteres)
        {
            string[] palavras = texto.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> linhas = new List<string>();
            StringBuilder linhaAtual = new StringBuilder();
            int comprimentoAtual = 0;

            foreach (string palavra in palavras)
            {
                var comprimentoEsperado = comprimentoAtual + palavra.Length;
                if (comprimentoEsperado <= limiteCaracteres)
                {
                    if ((comprimentoEsperado + 1) <= limiteCaracteres)
                    {
                        linhaAtual.Append(palavra + " ");
                        comprimentoAtual += palavra.Length + 1;
                    }
                    else
                    {
                        linhaAtual.Append(palavra);
                        comprimentoAtual = limiteCaracteres;
                    }
                }
                else
                {
                    linhas.Add(linhaAtual.ToString().Trim());
                    linhaAtual.Clear();
                    linhaAtual.Append(palavra + " ");
                    comprimentoAtual = palavra.Length + 1;
                }
            }

            linhas.Add(linhaAtual.ToString().Trim());

            return linhas;
        }


        public static string CentralizarString(string texto, int limite)
        {
            int espacosTotais = limite - texto.Length;
            int espacosAntes = (int)Math.Ceiling(espacosTotais / 2.0);
            int espacosDepois = espacosTotais - espacosAntes;

            texto = $"{"".PadRight(espacosAntes)}{texto}{"".PadRight(espacosDepois)}";
            return texto;
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
                    var incluirCaixa = chkCaixa.Checked;
                    var incluirSaldosZero = chkSaldosZero.Checked;

                    // Criar comando para obter dados das contas
                    var sql = "";
                    if (nivel == "S")
                    {
                        // Verificar se deve incluir as contas com saldo 0
                        if (incluirSaldosZero)
                        {
                            sql = "SELECT c.conta, c.descricao, COALESCE((SELECT l.saldo FROM lancamentos l WHERE c.conta = l.conta AND l.data <= @data ORDER BY l.data DESC, l.id DESC LIMIT 1), 0) AS saldo FROM contas c WHERE c.conta LIKE @conta || '.%';";
                        }
                        // Caso não deva incluir
                        else
                        {
                            sql = "WITH contas_saldo AS (SELECT c.conta, c.descricao, COALESCE((SELECT l.saldo FROM lancamentos l WHERE c.conta = l.conta AND l.data <= @data ORDER BY l.data DESC, l.id DESC LIMIT 1), 0) AS saldo FROM contas c WHERE c.conta LIKE @conta || '.%') SELECT * FROM contas_saldo WHERE saldo != 0;";
                        }
                    }
                    else
                    {
                        sql = "SELECT c.conta, c.descricao, COALESCE((SELECT l.saldo FROM lancamentos l WHERE c.conta = l.conta AND l.data <= @data ORDER BY l.data DESC, l.id DESC LIMIT 1), 0) AS saldo FROM contas c WHERE c.conta = @conta;";
                    }
                    var comando = new SqliteCommand(sql, con.conn);

                    var data = dtpData.Value;
                    comando.Parameters.AddWithValue("@data", data);
                    comando.Parameters.AddWithValue("@conta", txtConta.Text);

                    List<Lancamento> listLancamentos = new List<Lancamento>();

                    // Executar comando e obter os dados
                    using (var reader = comando.ExecuteReader())
                    {
                        // Criar uma lista com todos os dados
                        while (reader.Read())
                        {
                            Lancamento lancamento = new Lancamento
                            {
                                conta = reader["conta"].ToString(),
                                descricao = reader["descricao"].ToString(),
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
                                    var fonte = new iTextSharp.text.Font(bf, 9);

                                    // Obter data formatada                    
                                    string dataFormatada = $"{data:dd}/{data:MM}/{data:yyyy}";
                                    var linhasDisponiveis = 57;

                                    // Operações com o subtítulo
                                    string subtitulo = "   ";
                                    if (!string.IsNullOrWhiteSpace(txtSubtitulo.Text))
                                    {
                                        subtitulo = CentralizarString(txtSubtitulo.Text, 110);
                                    }

                                    // Função local para adicionar o cabeçalho
                                    void adicionarCabecalho(string subtitulo)
                                    {
                                        // Adicionando parágrafos ao documento
                                        pdf.Add(new Paragraph($"                                       LISTAGEM DE SALDOS - {dataFormatada}                             PÁGINA: {(pdf.PageNumber + 1).ToString("D3")}", fonte));
                                        pdf.Add(new Paragraph($"{subtitulo}", fonte));
                                        pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                        pdf.Add(new Paragraph("CONTA           DESCRIÇÃO                                                                                SALDO", fonte));
                                        pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                        pdf.Add(new Paragraph("    ", fonte));

                                        // Contar linhas usadas após adição do cabeçalho
                                        linhasDisponiveis -= 6;
                                    };

                                    // Adicionar cabeçalho da primeira página
                                    adicionarCabecalho(subtitulo);

                                    // Verificar se deve incluir ou não o registro do caixa
                                    if (incluirCaixa)
                                    {
                                        // Obter nome da conta referente ao caixa
                                        comando.CommandText = "SELECT descricao FROM contas WHERE conta = '0';";
                                        var descricaoCaixa = comando.ExecuteScalar()?.ToString();

                                        // Obter saldo no dia informado
                                        comando.CommandText = "SELECT COALESCE((SELECT saldo FROM registros_caixa WHERE data <= @data ORDER BY data DESC LIMIT 1), 0);";
                                        var saldoCaixa = Convert.ToDecimal(comando.ExecuteScalar());

                                        // Verificar se a descrição do caixa precisa de uma segunda linha (no caso de ser muito grande)
                                        if (descricaoCaixa?.Length >= 80)
                                        {
                                            // Dividir considerando o tamanho máximo que pode ter (sem contar o espaço para a outra coluna)
                                            var linhasDescricao = QuebrarLinhaString(descricaoCaixa, 80);

                                            // Adicionar primeira linha
                                            pdf.Add(new Paragraph($"{"0".PadRight(16)}{linhasDescricao[0].PadRight(80)}{saldoCaixa.ToString("#,##0.00").PadLeft(14)}", fonte));

                                            // Adicionar segunda linha
                                            pdf.Add(new Paragraph($"{"    ".ToString().PadRight(16)}{linhasDescricao[1]}", fonte));

                                            // Contabilizar linhas
                                            linhasDisponiveis -= 2;
                                        }
                                        else
                                        {
                                            pdf.Add(new Paragraph($"{"0".PadRight(16)}{descricaoCaixa?.PadRight(80)}{saldoCaixa.ToString("#,##0.00").PadLeft(14)}", fonte));

                                            // Contabilizar linha
                                            linhasDisponiveis -= 1;
                                        }
                                    }

                                    // Para cada lançamento
                                    foreach (var lancamento in listLancamentos)
                                    {
                                        // Obter dados
                                        var (conta, descricao, saldo) = (lancamento.conta, lancamento.descricao, lancamento.saldo);

                                        // Verificar quantas linhas serão necessárias para o lançamento - Não contar o espaço entre as colunas
                                        var linhasNecessarias = lancamento.descricao.Length >= 80 ? 2 : 1;

                                        // Verificar se há linhas nessa página para incluir o lançamento, caso não haja: criar nova página com cabeçalho
                                        if ((linhasDisponiveis - linhasNecessarias) < 0)
                                        {
                                            pdf.NewPage();
                                            linhasDisponiveis = 57;
                                            adicionarCabecalho(subtitulo);
                                        }

                                        // Iniciar criação da linha
                                        var linha = new StringBuilder();

                                        // Tamanho da conta com todos dígitos e pontuações + 1 de espaço para a outra coluna
                                        linha.Append(conta.PadRight(16));

                                        // Testar se serão necessárias 1 ou 2 linhas por causa do comprimento da descrição
                                        if (linhasNecessarias == 2)
                                        {
                                            // Dividir considerando o tamanho máximo que pode ter (sem contar o espaço para a outra coluna)
                                            var linhasDescricao = QuebrarLinhaString(descricao, 80);

                                            linha.Append(linhasDescricao[0]);
                                            // Espaçamento do saldo + 1 para a divisão entre as colunas
                                            linha.Append(saldo.ToString("#,##0.00").PadLeft(14));

                                            // Adicionar primeira linha
                                            pdf.Add(new Paragraph(linha.ToString(), fonte));

                                            // Limpar o StringBuilder e iniciar a criação da segunda linha (com conta e saldos vázios).
                                            linha.Clear();

                                            // Espaço vázio referente a conta
                                            linha.Append("    ".ToString().PadRight(16));

                                            // Adicionar segunda linha
                                            linha.Append(linhasDescricao[1]);
                                            pdf.Add(new Paragraph(linha.ToString(), fonte));

                                            // Contabilizar linhas
                                            linhasDisponiveis -= 2;
                                        }
                                        else
                                        {
                                            linha.Append(descricao.PadRight(80));
                                            // Espaçamento do saldo + 1 para a divisão entre as colunas
                                            linha.Append(saldo.ToString("#,##0.00").PadLeft(14));

                                            pdf.Add(new Paragraph(linha.ToString(), fonte));

                                            // Contabilizar linha
                                            linhasDisponiveis -= 1;
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
                    // Senão: informar ao usuário
                    else
                    {
                        if (nivel == "S")
                        {
                            MessageBox.Show($"A conta sintética {txtConta.Text} não possuí nenhuma conta analítica registrada para consultar o saldo", "O relatório não foi gerado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else
                        {
                            MessageBox.Show($"Houve um erro ao buscar o saldo da conta analítica {txtConta.Text}, por favor contate o desenvolvedor.", "O relatório não foi gerado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao gerar o relatório", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
