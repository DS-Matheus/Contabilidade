using Contabilidade.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using System.Text;
using System.Data.SQLite;
using static System.Windows.Forms.LinkLabel;

namespace Contabilidade.Forms.Relatorios
{
    public partial class frmRelDiario : Form
    {
        Conexao con;
        public frmRelDiario(Conexao conexaoBanco)
        {
            InitializeComponent();

            con = conexaoBanco;
        }

        private class Lancamento
        {
            public string Conta { get; set; }
            public string Descricao { get; set; }
            public string Historico { get; set; }
            public decimal Valor { get; set; }
        }

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            var sql = "SELECT l.conta, c.descricao, h.historico, l.valor FROM lancamentos l JOIN contas c ON l.conta = c.conta JOIN historicos h ON l.id_historico = h.id WHERE date(l.data) = @data ORDER BY l.conta ASC, l.id ASC;";
            var comando = new SQLiteCommand(sql, con.conn);

            // Obter data selecionada
            DateTime data = dtpData.Value;
            comando.Parameters.AddWithValue("@data", data.ToString("yyyy-MM-dd"));

            // Ler valores de lançamentos
            List<Lancamento> listLancamentos = new List<Lancamento>();

            using (var reader = comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    Lancamento lancamento = new Lancamento
                    {
                        Conta = reader["conta"].ToString(),
                        Descricao = reader["descricao"].ToString(),
                        Historico = reader["historico"].ToString(),
                        Valor = Convert.ToDecimal(reader["valor"])
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
                                subtitulo = Contabilidade.Forms.Relatorios.frmSaldo.CentralizarString(txtSubtitulo.Text, 110);
                            }

                            // Função local para adicionar o cabeçalho
                            void adicionarCabecalho(string subtitulo)
                            {
                                // Adicionando parágrafos ao documento
                                pdf.Add(new Paragraph($"                                         RELATÓRIO DIÁRIO - {dataFormatada}                             PÁGINA: {(pdf.PageNumber + 1).ToString("D3")}", fonte));
                                pdf.Add(new Paragraph($"{subtitulo}", fonte));
                                pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                pdf.Add(new Paragraph("CONTA           DESCRIÇÃO                                                                                     ", fonte));
                                pdf.Add(new Paragraph("HISTÓRICO                                                                                DÉBITOS      CRÉDITOS", fonte));
                                pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));

                                // Contar linhas usadas após adição do cabeçalho
                                linhasDisponiveis -= 6;
                            };

                            // Adicionar cabeçalho da primeira página
                            adicionarCabecalho(subtitulo);

                            // Variáveis para controle
                            string contaAnterior = "";
                            decimal totalDebitos = 0;
                            decimal totalCreditos = 0;

                            // Obter nome da conta referente ao caixa
                            comando.CommandText = "SELECT descricao FROM contas WHERE conta = '0';";
                            var descricaoCaixa = comando.ExecuteScalar()?.ToString();

                            // Obter saldo anterior e atual do caixa (ou deixar como 0 se não possuir nenhum registro na data informada ou antes dela)
                            comando.CommandText = "SELECT saldo FROM registros_caixa WHERE data <= @data ORDER BY data DESC LIMIT 2;";

                            decimal saldoAtual = 0;
                            decimal saldoAnterior = 0;
                            using (var reader2 = comando.ExecuteReader())
                            {
                                if (reader2.Read())
                                {
                                    saldoAtual = reader2.GetDecimal(0);
                                }

                                if (reader2.Read())
                                {
                                    saldoAnterior = reader2.GetDecimal(0);
                                }
                            }

                            // Adicionar linha para a conta do caixa
                            pdf.Add(new Paragraph($"0 - {descricaoCaixa}", fonte));

                            totalCreditos += saldoAnterior;
                            totalDebitos -= saldoAtual;

