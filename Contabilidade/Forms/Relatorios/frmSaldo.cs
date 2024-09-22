﻿using Contabilidade.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;
using System.Diagnostics;
using System.Drawing.Printing;

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
            public string Conta { get; set; }
            public string Descricao { get; set; }
            public decimal Saldo { get; set; }
        }

        // Função para dividir a descrição
        public static string[] DividirDescricao(string descricao, int limiteComEspaco)
        {
            string[] palavras = descricao.Split(' ');
            StringBuilder linha1 = new StringBuilder();
            StringBuilder linha2 = new StringBuilder();
            int comprimentoAtual = 0;

            // Para cada palavra testar
            foreach (string palavra in palavras)
            {
                // Se ao adicionar mais uma palavra o tamanho for menor ou igual ao limite: adicionar com espaço
                if ((comprimentoAtual + palavra.Length + 1) <= limiteComEspaco)
                {
                    linha1.Append(palavra + " ");
                    comprimentoAtual += palavra.Length + 1;
                }
                // Senão adicionar na linha 2
                else
                {
                    // Travar a inserção de palavras muito pequenas na linha 1
                    comprimentoAtual = limiteComEspaco;
                    linha2.Append(palavra + " ");
                }
            }

            return new string[] { linha1.ToString().PadRight(limiteComEspaco), linha2.ToString().PadRight(limiteComEspaco) };
        }

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            // Verificar se a conta foi preenchida
            if (string.IsNullOrWhiteSpace(txtConta.Text))
            {
                MessageBox.Show("Selecione uma conta antes de gerar o relatório, dê um duplo clique na linha desejada.", "Uma conta não foi selecionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Criar comando para obter dados das contas
                var sql = "";
                if (nivel == "S")
                {
                    sql = "SELECT c.conta, c.descricao, COALESCE((SELECT l.saldo_atualizado FROM lancamentos l WHERE c.conta = l.conta AND l.data <= @data ORDER BY l.data DESC, l.id DESC LIMIT 1), 0) AS saldo_atualizado FROM contas c WHERE c.conta LIKE @conta || '.%';";
                }
                else
                {
                    sql = "SELECT c.conta, c.descricao, COALESCE((SELECT l.saldo_atualizado FROM lancamentos l WHERE c.conta = l.conta AND l.data <= @data ORDER BY l.data DESC, l.id DESC LIMIT 1), 0) AS saldo_atualizado FROM contas c WHERE c.conta = @conta;";
                }
                var comando = new SQLiteCommand(sql, con.conn);

                var data = dtpData.Value;
                comando.Parameters.AddWithValue("@data", data);
                comando.Parameters.AddWithValue("@conta", txtConta.Text);

                // Executar comando e obter os dados
                using (SQLiteDataReader reader = comando.ExecuteReader())
                {
                    List<Lancamento> listLancamentos = new List<Lancamento>();

                    // Criar uma lista com todos os dados
                    while (reader.Read())
                    {
                        Lancamento lancamento = new Lancamento
                        {
                            Conta = reader["conta"].ToString(),
                            Descricao = reader["descricao"].ToString(),
                            Saldo = Convert.ToDecimal(reader["saldo_atualizado"])
                        };
                        listLancamentos.Add(lancamento);
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
                            Document pdf = new Document(PageSize.A4, 25, 25, 25, 25); // Margens padrão (36 pontos)

                            // Caminho do arquivo de fonte Consolas
                            string fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fontes", "consola.ttf");

                            // Verifica se o arquivo de fonte existe
                            if (!File.Exists(fontPath))
                            {
                                Console.Error.WriteLine("Arquivo de fonte não encontrado, faça backup dos bancos de dados e reinstale o programa");
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
                                string subtitulo = "";
                                if (!string.IsNullOrWhiteSpace(txtSubtitulo.Text))
                                {
                                    subtitulo = txtSubtitulo.Text;

                                    int espacosTotais = 110 - subtitulo.Length;
                                    int espacosAntes = (int)Math.Ceiling(espacosTotais / 2.0);
                                    int espacosDepois = espacosTotais - espacosAntes;

                                    subtitulo = $"{"".PadRight(espacosAntes)}{subtitulo}{"".PadRight(espacosDepois)}";
                                }

                                // Função local para adicionar o cabeçalho
                                void adicionarCabecalho(string subtitulo)
                                {
                                    // Adicionando parágrafos ao documento
                                    pdf.Add(new Paragraph($"                                       LISTAGEM DE SALDOS - {dataFormatada}                             PÁGINA: {(pdf.PageNumber + 1).ToString("D3")}", fonte));
                                    pdf.Add(new Paragraph($"{subtitulo.PadLeft(110)}", fonte));
                                    pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                    pdf.Add(new Paragraph("CONTA           DESCRIÇÃO                                                                                SALDO", fonte));
                                    pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                    pdf.Add(new Paragraph("", fonte));

                                    // Contar linhas usadas após adição do cabeçalho
                                    linhasDisponiveis -= 6;
                                };

                                // Adicionar cabeçalho da primeira página
                                adicionarCabecalho(subtitulo);

                                // Para cada lançamento
                                foreach (var lancamento in listLancamentos)
                                {
                                    // Obter dados
                                    var (conta, descricao, saldo) = (lancamento.Conta, lancamento.Descricao, lancamento.Saldo);

                                    // Verificar quantas linhas serão necessárias para o lançamento - Não contar o espaço entre as colunas
                                    var linhasNecessarias = lancamento.Descricao.Length >= 80 ? 2 : 1;

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
                                        // Dividir considerando o tamanho máximo que pode ter + 1 de espaço para a outra coluna
                                        string[] linhasDescricao = DividirDescricao(descricao, 81);

                                        linha.Append(linhasDescricao[0]);
                                        // Por ser a última coluna, não considerar o espaçamento no tamanho limite
                                        linha.Append(saldo.ToString("#,##0.00").PadLeft(13));

                                        // Adicionar primeira linha
                                        pdf.Add(new Paragraph(linha.ToString(), fonte));

                                        // Limpar o StringBuilder e iniciar a criação da segunda linha (com conta e saldos vázios).
                                        linha.Clear();

                                        // Espaço vázio referente a conta
                                        linha.Append("".ToString().PadRight(16));

                                        // Adicionar segunda linha
                                        linha.Append(linhasDescricao[1]);
                                        pdf.Add(new Paragraph(linha.ToString(), fonte));

                                        // Contabilizar linhas
                                        linhasDisponiveis -= 2;
                                    }
                                    else
                                    {
                                        // Considerar o limite + 1 de espaçamento entre as colunas
                                        linha.Append(descricao.PadRight(81));
                                        linha.Append(saldo.ToString("#,##0.00").PadLeft(13));

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
            }
        }
    }
}
