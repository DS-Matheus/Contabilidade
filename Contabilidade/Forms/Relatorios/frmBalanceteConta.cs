﻿using Contabilidade.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Data;
using System.Diagnostics;
using System.Data.SQLite;
using Contabilidade.Classes;
using Contabilidade.Forms.Cadastros;

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

            txtFiltrar.Select();
        }

        private void atualizarDataGrid()
        {
            // Query de pesquisa
            string sql = "SELECT * FROM contas WHERE conta != '0' ORDER BY conta;";
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

        private void dgvContas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvContas.Rows[e.RowIndex];
                txtConta.Text = row.Cells["Conta"].Value.ToString();
                nivel = row.Cells["Nível"].Value.ToString();
            }
        }

        private class ContaAnalitica
        {
            public string Conta { get; set; }
            public string Descricao { get; set; }
            public decimal Debitos { get; set; }
            public decimal Creditos { get; set; }
            public decimal Saldo { get; set; }
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
                // Verificar se a conta selecionada é o caixa (caso ela apareça, de alguma forma)
                else if (txtConta.Text == "0")
                {
                    MessageBox.Show("Não é possível gerar um relatório desse tipo para o caixa, por favor, selecione as outras contas.", "Uma conta válida não foi selecionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    var incluirSaldosZero = chkSaldosZero.Checked;

                    // Obter datas
                    var (dataInicial, dataInicialFormatada, dataFinal, dataFinalFormatada) = Contabilidade.Forms.Relatorios.frmRazaoAnalitico.ordenarDatasEObterStringsFormatadas(dtpInicial.Value, dtpFinal.Value);

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

                    using (var comando = new SQLiteCommand(sql, con.conn))
                    {
                        comando.Parameters.AddWithValue("@dataInicial", dataInicial);
                        comando.Parameters.AddWithValue("@dataFinal", dataFinal);
                        comando.Parameters.AddWithValue("@conta", txtConta.Text);

                        List<ContaAnalitica> listContasAnaliticas = new List<ContaAnalitica>();

                        // Obter dados
                        using (var reader = comando.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ContaAnalitica contaAnalitica = new ContaAnalitica
                                {
                                    Conta = reader["conta"].ToString(),
                                    Descricao = reader["descricao"].ToString(),
                                    Debitos = (Convert.ToInt32(reader["debito"]) / 100m),
                                    Creditos = (Convert.ToInt32(reader["credito"]) / 100m),
                                    Saldo = (Convert.ToInt32(reader["saldo"]) / 100m)
                                };
                                listContasAnaliticas.Add(contaAnalitica);
                            }
                        }

                        // Verificar se pelo menos 1 registro foi encontrado
                        if (listContasAnaliticas.Count > 0)
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
                                            pdf.Add(new Paragraph("CONTA - DESCRIÇÃO                                                 SALDO ANTERIOR       DEBITOS      CREDITOS   SALDO ATUAL", fonte));
                                            pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                            pdf.Add(new Paragraph($"   ", fonte));

                                            // Contar linhas usadas após adição do cabeçalho
                                            linhasDisponiveis -= 6;
                                        };

                                        // Adicionar cabeçalho da primeira página
                                        adicionarCabecalho(subtitulo);

                                        var listContasSinteticas = new List<frmBalanceteGeral.ContaSintetica>();

                                        // Para cada lançamento: obter as contas sintéticas
                                        foreach (var contaAnalitica in listContasAnaliticas)
                                        {
                                            frmBalanceteGeral.DecomporContaAnalitica(contaAnalitica.Conta, listContasSinteticas, con);
                                        }

                                        // Armazenar todos os registros em uma lista de objetos e ordenar
                                        var todasContas = listContasSinteticas.Cast<object>().Concat(listContasAnaliticas)
                                            .OrderBy(c => c is frmBalanceteGeral.ContaSintetica ? ((frmBalanceteGeral.ContaSintetica)c).Conta : ((ContaAnalitica)c).Conta)
                                            .ThenBy(c => c is frmBalanceteGeral.ContaSintetica ? 1 : 0)
                                            .ToList();

                                        // Liberar a memória das listas anteriores
                                        listContasSinteticas = null;
                                        listContasAnaliticas = null;

                                        // Solicitar coleta de lixo
                                        GC.Collect();

                                        // Pilha para armazenar as contas sintéticas "abertas"
                                        Stack<frmBalanceteGeral.ContaSintetica> pilhaContas = new Stack<frmBalanceteGeral.ContaSintetica>();

                                        // Função para adicionar linha de contas sintéticas
                                        void AdicionarParagrafoSintetico(string conta, string descricao, int espacosInicio, int espacosDescricao, int linhasNecessarias)
                                        {
                                            // Obter linhas da descrição
                                            var linhasDescricao = Contabilidade.Forms.Relatorios.frmSaldo.QuebrarLinhaString(descricao, espacosDescricao);

                                            pdf.Add(new Paragraph($"{"".PadRight(espacosInicio)}{conta.PadRight(conta.Length)} - {linhasDescricao[0].PadRight(espacosDescricao)}", fonte));
                                            linhasDisponiveis -= 1;

                                            // Adicionar as outras linhas (se houver mais que uma)
                                            for (int i = 1; i < linhasNecessarias; i++)
                                            {
                                                pdf.Add(new Paragraph($"{"    ".PadRight(espacosInicio + conta.Length)}   {linhasDescricao[i].PadRight(espacosDescricao)}", fonte));
                                                linhasDisponiveis -= 1;
                                            }
                                        }

                                        // Função para adicionar linha de contas analiticas
                                        void AdicionarParagrafo(string conta, string descricao, decimal saldoAnterior, decimal debitos, decimal creditos, decimal saldo, int espacosInicio, int espacosDescricao, int linhasNecessarias)
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

                                        void ProcessarContaFechada(frmBalanceteGeral.ContaSintetica contaFechada)
                                        {
                                            // Obter dados e calcular espaços
                                            var grauContaRemover = frmBalanceteGeral.verificarGrauConta(contaFechada.Conta);
                                            int espacosInicioRemover = 2 * (grauContaRemover - 1);
                                            int espacosDescricaoRemover = 66 - espacosInicioRemover - contaFechada.Conta.Length - 3;
                                            var saldoAnteriorRemover = contaFechada.Saldo - (contaFechada.Creditos + contaFechada.Debitos);
                                            var linhasNecessariasRemover = frmBalanceteGeral.obterQuantidadeLinhasString(contaFechada.Descricao, espacosDescricaoRemover);

                                            // Verificar se a quantidade de linhas disponiveis é suficiente
                                            if ((linhasDisponiveis - linhasNecessariasRemover) < 0)
                                            {
                                                pdf.NewPage();
                                                linhasDisponiveis = 63;
                                                adicionarCabecalho(subtitulo);
                                            }

                                            AdicionarParagrafo(contaFechada.Conta, contaFechada.Descricao, saldoAnteriorRemover, contaFechada.Debitos, contaFechada.Creditos, contaFechada.Saldo, espacosInicioRemover, espacosDescricaoRemover, linhasNecessariasRemover);
                                        }

                                        // Para cada conta
                                        foreach (var conta in todasContas)
                                        {
                                            // Verificar se é sintética ou analitica
                                            if (conta is frmBalanceteGeral.ContaSintetica contaSintetica)
                                            {
                                                // Fechar contas de grau igual ou superior a esta
                                                while (pilhaContas.Count > 0 && pilhaContas.Peek().Grau >= contaSintetica.Grau)
                                                {
                                                    ProcessarContaFechada(pilhaContas.Pop());
                                                }

                                                // Obter dados e espaçamentos a descrição
                                                var grauConta = frmBalanceteGeral.verificarGrauConta(contaSintetica.Conta);
                                                int espacosInicio = 2 * (grauConta - 1);
                                                int espacosDescricao = 66 - espacosInicio - contaSintetica.Conta.Length - 3;
                                                var linhasNecessarias = frmBalanceteGeral.obterQuantidadeLinhasString(contaSintetica.Descricao, espacosDescricao);

                                                // Verificar se a quantidade de linhas disponiveis é suficiente
                                                if ((linhasDisponiveis - linhasNecessarias) < 0)
                                                {
                                                    pdf.NewPage();
                                                    linhasDisponiveis = 63;
                                                    adicionarCabecalho(subtitulo);
                                                }

                                                AdicionarParagrafoSintetico(contaSintetica.Conta, contaSintetica.Descricao, espacosInicio, espacosDescricao, linhasNecessarias);
                                                // Adicionar a conta sintetica aberta a pilha de contas
                                                pilhaContas.Push(contaSintetica);
                                            }
                                            // Se for analitica
                                            else if (conta is ContaAnalitica contaAnalitica)
                                            {
                                                // Obter dados e calcular espaços
                                                var grauConta = frmBalanceteGeral.verificarGrauConta(contaAnalitica.Conta);
                                                int espacosInicio = 2 * (grauConta - 1);
                                                int espacosDescricao = 66 - espacosInicio - contaAnalitica.Conta.Length - 3;
                                                var saldoAnterior = contaAnalitica.Saldo - (contaAnalitica.Creditos + contaAnalitica.Debitos);
                                                var linhasNecessarias = frmBalanceteGeral.obterQuantidadeLinhasString(contaAnalitica.Descricao, espacosDescricao);

                                                // Verificar se a quantidade de linhas disponiveis é suficiente
                                                if ((linhasDisponiveis - linhasNecessarias) < 0)
                                                {
                                                    pdf.NewPage();
                                                    linhasDisponiveis = 63;
                                                    adicionarCabecalho(subtitulo);
                                                }

                                                AdicionarParagrafo(contaAnalitica.Conta, contaAnalitica.Descricao, saldoAnterior, contaAnalitica.Debitos, contaAnalitica.Creditos, contaAnalitica.Saldo, espacosInicio, espacosDescricao, linhasNecessarias);

                                                // Adicionar valores em cada conta sintética aberta
                                                foreach (var grupoAberto in pilhaContas)
                                                {
                                                    grupoAberto.Debitos += contaAnalitica.Debitos;
                                                    grupoAberto.Creditos += contaAnalitica.Creditos;
                                                    grupoAberto.Saldo += contaAnalitica.Saldo;
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao gerar o relatório", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
