﻿using Contabilidade.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Diagnostics;
using System.Text;
using Microsoft.Data.Sqlite;
using Contabilidade.Classes;

namespace Contabilidade.Forms.Relatorios
{
    public partial class frmBalanceteGeral : Form
    {
        Conexao con;
        public frmBalanceteGeral(Conexao conexaoBanco)
        {
            InitializeComponent();

            con = conexaoBanco;

            // Defina a propriedade MaxDate para a data atual
            dtpInicial.MaxDate = DateTime.Today;
            dtpFinal.MaxDate = DateTime.Today;
        }

        private class Lancamento
        {
            public string conta { get; set; }
            public string descricao { get; set; }
            public decimal debitos {  get; set; }
            public decimal creditos { get; set; }
            public decimal saldo { get; set; }
        }

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            try
            {
                // Obter datas
                var (dataInicial, dataInicialFormatada, dataFinal, dataFinalFormatada) = Contabilidade.Forms.Relatorios.frmRazaoAnalitico.ordenarDatasEObterStrings(dtpInicial.Value, dtpFinal.Value);

                // Consulta de dados no período informado
                var sql = "SELECT l.conta, c.descricao, SUM(CASE WHEN l.valor > 0 THEN l.valor ELSE 0 END) AS credito, SUM(CASE WHEN l.valor < 0 THEN l.valor ELSE 0 END) AS debito, (SELECT saldo FROM lancamentos WHERE conta = l.conta ORDER BY data DESC, id DESC LIMIT 1) AS saldo FROM lancamentos l JOIN contas c ON l.conta = c.conta WHERE l.data BETWEEN @dataInicial AND @dataFinal GROUP BY l.conta ORDER BY l.conta;";

                using (var comando = new SqliteCommand(sql, con.conn))
                {
                    comando.Parameters.AddWithValue("@dataInicial", dataInicial);
                    comando.Parameters.AddWithValue("@dataFinal", dataFinal);

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
                                        pdf.Add(new Paragraph($"BALANCETE GERAL                                PERÍODO: {dataInicialFormatada} A {dataFinalFormatada}                                PÁGINA: {(pdf.PageNumber + 1).ToString("D3")}", fonte));
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
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao gerar o relatório", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
