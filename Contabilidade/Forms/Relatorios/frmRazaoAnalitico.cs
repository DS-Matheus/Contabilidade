using Contabilidade.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Data.SQLite;
using Org.BouncyCastle.Asn1.X509;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Generic;
using System.Windows.Forms;
using Contabilidade.Forms.Cadastros;
using System.Linq;

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
        private class Lancamento
        {
            public DateTime Data { get; set; }
            public string Historico { get; set; }
            public decimal Valor { get; set; }
            public decimal Saldo { get; set; }
        }

        public static (string, string, string, string) ordenarDatasEObterStringsFormatadas(DateTime data1, DateTime data2)
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
            try
            {
                var conta = txtConta.Text;

                // Verificar se a conta foi preenchida
                if (string.IsNullOrWhiteSpace(txtConta.Text))
                {
                    MessageBox.Show("Selecione uma conta antes de gerar o relatório, dê um duplo clique na linha desejada.", "Uma conta não foi selecionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                // Testar se a conta selecionada não foi o caixa
                if (conta == "0")
                {
                    MessageBox.Show("Não é possível gerar um razão analítico do caixa, por favor selecione outra conta.", "Operação bloqueada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else if (nivel == "S")
                {
                    MessageBox.Show("Não é possível gerar um razão analítico de uma conta sintética, por favor selecione uma conta analítica.", "Tipo de conta inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    // Obter datas
                    var (dataInicial, dataInicialFormatada, dataFinal, dataFinalFormatada) = ordenarDatasEObterStringsFormatadas(dtpInicial.Value, dtpFinal.Value);

                    // Consulta de lançamentos para a conta no período informado
                    var sql = "SELECT l.data, h.historico, l.valor, l.saldo FROM lancamentos l JOIN historicos h ON l.id_historico = h.id WHERE l.conta = @conta AND l.data BETWEEN @dataInicial AND @dataFinal ORDER BY l.data ASC, l.id ASC;";
                    using (var comando = new SQLiteCommand(sql, con.conn))
                    {
                        comando.Parameters.AddWithValue("@conta", conta);
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
                                    Data = Convert.ToDateTime(reader["data"]),
                                    Historico = reader["historico"].ToString(),
                                    Valor = (Convert.ToInt32(reader["valor"]) / 100m),
                                    Saldo = (Convert.ToInt32(reader["saldo"]) / 100m)
                                };
                                listLancamentos.Add(lancamento);
                            }
                        }

                        // Verificar se pelo menos 1 registro foi encontrado
                        if (listLancamentos.Count > 0)
                        {
                            // Obter a descrição da conta
                            comando.CommandText = "SELECT descricao FROM contas WHERE conta = @conta;";
                            var descricao = comando.ExecuteScalar()?.ToString();

                            // Obter saldo anterior (ou deixar como 0 se não possuir nenhum registro antes do período)
                            comando.CommandText = "SELECT COALESCE((SELECT saldo FROM lancamentos WHERE conta = @conta AND data < @dataInicial ORDER BY data DESC, id DESC LIMIT 1), 0) AS saldo_anterior; ";
                            decimal saldoAnterior = (Convert.ToInt32(comando.ExecuteScalar()) / 100m);

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

                                        var espacoDescricao = 110 - 7 - conta.Length - 3;
                                        var linhasDescricao = Contabilidade.Forms.Relatorios.frmSaldo.QuebrarLinhaString(descricao, espacoDescricao);

                                        // Função local para adicionar o cabeçalho
                                        void adicionarCabecalho(string subtitulo)
                                        {
                                            // Adicionando parágrafos ao documento
                                            pdf.Add(new Paragraph($"RAZÃO ANALÍTICO                          PERÍODO: {dataInicialFormatada} A {dataFinalFormatada}                          PÁGINA: {(pdf.PageNumber + 1).ToString("D3")}", fonte));
                                            pdf.Add(new Paragraph($"{subtitulo}", fonte));
                                            pdf.Add(new Paragraph($"CONTA: {conta} - {linhasDescricao[0]}", fonte));
                                            // Verificação condicional para evitar "index out of bounds"
                                            string linhaDescricao1 = linhasDescricao.Count > 1 ? linhasDescricao[1] : "";
                                            pdf.Add(new Paragraph($"SALDO ANTERIOR: {saldoAnterior.ToString("#,##0.00")}   {linhaDescricao1}", fonte));
                                            pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                            pdf.Add(new Paragraph("DATA       HISTÓRICO                                                       DÉBITOS      CRÉDITOS         SALDO", fonte));
                                            pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                            pdf.Add(new Paragraph($"   ", fonte));

                                            // Contar linhas usadas após adição do cabeçalho
                                            linhasDisponiveis -= 8;
                                        };

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
                                            var (data, historico, valor, saldo) = (lancamento.Data, lancamento.Historico, lancamento.Valor, lancamento.Saldo);
                                            var dataLancamento = data.ToString("dd/MM/yyyy");

                                            // Iniciar criação de linha
                                            var linha = new StringBuilder();

                                            // Verificar se o mês anterior é diferente do atual 
                                            if (mesAnterior != data.ToString("MM/yyyy"))
                                            {
                                                // Verificar se é ou não a primeira execução, se não for (IF = TRUE): inserir rodapé com o total do mês anterior
                                                if (mesAnterior != "")
                                                {
                                                    // Verificar se há linhas nessa página para incluir o rodapé, caso não haja: criar nova página com o cabeçalho
                                                    if (linhasDisponiveis < 2)
                                                    {
                                                        pdf.NewPage();
                                                        linhasDisponiveis = 57;
                                                        adicionarCabecalho(subtitulo);
                                                    }

                                                    var linhasGastas = 2; // padrão do rodapé

                                                    // Verificar se é possível inserir com espaço superior
                                                    if (linhasDisponiveis >= 3)
                                                    {
                                                        pdf.Add(new Paragraph($"   ", fonte));
                                                        linhasGastas++;
                                                    }

                                                    // Inserir rodapé
                                                    pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                                    pdf.Add(new Paragraph($"{$"TOTAL DO MÊS {mesAnterior}:".PadLeft(68)}{debitosMes.ToString("#,##0.00").PadLeft(14)}{creditosMes.ToString("#,##0.00").PadLeft(14)}{(creditosMes + debitosMes).ToString("#,##0.00").PadLeft(14)}", fonte));

                                                    if (linhasDisponiveis >= 4)
                                                    {
                                                        pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                                        linhasGastas++;
                                                    }

                                                    // Verificar se é possível inserir com espaço inferior
                                                    if (linhasDisponiveis >= 5)
                                                    {
                                                        pdf.Add(new Paragraph($"   ", fonte));
                                                        linhasGastas++;
                                                    }

                                                    linhasDisponiveis -= linhasGastas;
                                                }
                                                // Transferir valores para o total
                                                totalDebitos += debitosMes;
                                                totalCreditos += creditosMes;

                                                // Zerar variáveis
                                                debitosMes = 0;
                                                creditosMes = 0;

                                                // Atualizar mês/ano
                                                mesAnterior = data.ToString("MM/yyyy");
                                            }

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

                                            // Dividir considerando o tamanho máximo que pode ter
                                            var linhasHistorico = Contabilidade.Forms.Relatorios.frmSaldo.QuebrarLinhaString(historico, 57);

                                            // Verificar quantas linhas serão necessárias para cada uso - Não contar o espaço entre as colunas
                                            var linhasNecessarias = lancamento.Historico.Length >= 57 ? linhasHistorico.Count : 1;

                                            // Verificar se há linhas nessa página para incluir os registros, caso não haja: criar nova página com o cabeçalho
                                            if ((linhasDisponiveis - linhasNecessarias) < 0)
                                            {
                                                pdf.NewPage();
                                                linhasDisponiveis = 57;
                                                adicionarCabecalho(subtitulo);
                                            }

                                            // Testar se serão necessárias 1 ou 2 linhas por causa do comprimento do histórico
                                            if (linhasNecessarias >= 2)
                                            {
                                                linha.Append(linhasHistorico[0].PadRight(57));

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

                                                // Remover primeiro item da lista e contabilizar sua adição no pdf
                                                linhasHistorico.RemoveAt(0);
                                                linhasDisponiveis--;

                                                // Adicionar demais linhas
                                                foreach (var linhaHistorico in linhasHistorico)
                                                {
                                                    // Limpar o StringBuilder e iniciar a criação das demais linhas (com conta e valores vázios).
                                                    linha.Clear();

                                                    // Espaço vázio referente a data
                                                    linha.Append("   ".PadRight(11));

                                                    // Adicionar demais linhas
                                                    linha.Append(linhaHistorico.PadRight(57));
                                                    pdf.Add(new Paragraph(linha.ToString(), fonte));

                                                    // Contabilizar linha
                                                    linhasDisponiveis -= 1;
                                                }
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
                                                linhasDisponiveis--;
                                            }
                                        }

                                        // Verificar se há linhas nessa página para incluir o rodapé do último mês, caso não haja: criar nova página com o cabeçalho
                                        if (linhasDisponiveis < 2)
                                        {
                                            pdf.NewPage();
                                            linhasDisponiveis = 57;
                                            adicionarCabecalho(subtitulo);
                                        }

                                        var linhasUsadas = 2; // padrão do rodapé

                                        // Verificar se é possível inserir com espaço superior (já considerando o espaço do próximo/último rodapé)
                                        if (linhasDisponiveis != 4 && linhasDisponiveis >= 3)
                                        {
                                            pdf.Add(new Paragraph($"   ", fonte));
                                            linhasUsadas++;
                                        }

                                        // Inserir rodapé
                                        pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                        pdf.Add(new Paragraph($"{$"TOTAL DO MÊS {mesAnterior}:".PadLeft(68)}{debitosMes.ToString("#,##0.00").PadLeft(14)}{creditosMes.ToString("#,##0.00").PadLeft(14)}{(creditosMes + debitosMes).ToString("#,##0.00").PadLeft(14)}", fonte));

                                        if (linhasDisponiveis > 3)
                                        {
                                            pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                            linhasUsadas++;
                                        }

                                        linhasDisponiveis -= linhasUsadas;

                                        // Transferir valores para o total
                                        totalDebitos += debitosMes;
                                        totalCreditos += creditosMes;

                                        // Verificar se existe espaço para o último rodapé, senão: criar nova página
                                        if (linhasDisponiveis < 1)
                                        {
                                            pdf.NewPage();
                                            linhasDisponiveis = 57;
                                            adicionarCabecalho(subtitulo);
                                        }

                                        // Inserindo rodapé
                                        pdf.Add(new Paragraph($"{$"TOTAL DO PERÍODO ({dataInicialFormatada} A {dataFinalFormatada}):".PadLeft(68)}{totalDebitos.ToString("#,##0.00").PadLeft(14)}{totalCreditos.ToString("#,##0.00").PadLeft(14)}{(totalCreditos + totalDebitos).ToString("#,##0.00").PadLeft(14)}", fonte));
                                        linhasDisponiveis--;
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
