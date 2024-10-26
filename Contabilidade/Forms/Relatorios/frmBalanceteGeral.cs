﻿using Contabilidade.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Diagnostics;
using System.Text;
using Microsoft.Data.Sqlite;
using Contabilidade.Classes;
using static Contabilidade.Forms.Relatorios.frmBalanceteGeral;
using static System.Windows.Forms.LinkLabel;

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

        public class ContaSintetica
        {
            public string Conta { get; set; }
            public string Descricao { get; set; }
            public int Grau { get; set; }
            public decimal Debitos { get; set; }
            public decimal Creditos { get; set; }
            public decimal Saldo { get; set; }

            public ContaSintetica(string conta, string descricao, int grau)
            {
                this.Conta = conta;
                this.Descricao = descricao;
                this.Grau = grau;
                this.Debitos = 0;
                this.Creditos = 0;
                this.Saldo = 0;
            }
        }

        public class ContaAnalitica
        {
            public string Conta { get; set; }
            public string Descricao { get; set; }
            public decimal Debitos {  get; set; }
            public decimal Creditos { get; set; }
            public decimal Saldo { get; set; }
        }

        public static int verificarGrauConta(string conta)
        {
            return conta.Split('.').Length;
        }

        public static void DecomporContaAnalitica(string conta, List<ContaSintetica> listaContasSinteticas, Conexao conexao)
        {
            // Iterar enquanto a conta possuir ponto (não for a conta raiz = grau 1)
            while (conta.Contains("."))
            {
                int ultimoPonto = conta.LastIndexOf('.');
                conta = conta.Substring(0, ultimoPonto);
                int grau = verificarGrauConta(conta);

                // Verificar se a conta já existe na lista
                if (listaContasSinteticas.Any(cs => cs.Conta == conta))
                {
                    return; // Cancelar a função se a conta já existir na lista
                }

                // Obter descrição da conta
                var comandoSql = "SELECT descricao FROM contas WHERE conta = @conta;";
                using (var comando = new SqliteCommand(comandoSql, conexao.conn))
                {
                    comando.Parameters.AddWithValue("@conta", conta);
                    var descricao = comando.ExecuteScalar()?.ToString();
                    listaContasSinteticas.Add(new ContaSintetica(conta, descricao, grau));
                }
            }

            // Adicionar a conta de grau 1 (sem nenhum ponto)
            // Verificar se a conta já existe na lista
            if (listaContasSinteticas.Any(cs => cs.Conta == conta))
            {
                return; // Cancelar a função se a conta já existir na lista
            }

            // Obter descrição da conta
            var sql = "SELECT descricao FROM contas WHERE conta = @conta;";
            using (var comando = new SqliteCommand(sql, conexao.conn))
            {
                comando.Parameters.AddWithValue("@conta", conta);
                var descricao = comando.ExecuteScalar()?.ToString();
                listaContasSinteticas.Add(new ContaSintetica(conta, descricao, 1));
            }
        }

        public static int obterQuantidadeLinhasString(string descricao, int caracteresDisponiveis)
        {
            var tamanhoDescricao = descricao.Length;
            return tamanhoDescricao <= caracteresDisponiveis ? 1 : tamanhoDescricao <= caracteresDisponiveis * 2 ? 2 : 3;
        }

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            try
            {
                var incluirSaldosZero = chkSaldosZero.Checked;

                // Obter datas
                var (dataInicial, dataInicialFormatada, dataFinal, dataFinalFormatada) = Contabilidade.Forms.Relatorios.frmRazaoAnalitico.ordenarDatasEObterStrings(dtpInicial.Value, dtpFinal.Value);

                // Consulta de dados no período informado
                var sql = "";

                // Verificar se deve incluir as contas com saldo 0
                if (incluirSaldosZero)
                {
                    sql = "SELECT l.conta, c.descricao, SUM(CASE WHEN l.valor > 0 THEN l.valor ELSE 0 END) AS credito, SUM(CASE WHEN l.valor < 0 THEN l.valor ELSE 0 END) AS debito, COALESCE((SELECT saldo FROM lancamentos WHERE conta = l.conta ORDER BY data DESC, id DESC LIMIT 1), 0) AS saldo FROM lancamentos l JOIN contas c ON l.conta = c.conta WHERE l.data BETWEEN @dataInicial AND @dataFinal GROUP BY l.conta ORDER BY l.conta;";
                }
                // Caso não deva incluir
                else
                {
                    sql = "WITH dados_contas AS (SELECT l.conta, c.descricao, SUM(CASE WHEN l.valor > 0 THEN l.valor ELSE 0 END) AS credito, SUM(CASE WHEN l.valor < 0 THEN l.valor ELSE 0 END) AS debito, COALESCE((SELECT saldo FROM lancamentos WHERE conta = l.conta ORDER BY data DESC, id DESC LIMIT 1), 0) AS saldo FROM lancamentos l JOIN contas c ON l.conta = c.conta WHERE l.data BETWEEN @dataInicial AND @dataFinal GROUP BY l.conta ORDER BY l.conta) SELECT * FROM dados_contas WHERE saldo != 0;";
                }

                using (var comando = new SqliteCommand(sql, con.conn))
                {
                    comando.Parameters.AddWithValue("@dataInicial", dataInicial);
                    comando.Parameters.AddWithValue("@dataFinal", dataFinal);

                    List<ContaAnalitica> listLancamentos = new List<ContaAnalitica>();

                    // Obter dados
                    using (var reader = comando.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ContaAnalitica lancamento = new ContaAnalitica
                            {
                                Conta = reader["conta"].ToString(),
                                Descricao = reader["descricao"].ToString(),
                                Debitos = Convert.ToDecimal(reader["debito"]),
                                Creditos = Convert.ToDecimal(reader["credito"]),
                                Saldo = Convert.ToDecimal(reader["saldo"])
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

                                    var listContasSinteticas = new List<ContaSintetica>(); 

                                    // Para cada lançamento: obter as contas sintéticas
                                    foreach (var lancamento in listLancamentos)
                                    {
                                        DecomporContaAnalitica(lancamento.Conta, listContasSinteticas, con);
                                    }

                                    // Armazenar todos os registros em uma lista de objetos e ordenar
                                    var todasContas = listContasSinteticas.Cast<object>().Concat(listLancamentos)
                                        .OrderBy(c => c is ContaSintetica ? ((ContaSintetica)c).Conta : ((ContaAnalitica)c).Conta)
                                        .ThenBy(c => c is ContaSintetica ? 1 : 0)
                                        .ToList();

                                    // Pilha para armazenar as contas sintéticas "abertas"
                                    Stack<ContaSintetica> pilhaContas = new Stack<ContaSintetica>();

                                    void AdicionarParagrafosPdf(string conta, string descricao, decimal saldoAnterior, decimal debitos, decimal creditos, decimal saldo, int espacosInicio, int espacosDescricao, int linhasNecessarias)
                                    {
                                        // Obter linhas da descrição
                                        var linhasDescricao = Contabilidade.Forms.Relatorios.frmSaldo.QuebrarLinhaString(descricao, espacosDescricao);
                                        
                                        pdf.Add(new Paragraph($"{"".PadRight(espacosInicio)}{conta.PadRight(conta.Length)} - {linhasDescricao[0].PadRight(espacosDescricao)}{saldoAnterior.ToString("#,##0.00").PadLeft(14)}{debitos.ToString("#,##0.00").PadLeft(14)}{creditos.ToString("#,##0.00").PadLeft(14)}{saldo.ToString("#,##0.00").PadLeft(14)}", fonte));
                                        linhasDisponiveis -= 1;
                                        
                                        // Adicionar as outras linhas (se houver mais que uma)
                                        for (int i = 1; i < linhasNecessarias; i++)
                                        {
                                            pdf.Add(new Paragraph($"{"    ".PadRight(espacosInicio + conta.Length)}   {linhasDescricao[i].PadRight(espacosDescricao)}", fonte));
                                            linhasDisponiveis -= 1;
                                        }
                                    }

                                    void ProcessarContaFechada(ContaSintetica contaFechada)
                                    {
                                        // Obter dados e calcular espaços
                                        var grauContaRemover = verificarGrauConta(contaFechada.Conta);
                                        int espacosInicioRemover = 2 * (grauContaRemover - 1);
                                        int espacosDescricaoRemover = 66 - espacosInicioRemover - contaFechada.Conta.Length - 3;
                                        var saldoAnteriorRemover = contaFechada.Saldo - (contaFechada.Creditos + contaFechada.Debitos);
                                        var linhasNecessariasRemover = obterQuantidadeLinhasString(contaFechada.Descricao, espacosDescricaoRemover);

                                        // Verificar se a quantidade de linhas disponiveis é suficiente
                                        if ((linhasDisponiveis - linhasNecessariasRemover) < 0)
                                        {
                                            pdf.NewPage();
                                            linhasDisponiveis = 63;
                                            adicionarCabecalho(subtitulo);
                                        }

                                        AdicionarParagrafosPdf(contaFechada.Conta, contaFechada.Descricao, saldoAnteriorRemover, contaFechada.Debitos, contaFechada.Creditos, contaFechada.Saldo, espacosInicioRemover, espacosDescricaoRemover, linhasNecessariasRemover);
                                    }

                                    // Para cada conta
                                    foreach (var conta in todasContas)
                                    {
                                        // Verificar se é sintética ou analitica
                                        if (conta is ContaSintetica contaSintetica)
                                        {
                                            // Fechar contas de grau igual ou superior a esta
                                            while (pilhaContas.Count > 0 && pilhaContas.Peek().Grau >= contaSintetica.Grau)
                                            {
                                                ProcessarContaFechada(pilhaContas.Pop());
                                            }

                                            // Obter dados e espaçamentos a descrição
                                            var grauConta = verificarGrauConta(contaSintetica.Conta);
                                            int espacosInicio = 2 * (grauConta - 1);
                                            int espacosDescricao = 66 - espacosInicio - contaSintetica.Conta.Length - 3;
                                            var linhasNecessarias = obterQuantidadeLinhasString(contaSintetica.Descricao, espacosDescricao);

                                            // Verificar se a quantidade de linhas disponiveis é suficiente
                                            if ((linhasDisponiveis - linhasNecessarias) < 0)
                                            {
                                                pdf.NewPage();
                                                linhasDisponiveis = 63;
                                                adicionarCabecalho(subtitulo);
                                            }

                                            AdicionarParagrafosPdf(contaSintetica.Conta, contaSintetica.Descricao, 0, 0, 0, 0, espacosInicio, espacosDescricao, linhasNecessarias);
                                            // Adicionar a conta sintetica aberta a pilha de contas
                                            pilhaContas.Push(contaSintetica);
                                        }
                                        // Se for analitica
                                        else if (conta is ContaAnalitica lancamento)
                                        {
                                            // Obter dados e calcular espaços
                                            var grauConta = verificarGrauConta(lancamento.Conta);
                                            int espacosInicio = 2 * (grauConta - 1);
                                            int espacosDescricao = 66 - espacosInicio - lancamento.Conta.Length - 3;
                                            var saldoAnterior = lancamento.Saldo - (lancamento.Creditos + lancamento.Debitos);
                                            var linhasNecessarias = obterQuantidadeLinhasString(lancamento.Descricao, espacosDescricao);

                                            // Verificar se a quantidade de linhas disponiveis é suficiente
                                            if ((linhasDisponiveis - linhasNecessarias) < 0)
                                            {
                                                pdf.NewPage();
                                                linhasDisponiveis = 63;
                                                adicionarCabecalho(subtitulo);
                                            }

                                            AdicionarParagrafosPdf(lancamento.Conta, lancamento.Descricao, saldoAnterior, lancamento.Debitos, lancamento.Creditos, lancamento.Saldo, espacosInicio, espacosDescricao, linhasNecessarias);

                                            // Adicionar valores em cada conta sintética aberta
                                            foreach (var grupoAberto in pilhaContas)
                                            {
                                                grupoAberto.Debitos += lancamento.Debitos;
                                                grupoAberto.Creditos += lancamento.Creditos;
                                                grupoAberto.Saldo += lancamento.Saldo;
                                            }
                                        }
                                    }

                                    // Fechar contas sintéticas restantes
                                    while (pilhaContas.Count > 0)
                                    {
                                        ProcessarContaFechada(pilhaContas.Pop());
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
