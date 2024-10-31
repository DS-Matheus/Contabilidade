using Contabilidade.Models;
using DGVPrinterHelper;
using System.Data;
using Microsoft.Data.Sqlite;
using Contabilidade.Classes;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Contabilidade.Forms.Cadastros
{
    public partial class frmContas : Form
    {
        Conexao con;
        static DataTable dtDados = new DataTable();
        DataView dv = dtDados.DefaultView;
        public static string conta { get; set; } = "";
        public static string descricao { get; set; } = "";
        public static string nivel { get; set; } = "";
        public static bool alterouNivel { get; set; } = false;

        public frmContas(Conexao conexaoBanco)
        {
            InitializeComponent();

            con = conexaoBanco;

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
            }
            else
            {
                cbbNivel.Visible = false;
                txtFiltrar.Visible = true;
                txtFiltrar.Width = 238;
            }

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
        }

        private bool IsNumeric(string text)
        {
            return int.TryParse(text, out _);
        }

        public static bool verificarExistenciaConta(string conta)
        {
            return dtDados.AsEnumerable().Any(row => conta == row.Field<string>("conta"));
        }

        public static bool verificarExistenciaConta(string conta, string contaAntiga)
        {
            return dtDados.AsEnumerable().Any(row => conta == row.Field<string>("conta") && row.Field<string>("conta") != contaAntiga);
        }

        public static bool verificarContaSintetica(string conta)
        {
            // Remove o último grupo de caracteres após o último ponto (assim se obtêm a conta sintética associada)
            int ultimoPonto = conta.LastIndexOf('.');
            string contaSintetica = conta;
            if (ultimoPonto != -1)
            {
                contaSintetica = conta.Substring(0, ultimoPonto);
            }

            // Verificar se a conta existe e retornar
            return verificarExistenciaConta(contaSintetica);
        }

        private void btnCriar_Click(object sender, EventArgs e)
        {
            // Criar uma instância do formulário de dados e aguardar um retorno
            using (var frmDados = new frmContasDados("Criar conta", "", "", "A"))
            {
                // O usuário apertou o botão de salvar
                if (frmDados.ShowDialog() == DialogResult.OK)
                {
                    // Criar conta
                    string sql = "INSERT INTO contas (conta, descricao, nivel) VALUES(@conta, @descricao, @nivel);";
                    using (var comando = new SqliteCommand(sql, con.conn))
                    {
                        try
                        {
                            comando.Parameters.AddWithValue("@conta", conta);
                            comando.Parameters.AddWithValue("@descricao", descricao);
                            comando.Parameters.AddWithValue("@nivel", nivel);

                            int retornoBD = comando.ExecuteNonQuery();

                            // Verificar se houve a criação da linha (0 = negativo)
                            if (retornoBD > 0)
                            {
                                // Adicionar dados na tabela
                                DataRow row = dtDados.NewRow();
                                row["conta"] = conta;
                                row["descricao"] = descricao;
                                row["nivel"] = nivel;
                                dtDados.Rows.Add(row);

                                dgvContas.Refresh();

                                // Remover dados das variáveis
                                conta = "";
                                descricao = "";
                                nivel = "";

                                MessageBox.Show("Conta criada com sucesso!", "Criação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            else
                            {
                                throw new Exception("Não foi possível criar a nova conta.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), "Conta não criada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            // Solicita o título do arquivo ao usuário
            string inputTitle = Microsoft.VisualBasic.Interaction.InputBox("Digite o título do arquivo:", "Título do Arquivo", "");

            // Verifica se o usuário clicou em "Cancelar": se clicou não executa
            if (!string.IsNullOrEmpty(inputTitle))
            {
                // Verifica se o título está vazio ou contém apenas espaços
                string title = string.IsNullOrWhiteSpace(inputTitle) ? "Contas Cadastradas" : inputTitle;

                var printer = new DGVPrinter();
                printer.Title = title; // Usa o título fornecido pelo usuário
                printer.SubTitle = string.Format("Data: {0}", System.DateTime.Now.ToString("dd/MM/yyyy"));
                printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                printer.PageNumbers = true;
                printer.PageNumberInHeader = false;
                printer.PorportionalColumns = true;
                printer.HeaderCellAlignment = StringAlignment.Near;
                printer.FooterSpacing = 15;
                printer.PrintDataGridView(dgvContas);
            }
        }

        private (string conta, string descricao, string nivel) obterDadosDGV(int numLinha)
        {
            string conta = dgvContas.Rows[numLinha].Cells["Conta"].Value?.ToString();
            string descricao = dgvContas.Rows[numLinha].Cells["Descrição"].Value?.ToString();
            string nivel = dgvContas.Rows[numLinha].Cells["Nível"].Value?.ToString();

            return (conta, descricao, nivel);
        }

        public static string obterNovaConta(string contaSintetica, string contaAnalitica)
        {
            // Encontra o ponto de separação baseado nos pontos
            int index = contaSintetica.Length;

            // Monta a nova conta com a conta sintética e a parte remanescente da conta original
            string novaConta = contaSintetica + contaAnalitica.Substring(index);

            return novaConta;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Obter conta selecionada
            int numLinha = frmUsuarios.obterNumLinhaSelecionada(dgvContas);
            var (contaAntiga, descricaoAntiga, nivelAntigo) = obterDadosDGV(numLinha);

            // Criar uma instância do formulário de dados e aguardar um retorno
            using (var frmDados = new frmContasDados("Editar Conta", contaAntiga, descricaoAntiga, nivelAntigo, 1, con))
            {
                // O usuário apertou o botão de salvar
                if (frmDados.ShowDialog() == DialogResult.OK)
                {
                    using (var transacao = con.conn.BeginTransaction())
                    {
                        using (var comando = new SqliteCommand("", con.conn))
                        {
                            try
                            {
                                comando.Transaction = transacao;

                                // Se alterou o nível: excluir todos os registros associados
                                if (alterouNivel)
                                {
                                    // Se era analítico e alterou para sintético:
                                    if (nivel == "S")
                                    {
                                        // Excluir todos os lançamentos dessa conta apenas
                                        excluirTodosLancamentos(comando, contaAntiga);
                                    }
                                    // Se era sintético e alterou para analítico: excluir todas as contas no grupo de chave e os seus lançamentos
                                    else
                                    {
                                        // Obter relação de contas a serem excluidas
                                        comando.CommandText = "SELECT conta, nivel FROM contas WHERE conta LIKE @conta || '.%';";
                                        comando.Parameters.Clear();
                                        comando.Parameters.AddWithValue("@conta", contaAntiga);

                                        var listContas = new List<Contas>();

                                        // Ler e atribuir na lista as contas a serem excluídas
                                        using (var reader = comando.ExecuteReader())
                                        {
                                            while (reader.Read())
                                            {
                                                var novaConta = new Contas(reader["conta"].ToString(), reader["nivel"].ToString());
                                                listContas.Add(novaConta);
                                            }
                                        }

                                        // Excluir cada conta presente na lista
                                        foreach (var conta in listContas)
                                        {
                                            excluirConta(con, conta.Conta, conta.Nivel, transacao);
                                        }
                                    }
                                }
                                // Se não alterou o nível, testar se o número da conta mudou e se ela é sintética -> Se for: atualizar número de conta de todo o grupo
                                else if (contaAntiga != conta && nivel == "S")
                                {
                                    // obter relação de contas
                                    comando.CommandText = "SELECT conta FROM contas WHERE conta LIKE @conta || '.%';";
                                    comando.Parameters.Clear();
                                    comando.Parameters.AddWithValue("@conta", contaAntiga);

                                    var listContas = new List<string>();

                                    // Ler e atribuir na lista as contas a serem alteradas
                                    using (var reader = comando.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            var novaConta = reader["conta"].ToString();
                                            listContas.Add(novaConta);
                                        }
                                    }

                                    // Comando para atualizar as contas
                                    comando.CommandText = "UPDATE contas SET conta = @contaNova WHERE conta = @contaAntiga;";

                                    // Alterar cada conta presente na lista
                                    foreach (var contaFilha in listContas)
                                    {
                                        // Número de conta novo (var conta = novo número da conta sintética)
                                        var numeroNovo = obterNovaConta(conta, contaFilha);

                                        comando.Parameters.Clear();
                                        comando.Parameters.AddWithValue("@contaNova", numeroNovo);
                                        comando.Parameters.AddWithValue("@contaAntiga", contaFilha);
                                        comando.ExecuteNonQuery();
                                    }
                                }

                                // Atualizar conta (se o número dela alterou, os lançamentos também serão atualizados por causa do ON UPDATE da chave estrangeira)
                                comando.CommandText = "UPDATE contas SET conta = @contaNova, descricao = @descricao, nivel = @nivel WHERE conta = @contaAntiga;";
                                comando.Parameters.Clear();
                                comando.Parameters.AddWithValue("@contaNova", conta);
                                comando.Parameters.AddWithValue("@descricao", descricao);
                                comando.Parameters.AddWithValue("@nivel", nivel);
                                comando.Parameters.AddWithValue("@contaAntiga", contaAntiga);

                                int retornoBD = comando.ExecuteNonQuery();

                                // Verificar se houve a edição de alguma linha (0 = negativo)
                                if (retornoBD > 0)
                                {
                                    transacao.Commit();

                                    // Atualizar DataTable
                                    atualizarDataGrid();

                                    // Remover dados das variáveis
                                    conta = "";
                                    descricao = "";
                                    nivel = "";
                                    alterouNivel = false;

                                    MessageBox.Show("Conta editada com sucesso!", "Edição bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                                else
                                {
                                    throw new Exception("Não foi possível encontrar a conta ou ocorreu um erro na edição.");
                                }
                            }
                            catch (Exception ex)
                            {
                                transacao.Rollback();
                                MessageBox.Show(ex.Message.ToString(), "Edição não realizada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
        }

        private class TotalLancamentos
        {
            public string data { get; set; }
            public decimal total { get; set; }

            public TotalLancamentos(string data,  decimal total)
            {
                this.data = data;
                this.total = total;
            }
        }

        public void excluirTodosLancamentos(SqliteCommand comando, string conta)
        {
            // Obter a lista de datas que possuem lançamentos
            var listDatas = new List<string>();
            comando.CommandText = "SELECT DISTINCT data FROM lancamentos WHERE conta = @conta ORDER BY data;";
            comando.Parameters.AddWithValue("@conta", conta);

            using (var reader = comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    listDatas.Add(reader["data"].ToString());
                }
            }

            // Verificar se foi encontrado algum lançamento
            if (listDatas.Any())
            {
                // Obter relação de datas e valores líquidos dos lançamentos em cada data
                var listLancamentos = new List<TotalLancamentos>();
                
                // Obter o valor total dos lançamentos em cada data -> obter saldo mais recente em uma data e somar esse valor a um total, a cada operação reduzir do saldo encontrado o total (esse será o real valor dos lançamentos no período)
                var saldoAnterior = 0m;
                foreach (var data in listDatas)
                {
                    comando.Parameters.Clear();
                    comando.Parameters.AddWithValue("@conta", conta);
                    comando.Parameters.AddWithValue("@data", data);

                    comando.CommandText = "SELECT COALESCE((SELECT saldo FROM lancamentos WHERE conta = @conta and data = @data ORDER BY data DESC, id DESC LIMIT 1), 0);";
                    var saldoEncontrado = Convert.ToDecimal(comando.ExecuteScalar());

                    // Valor líquido do periodo (o que será reduzido do caixa) = saldoEncontrado - saldoAnterior
                    saldoEncontrado -= saldoAnterior;
                    saldoAnterior += saldoEncontrado;

                    var novoLancamento = new TotalLancamentos(data, saldoEncontrado);
                    listLancamentos.Add(novoLancamento);
                }

                // Atualizar registros do caixa com os valores encontrados (reverter os lançamentos)
                comando.CommandText = "UPDATE registros_caixa SET saldo = (saldo - @valor) WHERE data >= @data;";
                foreach (var registro in listLancamentos)
                {
                    comando.Parameters.Clear();
                    comando.Parameters.AddWithValue("@valor", registro.total);
                    comando.Parameters.AddWithValue("@data", registro.data);
                    comando.ExecuteNonQuery();
                }

                // Excluir todos os lançamentos da conta
                comando.CommandText = "DELETE FROM lancamentos WHERE conta = @conta;";
                comando.Parameters.Clear();
                comando.Parameters.AddWithValue("@conta", conta);
                comando.ExecuteNonQuery();
            }
        }
        
        public void excluirConta(Conexao con, string conta, string nivel, SqliteTransaction transacao)
        {
            using (var comando = new SqliteCommand("", con.conn))
            {
                comando.Transaction = transacao;

                // Se for uma conta analítica:
                if (nivel == "A")
                {
                    excluirTodosLancamentos(comando, conta);
                }

                // Excluir a conta
                comando.CommandText = "DELETE FROM contas WHERE conta = @conta;";
                comando.Parameters.Clear();
                comando.Parameters.AddWithValue("@conta", conta);
                var result = comando.ExecuteNonQuery();

                if (result == 0) {
                    throw new CustomException("Houve um erro ao excluir a conta");
                }
            }
        }

        private class Contas
        {
            public string Conta { get; set; }
            public string Nivel { get; set; }

            public Contas(string conta, string nivel)
            {
                this.Conta = conta;
                this.Nivel = nivel;
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                var dialogResult = MessageBox.Show("Deseja excluir a conta selecionada? Esse processo não pode ser desfeito!", "Confirmação de exclusão da conta", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    // Verifica se uma linha foi selecionada
                    if (dgvContas.SelectedRows.Count > 0)
                    {
                        // Obtem a linha selecionada
                        DataGridViewRow selectedRow = dgvContas.SelectedRows[0];

                        // Obtem a conta a ser excluida
                        var contaExcluir = selectedRow.Cells["Conta"].Value.ToString();
                        var nivelExcluir = selectedRow.Cells["Nível"].Value.ToString();

                        using (var transacao = con.conn.BeginTransaction())
                        {
                            try
                            {
                                using (var comando = new SqliteCommand("", con.conn))
                                {
                                    comando.Transaction = transacao;

                                    // Se o nível for sintético, avisar sobre a exclusão de todas as contas analiticas associadas
                                    if (nivelExcluir == "S")
                                    {
                                        dialogResult = MessageBox.Show("A conta selecionada é do tipo sintético, isso quer dizer que ao excluí-la todas as contas analíticas associadas também serão removidas, deseja continuar? Esse processo é irreversível!", "Confirmação de exclusão de conta sintética", MessageBoxButtons.YesNoCancel);

                                        if (dialogResult != DialogResult.Yes)
                                        {
                                            return;
                                        }
                                    }

                                    // Confirmar uma última vez sobre a exclusão dos registros/lançamentos
                                    dialogResult = MessageBox.Show("Deseja realmente excluir a conta e, se existirem, todos os seus lancamentos associados? Este é o último aviso!", "Confirmação da exclusão da conta", MessageBoxButtons.YesNo);

                                    if (dialogResult == DialogResult.Yes)
                                    {
                                        // Verificar tipo da conta
                                        if (nivelExcluir == "S")
                                        {
                                            // Obter relação de contas a serem excluidas
                                            comando.CommandText = "SELECT conta, nivel FROM contas WHERE conta LIKE @conta || '.%';";
                                            comando.Parameters.AddWithValue("@conta", contaExcluir);

                                            var listContas = new List<Contas>();

                                            // Ler e atribuir na lista as contas a serem excluídas
                                            using (var reader = comando.ExecuteReader())
                                            {
                                                while (reader.Read())
                                                {
                                                    var novaConta = new Contas(reader["conta"].ToString(), reader["nivel"].ToString());
                                                    listContas.Add(novaConta);
                                                }
                                            }
                                            
                                            // Excluir cada conta presente na lista
                                            foreach(var conta in listContas)
                                            {
                                                excluirConta(con, conta.Conta, conta.Nivel, transacao);
                                            }
                                        }

                                        // Excluir a conta selecionada
                                        excluirConta(con, contaExcluir, nivelExcluir, transacao);

                                        // Efetivar alterações
                                        transacao.Commit();

                                        // Remover dados da tabela - Recarregar completamente
                                        atualizarDataGrid();

                                        MessageBox.Show("Conta excluida com sucesso.", "Operação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                            }
                            catch (CustomException ex)
                            {
                                transacao.Rollback();
                                MessageBox.Show($"{ex.Message?.ToString()}", "Erro ao excluir a conta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            catch (Exception ex)
                            {
                                transacao.Rollback();
                                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao excluir a conta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao excluir a conta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
