using Contabilidade.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Data;
using System.Text;
using System.Diagnostics;
using System.Data.SQLite;
using Contabilidade.Forms.Cadastros;

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

        public static void DecomporContaAnalitica(string conta, List<ContaSintetica> listaContasSinteticas, Conexao conexao)
        {
            // Iterar enquanto a conta possuir ponto (não for a conta raiz = grau 1)
            while (conta.Contains("."))
            {
                int ultimoPonto = conta.LastIndexOf('.');
                conta = conta.Substring(0, ultimoPonto);
                int grau = Contabilidade.Forms.Relatorios.frmBalanceteGeral.verificarGrauConta(conta);

                // Verificar se a conta já existe na lista
                if (listaContasSinteticas.Any(cs => cs.Conta == conta))
                {
                    return; // Cancelar a função se a conta já existir na lista
                }

                // Obter descrição da conta
                var comandoSql = "SELECT descricao FROM contas WHERE conta = @conta;";
                using (var comando = new SQLiteCommand(comandoSql, conexao.conn))
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
            using (var comando = new SQLiteCommand(sql, conexao.conn))
            {
                comando.Parameters.AddWithValue("@conta", conta);
                var descricao = comando.ExecuteScalar()?.ToString();
                listaContasSinteticas.Add(new ContaSintetica(conta, descricao, 1));
            }
        }

        public class ContaSintetica
        {
            public string Conta { get; set; }
            public string Descricao { get; set; }
            public int Grau { get; set; }
            public decimal Saldo { get; set; }

            public ContaSintetica(string conta, string descricao, int grau)
            {
                this.Conta = conta;
                this.Descricao = descricao;
                this.Grau = grau;
                this.Saldo = 0;
            }
        }

        private class ContaAnalitica
        {
            public string Conta { get; set; }
            public string Descricao { get; set; }
            public decimal Saldo { get; set; }
        }

        // Função para dividir a descrição
        public static List<string> QuebrarLinhaString(string texto, int limiteCaracteresColuna)
        {
            string[] palavras = texto.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            List<string> linhas = new List<string>();
            StringBuilder linhaAtual = new StringBuilder();
            int comprimentoAtual = 0;

            foreach (string palavra in palavras)
            {
                var comprimentoEsperado = comprimentoAtual + palavra.Length;
                if (comprimentoEsperado <= limiteCaracteresColuna)
                {
                    if ((comprimentoEsperado + 1) <= limiteCaracteresColuna)
                    {
                        linhaAtual.Append(palavra + " ");
                        comprimentoAtual += palavra.Length + 1;
                    }
                    else
                    {
                        linhaAtual.Append(palavra);
                        comprimentoAtual = limiteCaracteresColuna;
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
                // Verificar se a conta selecionada é o caixa (caso ela apareça, de alguma forma)
                else if (txtConta.Text == "0")
                {
                    MessageBox.Show("Não é possível gerar um relatório desse tipo para o caixa, por favor, selecione as outras contas.", "Uma conta válida não foi selecionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    var comando = new SQLiteCommand(sql, con.conn);

                    var data = dtpData.Value;
                    comando.Parameters.AddWithValue("@data", data);
                    comando.Parameters.AddWithValue("@conta", txtConta.Text);

                    List<ContaAnalitica> listContasAnaliticas = new List<ContaAnalitica>();

                    // Executar comando e obter os dados
                    using (var reader = comando.ExecuteReader())
                    {
                        // Criar uma lista com todos os dados
                        while (reader.Read())
                        {
                            var contaAnalitica = new ContaAnalitica
                            {
                                Conta = reader["conta"].ToString(),
                                Descricao = reader["descricao"].ToString(),
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
                                        pdf.Add(new Paragraph($"                                       LISTAGEM DE SALDOS - {dataFormatada}                             PÁGINA: {writer.PageNumber.ToString("D3")}", fonte));
                                        pdf.Add(new Paragraph($"{subtitulo}", fonte));
                                        pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                        pdf.Add(new Paragraph("CONTA - DESCRIÇÃO                                                                                        SALDO", fonte));
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
                                        if (descricaoCaixa?.Length >= 92)
                                        {
                                            // Dividir considerando o tamanho máximo que pode ter (sem contar o espaço para a outra coluna)
                                            var linhasDescricao = QuebrarLinhaString(descricaoCaixa, 92);

                                            // Adicionar primeira linha
                                            pdf.Add(new Paragraph($"0 - {linhasDescricao[0].PadRight(92)}{saldoCaixa.ToString("#,##0.00").PadLeft(14)}", fonte));

                                            // Adicionar segunda linha
                                            pdf.Add(new Paragraph($"    {linhasDescricao[1]}", fonte));

                                            // Contabilizar linhas
                                            linhasDisponiveis -= 2;
                                        }
                                        else
                                        {
                                            pdf.Add(new Paragraph($"0 - {descricaoCaixa?.PadRight(92)}{saldoCaixa.ToString("#,##0.00").PadLeft(14)}", fonte));

                                            // Contabilizar linha
                                            linhasDisponiveis -= 1;
                                        }
                                    }

                                    var listContasSinteticas = new List<ContaSintetica>();

                                    // Para cada lançamento: obter as contas sintéticas
                                    foreach (var contaAnalitica in listContasAnaliticas)
                                    {
                                        DecomporContaAnalitica(contaAnalitica.Conta, listContasSinteticas, con);
                                    }

                                    // Armazenar todos os registros em uma lista de objetos e ordenar
                                    var todasContas = listContasSinteticas.Cast<object>().Concat(listContasAnaliticas)
                                        .OrderBy(c => c is ContaSintetica ? ((ContaSintetica)c).Conta : ((ContaAnalitica)c).Conta)
                                        .ThenBy(c => c is ContaSintetica ? 1 : 0)
                                        .ToList();

                                    // Liberar a memória das listas anteriores
                                    listContasSinteticas = null;
                                    listContasAnaliticas = null;

                                    // Solicitar coleta de lixo
                                    GC.Collect();

                                    // Pilha para armazenar as contas sintéticas "abertas"
                                    Stack<ContaSintetica> pilhaContas = new Stack<ContaSintetica>();

                                    // Função para adicionar linha de contas sintéticas
                                    void AdicionarParagrafoSintetico(string conta, string descricao, int espacosInicio, int espacosDescricao)
                                    {
                                        // Por padrão apenas 1 linha será necessária
                                        var linhasNecessarias = 1;
                                        List<string> linhasDescricao = [];
                                        var haveraQuebra = frmBalanceteGeral.verificarSeHaveraQuebraDeLinha(descricao, espacosDescricao);

                                        // Verificar se haverá quebra de linha na descrição - se houver, obter linhas
                                        if (haveraQuebra)
                                        {
                                            linhasDescricao = Contabilidade.Forms.Relatorios.frmSaldo.QuebrarLinhaString(descricao, espacosDescricao);
                                            linhasNecessarias = linhasDescricao.Count;
                                        }

                                        // Verificar se a quantidade de linhas disponiveis é suficiente
                                        if ((linhasDisponiveis - linhasNecessarias) < 0)
                                        {
                                            pdf.NewPage();
                                            linhasDisponiveis = 63;
                                            adicionarCabecalho(subtitulo);
                                        }

                                        // Adicionar linhas - testar se será apenas uma ou várias
                                        if (haveraQuebra)
                                        {
                                            // Várias linhas
                                            for (int i = 0; i < linhasNecessarias; i++)
                                            {
                                                pdf.Add(new Paragraph($"{"    ".PadRight(espacosInicio + conta.Length)}   {linhasDescricao[i].PadRight(espacosDescricao)}", fonte));
                                                linhasDisponiveis -= 1;
                                            }
                                        }
                                        else
                                        {
                                            pdf.Add(new Paragraph($"{"".PadRight(espacosInicio)}{conta.PadRight(conta.Length)} - {descricao.PadRight(espacosDescricao)}", fonte));
                                            linhasDisponiveis -= 1;
                                        }
                                    }

                                    void AdicionarParagrafosPdf(string conta, string descricao, decimal saldo, int espacosInicio, int espacosDescricao)
                                    {
                                        // Por padrão apenas 1 linha será necessária
                                        var linhasNecessarias = 1;
                                        List<string> linhasDescricao = [];
                                        var haveraQuebra = frmBalanceteGeral.verificarSeHaveraQuebraDeLinha(descricao, espacosDescricao);

                                        // Verificar se haverá quebra de linha na descrição - se houver, obter linhas
                                        if (haveraQuebra)
                                        {
                                            linhasDescricao = Contabilidade.Forms.Relatorios.frmSaldo.QuebrarLinhaString(descricao, espacosDescricao);
                                            linhasNecessarias = linhasDescricao.Count;
                                        }

                                        // Verificar se a quantidade de linhas disponiveis é suficiente
                                        if ((linhasDisponiveis - linhasNecessarias) < 0)
                                        {
                                            pdf.NewPage();
                                            linhasDisponiveis = 63;
                                            adicionarCabecalho(subtitulo);
                                        }

                                        // Adicionar linhas - testar se será apenas uma ou várias
                                        if (haveraQuebra)
                                        {
                                            // Várias linhas
                                            for (int i = 0; i < linhasNecessarias; i++)
                                            {
                                                pdf.Add(new Paragraph($"{"    ".PadRight(espacosInicio + conta.Length)}   {linhasDescricao[i].PadRight(espacosDescricao)}", fonte));
                                                linhasDisponiveis -= 1;
                                            }
                                        }
                                        else
                                        {
                                            pdf.Add(new Paragraph($"{"".PadRight(espacosInicio)}{conta.PadRight(conta.Length)} - {descricao.PadRight(espacosDescricao)}{saldo.ToString("#,##0.00").PadLeft(14)}", fonte));
                                            linhasDisponiveis -= 1;
                                        }
                                    }

                                    void ProcessarContaFechada(ContaSintetica contaFechada)
                                    {
                                        // Obter dados e calcular espaços
                                        var grauContaRemover = Contabilidade.Forms.Relatorios.frmBalanceteGeral.verificarGrauConta(contaFechada.Conta);
                                        int espacosInicioRemover = 2 * (grauContaRemover - 1);
                                        int espacosDescricaoRemover = 96 - espacosInicioRemover - contaFechada.Conta.Length - 3;

                                        AdicionarParagrafosPdf(contaFechada.Conta, contaFechada.Descricao, contaFechada.Saldo, espacosInicioRemover, espacosDescricaoRemover);
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
                                            var grauConta = Contabilidade.Forms.Relatorios.frmBalanceteGeral.verificarGrauConta(contaSintetica.Conta);
                                            int espacosInicio = 2 * (grauConta - 1);
                                            int espacosDescricao = 96 - espacosInicio - contaSintetica.Conta.Length - 3;

                                            AdicionarParagrafoSintetico(contaSintetica.Conta, contaSintetica.Descricao, espacosInicio, espacosDescricao);
                                            // Adicionar a conta sintetica aberta a pilha de contas
                                            pilhaContas.Push(contaSintetica);
                                        }
                                        // Se for analitica
                                        else if (conta is ContaAnalitica lancamento)
                                        {
                                            // Obter dados e calcular espaços
                                            var grauConta = Contabilidade.Forms.Relatorios.frmBalanceteGeral.verificarGrauConta(lancamento.Conta);
                                            int espacosInicio = 2 * (grauConta - 1);
                                            int espacosDescricao = 96 - espacosInicio - lancamento.Conta.Length - 3;

                                            AdicionarParagrafosPdf(lancamento.Conta, lancamento.Descricao, lancamento.Saldo, espacosInicio, espacosDescricao);

                                            // Adicionar valores em cada conta sintética aberta
                                            foreach (var grupoAberto in pilhaContas)
                                            {
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