                            // Adicionar saldo anterior (crédito) e saldo atual (débito) do caixa
                            pdf.Add(new Paragraph($"{"(+) SALDO ANTERIOR".PadRight(82)}{saldoAnterior.ToString("#,##0.00").PadLeft(28)}", fonte));
                            pdf.Add(new Paragraph($"{"(-) SALDO ATUAL".PadRight(82)}{saldoAtual.ToString("#,##0.00").PadLeft(14)}", fonte));

                            linhasDisponiveis -= 3;

                            // Para cada lançamento
                            foreach (var lancamento in listLancamentos)
                            {
                                // Obter dados
                                var (conta, descricao, historico, valor) = (lancamento.Conta, lancamento.Descricao, lancamento.Historico, lancamento.Valor);

                                // Dividir considerando o tamanho máximo que pode ter
                                var linhasHistorico = Contabilidade.Forms.Relatorios.frmSaldo.QuebrarLinhaString(historico, 82);

                                // Verificar quantas linhas serão necessárias para cada uso - Não contar o espaço entre as colunas
                                var linhasNecessariasHistorico = lancamento.Historico.Length >= 82 ? linhasHistorico.Count : 1;
                                var linhasNecessarias = linhasNecessariasHistorico;

                                // Declarar para atribuir posteriormente, caso necessário
                                var linhasNecessariasDescricao = 0;
                                int espacosDescricao = 0;

                                // Verificar última conta inserida
                                if (contaAnterior != conta)
                                {
                                    espacosDescricao = 82 - conta.Length - 3;

                                    // Contabilizar linhas de identificação
                                    linhasNecessariasDescricao = lancamento.Descricao.Length > espacosDescricao ? 2 : 1;
                                    // Linhas necessárias para descricão + espaço superior
                                    linhasNecessarias += linhasNecessariasDescricao + 1;
                                }

                                // Verificar se há linhas nessa página para incluir os registros, caso não haja: criar nova página com o cabeçalho
                                if ((linhasDisponiveis - linhasNecessarias) < 0)
                                {
                                    pdf.NewPage();
                                    linhasDisponiveis = 57;
                                    adicionarCabecalho(subtitulo);
                                }

                                // Verificar novamente se é a mesma conta ou não (através do número de linhas para evitar comparar uma string inteira novamente)
                                if (linhasNecessariasDescricao != 0)
                                {
                                    // Atualizar conta atual
                                    contaAnterior = conta;

                                    // Adicionar linha vázia para separar da conta anterior / cabeçalho
                                    pdf.Add(new Paragraph("   ", fonte));

                                    // Verificar quantas linhas serão necessárias para a identificação da conta
                                    if (linhasNecessariasDescricao == 2)
                                    {
                                        // Dividir a descrição considerando o tamanho máximo que pode ter
                                        var linhasDescricao = Contabilidade.Forms.Relatorios.frmSaldo.QuebrarLinhaString(descricao, espacosDescricao);

                                        // Adicionar primeira linha
                                        pdf.Add(new Paragraph($"{conta} - {linhasDescricao[0]}", fonte));

                                        // Adicionar segunda linha (com espaço vázio referente a conta)
                                        pdf.Add(new Paragraph($"{"   ".PadRight(conta.Length + 3)}{linhasDescricao[1]}", fonte));

                                        // Contabilizar linhas
                                        linhasDisponiveis -= 2;
                                    }
                                    else
                                    {
                                        // Linha para identificação da conta
                                        pdf.Add(new Paragraph($"{conta} - {descricao}", fonte));

                                        // Contabilizar linha
                                        linhasDisponiveis -= 1;
                                    }
                                }

                                // Testar se serão necessárias 1 ou mais linhas por causa do comprimento do histórico
                                if (linhasNecessariasHistorico >= 2)
                                {
                                    // Verificar se é um débito/crédito
                                    // Crédito
                                    if (valor > 0)
                                    {
                                        // Contabilizar valor do lançamento
                                        totalCreditos += valor;

                                        // Adicionar primeira linha (com os espaços do débito vázios)
                                        pdf.Add(new Paragraph($"{linhasHistorico[0]?.PadRight(82)}{"   "?.PadLeft(14)}{valor.ToString("#,##0.00").PadLeft(14)}", fonte));
                                    }
                                    // Débito
                                    else
                                    {
                                        // Contabilizar valor do lançamento
                                        totalDebitos += valor;

                                        // Adicionar primeira linha
                                        pdf.Add(new Paragraph($"{linhasHistorico[0]?.PadRight(82)}{valor.ToString("#,##0.00").PadLeft(14)}", fonte));
                                    }

                                    // Remover primeiro item da lista e contabilizar sua adição no pdf
                                    linhasHistorico.RemoveAt(0);
                                    linhasDisponiveis -= 1;

                                    // Adicionar demais linhas
                                    foreach (var linha in linhasHistorico) {
                                        pdf.Add(new Paragraph(linha, fonte));

                                        // Contabilizar linha
                                        linhasDisponiveis -= 1;
                                    }
                                }
                                else
                                {
                                    // Verificar se é um débito/crédito
                                    // Crédito
                                    if (valor > 0)
                                    {
                                        // Contabilizar valor do lançamento
                                        totalCreditos += valor;

                                        // Adicionar primeira linha (com os espaços do débito vázios)
                                        pdf.Add(new Paragraph($"{historico?.PadRight(82)}{"   "?.PadLeft(14)}{valor.ToString("#,##0.00").PadLeft(14)}", fonte));
                                    }
                                    // Débito
                                    else
                                    {
                                        // Contabilizar valor do lançamento
                                        totalDebitos += valor;

                                        // Adicionar primeira linha
                                        pdf.Add(new Paragraph($"{historico?.PadRight(82)}{valor.ToString("#,##0.00").PadLeft(14)}", fonte));
                                    }

                                    // Contabilizar linha
                                    linhasDisponiveis -= 1;
                                }
                            }
                            // Verificar se existe espaço para o rodapé, senão: criar nova página
                            if (linhasDisponiveis < 2)
                            {
                                pdf.NewPage();
                                linhasDisponiveis = 57;
                                adicionarCabecalho(subtitulo);
                            }

                            // Verificar se existe linhas sobrando para inserir um espaço antes do rodapé
                            if (linhasDisponiveis >= 3)
                            {
                                pdf.Add(new Paragraph($"   ", fonte));
                                linhasDisponiveis--;
                            }

                            // Inserindo rodapé
                            pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                            pdf.Add(new Paragraph($"{"TOTAL DO DIA:".PadLeft(82)}{totalDebitos.ToString("#,##0.00").PadLeft(14)}{totalCreditos.ToString("#,##0.00").PadLeft(14)}", fonte));
                            linhasDisponiveis -= 2;
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
                MessageBox.Show($"Nenhum lançamento foi realizado na data {data:dd}/{data:MM}/{data:yyyy}", "O relatório não foi gerado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void txtSubtitulo_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verifica se a tecla pressionada é Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Impede a quebra de linha
                e.Handled = true;

                btnVisualizar.PerformClick();
            }
        }
    }
}
