using Contabilidade.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Diagnostics;
using System.Text;
using Microsoft.Data.Sqlite;

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
            var comando = new SqliteCommand(sql, con.conn);

            // Obter data selecionada
            DateTime data = dtpData.Value;
            comando.Parameters.AddWithValue("@data", data.ToString("yyyy-MM-dd"));

            using (var reader = comando.ExecuteReader())
            {
                List<Lancamento> listLancamentos = new List<Lancamento>();

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

                                // Para cada lançamento
                                foreach (var lancamento in listLancamentos)
                                {
                                    // Obter dados
                                    var (conta, descricao, historico, valor) = (lancamento.Conta, lancamento.Descricao, lancamento.Historico, lancamento.Valor);

                                    // Verificar quantas linhas serão necessárias para cada uso - Não contar o espaço entre as colunas
                                    var linhasNecessariasHistorico = lancamento.Historico.Length >= 82 ? 2 : 1;
                                    var linhasNecessarias = linhasNecessariasHistorico;

                                    // Declarar para atribuir posteriormente, caso necessário
                                    var linhasNecessariasDescricao = 0;

                                    // Verificar última conta inserida
                                    if (contaAnterior != conta)
                                    {
                                        // Contabilizar linhas de identificação
                                        linhasNecessariasDescricao = lancamento.Descricao.Length > 66 ? 2 : 1;
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

                                    // Iniciar criação de linha
                                    var linha = new StringBuilder();

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
                                            // Tamanho da conta com todos dígitos e pontuações + 1 de espaço para a outra coluna
                                            linha.Append(conta.PadRight(16));

                                            // Dividir a descrição considerando o tamanho máximo que pode ter
                                            string[] linhasDescricao = Contabilidade.Forms.Relatorios.frmSaldo.QuebrarLinhaString(descricao, 66);
                                            linha.Append(linhasDescricao[0]);

                                            // Adicionar primeira linha
                                            pdf.Add(new Paragraph(linha.ToString(), fonte));

                                            // Limpar o StringBuilder e iniciar a criação da segunda linha
                                            linha.Clear();

                                            // Espaço vázio referente a conta
                                            linha.Append("   ".PadRight(16));

                                            // Adicionar segunda linha
                                            linha.Append(linhasDescricao[1]);
                                            pdf.Add(new Paragraph(linha.ToString(), fonte));

                                            // Contabilizar linhas
                                            linhasDisponiveis -= 2;
                                        }
                                        else
                                        {
                                            // Linha para identificação da conta
                                            pdf.Add(new Paragraph($"{conta.PadRight(16)}{descricao}", fonte));

                                            // Contabilizar linha
                                            linhasDisponiveis -= 1;
                                        }
                                    }

                                    // Limpar StringBuilder para a criação da linha de lançamento
                                    linha.Clear();

                                    // Testar se serão necessárias 1 ou 2 linhas por causa do comprimento do histórico
                                    if (linhasNecessariasHistorico == 2)
                                    {
                                        // Dividir considerando o tamanho máximo que pode ter
                                        string[] linhasHistorico = Contabilidade.Forms.Relatorios.frmSaldo.QuebrarLinhaString(historico, 82);
                                        linha.Append(linhasHistorico[0]);

                                        // Verificar se é um débito/crédito
                                        // Crédito
                                        if (valor > 0)
                                        {
                                            // Contabilizar valor do lançamento
                                            totalCreditos += valor;

                                            // String composta de caracteres vázios para ocupar o espaço do débito
                                            linha.Append("   ".PadLeft(14));
                                            // Espaçamento do crédito + 1 para a divisão entre as colunas
                                            linha.Append(valor.ToString("#,##0.00").PadLeft(14));
                                        }
                                        // Débito
                                        else
                                        {
                                            // Contabilizar valor do lançamento
                                            totalDebitos += valor;

                                            // Espaçamento do débito + 1 para a divisão entre as colunas
                                            linha.Append(valor.ToString("#,##0.00").PadLeft(14));
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
                                        linha.Append(historico.PadRight(82));

                                        // Verificar se é um débito/crédito
                                        // Crédito
                                        if (valor > 0)
                                        {
                                            // Contabilizar valor do lançamento
                                            totalCreditos += valor;

                                            // String composta de caracteres vázios para ocupar o espaço do débito
                                            linha.Append(valor.ToString("   ".PadLeft(14)));
                                            // Espaçamento do crédito + 1 para a divisão entre as colunas
                                            linha.Append(valor.ToString("#,##0.00").PadLeft(14));
                                        }
                                        // Débito
                                        else
                                        {
                                            // Contabilizar valor do lançamento
                                            totalDebitos += valor;

                                            // Espaçamento do débito + 1 para a divisão entre as colunas
                                            linha.Append(valor.ToString("#,##0.00").PadLeft(14));
                                        }

                                        pdf.Add(new Paragraph(linha.ToString(), fonte));

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
