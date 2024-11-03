using Contabilidade.Models;
using System.Data;
using Microsoft.Data.Sqlite;
using Contabilidade.Classes;

namespace Contabilidade.Forms.Lancamentos
{
    public partial class frmLancamentos : Form
    {
        Conexao con;
        static DataTable dtDados = new DataTable();
        DataView dv = dtDados.DefaultView;
        public static string conta { get; set; } = "";
        public static decimal valor { get; set; } = 0;
        public static string id_historico { get; set; } = "";
        public static DateTime data { get; set; } = DateTime.Today;

        public frmLancamentos(Conexao conexaoBanco)
        {
            InitializeComponent();

            con = conexaoBanco;

            atualizarDataGrid();
        }

        public void atualizarDataGrid()
        {
            // Query de pesquisa
            string sql = "SELECT l.id, l.conta, c.descricao, l.valor, l.data, l.id_historico, h.historico FROM lancamentos l JOIN contas c ON l.conta = c.conta JOIN historicos h ON l.id_historico = h.id ORDER BY l.data DESC, l.conta";
            using (var command = new SqliteCommand(sql, con.conn))
            {
                dtDados.Clear();
                using (var reader = command.ExecuteReader())
                {
                    dtDados.Load(reader);
                }

                dgvLancamentos.DataSource = dtDados;

                txtFiltrar.Text = "";
                dv.RowFilter = $"conta LIKE '{txtFiltrar.Text}%'";
                dgvLancamentos.DataSource = dv;

                cbbFiltrar.SelectedIndex = 0;
            }
        }

        public static void reverterLancamentos(Conexao con, SqliteCommand comando, decimal valorOriginal, SqliteTransaction transacao)
        {
            using (var reader = comando.ExecuteReader())
            {
                // Criar comando para reverter os registros
                var sql2 = "UPDATE lancamentos SET saldo = (saldo - @valor) WHERE id = @id";
                using (var comando2 = new SqliteCommand(sql2, con.conn))
                {
                    // Atribuir transação ao comando 2
                    comando2.Transaction = transacao;

                    // Variáveis para controle dos campos modificados
                    var totalCampos = 0;
                    var totalAlterado = 0;

                    // Para cada campo encontrado
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(0);

                        // Atualizar os campos de referência (valor apenas é inserido novamente por causa do .Clear())
                        comando2.Parameters.Clear();
                        comando2.Parameters.AddWithValue("@valor", valorOriginal);
                        comando2.Parameters.AddWithValue("@id", id);

                        totalCampos++;
                        totalAlterado += comando2.ExecuteNonQuery();
                    }

                    // Testar se todos os registros foram alterados
                    if (totalCampos != totalAlterado)
                    {
                        throw new CustomException("Houve um erro na hora de atualizar os valores de saldo");
                    }
                }
            }
        }

        public static void excluirLancamento(Conexao con, string ID, string dataConvertida, decimal valorLancamento, SqliteTransaction transacao)
        {
            using (var comando = new SqliteCommand("", con.conn))
            {
                comando.Transaction = transacao;

                // Reverter valores do lançamento na sua data -> atualizar saldos nos lançamentos seguintes
                comando.CommandText = "SELECT id FROM lancamentos WHERE data = @data AND id > @id;";
                comando.Parameters.AddWithValue("@data", dataConvertida);
                comando.Parameters.AddWithValue("@id", ID);
                reverterLancamentos(con, comando, valorLancamento, transacao);

                // Reverter valor do lançamento nas datas seguintes -> atualizar saldos dos lançamentos seguintes
                comando.CommandText = "SELECT id FROM lancamentos WHERE data > @data";
                reverterLancamentos(con, comando, valorLancamento, transacao);

                // Reverter valor do lançamento nos registros do caixa nas datas >= sua data
                comando.CommandText = "UPDATE registros_caixa SET saldo = (saldo - @valor) WHERE data >= @data;";
                comando.Parameters.AddWithValue("@valor", valorLancamento);
                comando.ExecuteNonQuery();

                // Excluir lançamento
                comando.CommandText = "DELETE FROM lancamentos WHERE id = @id;";
                var resultado = comando.ExecuteNonQuery();

                if (resultado == 0) {
                    throw new CustomException("Não foi possivel excluir o lançamento");
                }
            }
        }

        public void criarLancamento(SqliteCommand comando, SqliteTransaction transacao)
        {
            // Variável para armazenar o saldo da conta antes do lançamento
            decimal saldo = 0;

            // Converter data para o formato do banco (sem as horas)
            var dataConvertida = data.ToString("yyyy-MM-dd");

            // Obter valor de saldo da conta antes do lançamento
            comando.CommandText = "SELECT COALESCE ((SELECT saldo FROM lancamentos WHERE data <= @data and conta = @conta ORDER BY data DESC, id DESC LIMIT 1), 0);";
            comando.Parameters.AddWithValue("@data", dataConvertida);
            comando.Parameters.AddWithValue("@conta", conta);

            // Atribuir valor encontrado na variável de saldo
            var result = comando.ExecuteScalar();
            if (result != null)
            {
                saldo = Convert.ToDecimal(result);
            }
            else
            {
                throw new CustomException("Não foi possível obter o saldo da conta, por favor anote os valores inseridos e contate o desenvolvedor");
            }

            // Criar lançamento
            comando.CommandText = "INSERT INTO lancamentos (conta, valor, id_historico, data, saldo) VALUES(@conta, @valor, @id_historico, @data, @saldo);";

            comando.Parameters.AddWithValue("@valor", valor);
            comando.Parameters.AddWithValue("@id_historico", id_historico);
            comando.Parameters.AddWithValue("@saldo", saldo + valor);

            int retornoBD = comando.ExecuteNonQuery();

            // Verificar se houve a criação da linha (0 = negativo)
            if (retornoBD > 0)
            {
                // Seleciona todos os registros após a data especificada para atualizar os valores de saldo
                comando.CommandText = "SELECT id FROM lancamentos WHERE data > @data and conta = @conta";

                // Inicia leitor de registros
                using (var reader = comando.ExecuteReader())
                {
                    // Criar comando para atualizar os campos
                    var sql = "UPDATE lancamentos SET saldo = (saldo + @valor) WHERE id = @id";
                    using (var comando2 = new SqliteCommand(sql, con.conn))
                    {
                        // Atribuir transação ao comando 2
                        comando2.Transaction = transacao;

                        // Variáveis para controle dos campos modificados
                        var totalCampos = 0;
                        var totalAlterado = 0;

                        // Para cada campo encontrado
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);

                            // Atualizar os campos no banco de dados
                            comando2.Parameters.Clear();
                            comando2.Parameters.AddWithValue("@valor", valor);
                            comando2.Parameters.AddWithValue("@id", id);

                            totalCampos++;
                            totalAlterado += comando2.ExecuteNonQuery();
                        }

                        // Testar se todos os registros foram alterados
                        if (totalCampos != totalAlterado)
                        {
                            throw new CustomException("Houve um erro na hora de atualizar os valores de saldo após a data informada");
                        }
                    }
                }
            }
            else
            {
                throw new CustomException("Não foi possível criar um novo lançamento, por favor, anote os dados inseridos e tire um print da tela para contatar o desenvolvedor");
            }

            // Após inserir o lançamento: fazer o registro do lançamento no caixa
            // Verificar se a data já existe
            comando.Parameters.Clear();
            comando.CommandText = "SELECT COUNT(*) FROM registros_caixa WHERE data = @data";
            comando.Parameters.AddWithValue("@data", dataConvertida);
            int count = Convert.ToInt32(comando.ExecuteScalar());

            // Se a data existe: atualizar o saldo
            if (count > 0)
            {
                comando.CommandText = "UPDATE registros_caixa SET saldo = saldo + @valor WHERE data = @data;";
                comando.Parameters.AddWithValue("@valor", valor);
            }
            // Se a data não existe: criar o novo registro
            else
            {
                // Obter valor de saldo anterior
                comando.CommandText = "SELECT COALESCE((SELECT saldo FROM registros_caixa WHERE data <= @data ORDER BY data DESC LIMIT 1), 0);";
                var saldoAnterior = Convert.ToDecimal(comando.ExecuteScalar());

                comando.CommandText = "INSERT INTO registros_caixa (data, saldo) VALUES (@data, @saldo);";
                comando.Parameters.AddWithValue("@saldo", (saldoAnterior + valor));
            }

            var resultado = comando.ExecuteNonQuery();
            frmLogin.testarResultadoComando(resultado, "Não foi possível atualizar o valor do caixa");

            // Seleciona todos os registros após a data especificada para atualizar os valores de saldo_anterior e saldo_atualizado (após)
            comando.CommandText = "SELECT data FROM registros_caixa WHERE data > @data;";

            // Inicia leitor de registros
            using (var reader = comando.ExecuteReader())
            {
                // Criar comando para atualizar os campos
                var sql = "UPDATE registros_caixa SET saldo = saldo + @valor WHERE data = @data;";
                using (var comando2 = new SqliteCommand(sql, con.conn))
                {
                    // Atribuir transação ao comando 3
                    comando2.Transaction = transacao;

                    // Variáveis para controle dos campos modificados
                    var totalCampos = 0;
                    var totalAlterado = 0;

                    // Para cada campo encontrado
                    while (reader.Read())
                    {
                        string data = reader["data"]?.ToString();

                        // Atualizar os campos no banco de dados
                        comando2.Parameters.Clear();
                        comando2.Parameters.AddWithValue("@valor", valor);
                        comando2.Parameters.AddWithValue("@data", data);

                        totalCampos++;
                        totalAlterado += comando2.ExecuteNonQuery();
                    }

                    // Testar se todos os registros foram alterados
                    if (totalCampos != totalAlterado)
                    {
                        throw new CustomException("Houve um erro na hora de atualizar os valores do caixa após a data informada");
                    }
                }
            }
        }

        private void btnCriar_Click(object sender, EventArgs e)
        {
            try
            {
                // Criar uma instância do formulário de dados e aguardar um retorno
                using (var frmDados = new frmLancamentosDados(con, "Criar lançamento", "", "", 0, "", "", DateTime.Today))
                {
                    // O usuário apertou o botão de salvar
                    if (frmDados.ShowDialog() == DialogResult.OK)
                    {
                        // Iniciar transação
                        using (var transacao = con.conn.BeginTransaction())
                        {
                            // Inciar o comando
                            using (var comando = new SqliteCommand("", con.conn))
                            {
                                // Atribuir o comando a transação
                                comando.Transaction = transacao;

                                try
                                {
                                    // Criar lançamento e atualizar valores de saldos posteriores e no registro de caixa
                                    criarLancamento(comando, transacao);

                                    // Efetivar operações
                                    transacao.Commit();

                                    // Adicionar dados na tabela - Recarregar completamente
                                    atualizarDataGrid();

                                    // Remover dados das variáveis
                                    conta = "";
                                    valor = 0;
                                    id_historico = "";
                                    data = DateTime.Today;

                                    MessageBox.Show("Lançamento criado com sucesso!", "Criação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                                catch (CustomException ex)
                                {
                                    transacao.Rollback();
                                    MessageBox.Show($"{ex.Message?.ToString()}", "Erro ao criar o lançamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                catch (Exception ex)
                                {
                                    transacao.Rollback();
                                    MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao criar o lançamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao criar o lançamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void btnCalcular_Click(object sender, EventArgs e)
        {
            try
            {
                // Variáveis para os dados
                string dataInicial, dataInicialFormatada, dataFinal, dataFinalFormatada = "";

                using (var formDados = new frmCalcularObterDados())
                {
                    var resultado = formDados.ShowDialog();

                    if (resultado == DialogResult.OK)
                    {
                        (dataInicial, dataInicialFormatada, dataFinal, dataFinalFormatada) = (formDados.dataInicial, formDados.dataInicialFormatada, formDados.dataFinal, formDados.dataFinalFormatada);
                    }
                    else if (resultado == DialogResult.Cancel)
                    {
                        return;
                    }
                    else
                    {
                        throw new CustomException("Houve um erro na hora de obter as datas informadas, por favor, tente novamente ou contate o desenvolvedor");
                    }
                }

                // Obter valores de crédito e débito de todos os lançamentos
                var sql = "SELECT SUM(CASE WHEN l.valor > 0 THEN l.valor ELSE 0 END) AS creditos, SUM(CASE WHEN l.valor < 0 THEN l.valor ELSE 0 END) AS debitos FROM lancamentos l WHERE l.data BETWEEN @dataInicial AND @dataFinal;";
                using (var comando = new SqliteCommand(sql, con.conn))
                {
                    comando.Parameters.AddWithValue("@dataInicial", dataInicial);
                    comando.Parameters.AddWithValue("@dataFinal", dataFinal);

                    // Criação de variáveis
                    var creditos = 0m;
                    var debitos = 0m;

                    using (var reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Obter valor de crédito
                            var valorBD = reader.GetOrdinal("creditos");
                            creditos = reader.IsDBNull(valorBD) ? 0 : reader.GetDecimal(valorBD);

                            // Obter valor de débito
                            valorBD = reader.GetOrdinal("debitos");
                            debitos = reader.IsDBNull(valorBD) ? 0 : reader.GetDecimal(valorBD);
                        }
                        else
                        {
                            throw new CustomException("Nenhum lançamento encontrado no periodo informado.");
                        }
                    }

                    // Obter saldo do caixa na data final (ou deixar como 0 se não possuir nenhum registro na data informada ou antes dela)
                    comando.CommandText = "SELECT data, saldo FROM registros_caixa WHERE data <= @dataFinal ORDER BY data DESC LIMIT 1;";

                    DateTime dataSaldoAtual = DateTime.MinValue;
                    decimal saldoFinal = 0m;

                    using (var reader2 = comando.ExecuteReader())
                    {
                        if (reader2.Read())
                        {
                            dataSaldoAtual = Convert.ToDateTime(reader2["data"]);
                            saldoFinal = reader2.GetDecimal(1);
                        }
                    }

                    // Verificar se obteve a data corretamente
                    if (dataSaldoAtual == DateTime.MinValue)
                    {
                        throw new CustomException("Houve ume erro na hora de obter a data do saldo mais recente, por favor, tente novamente ou contate o desenvolvedor");
                    }

                    // Limpar parâmetros para evitar erros
                    comando.Parameters.Clear();

                    // Verificar a data do saldo atualizado, obtido anteriormente 
                    var dataReferencia = DateTime.ParseExact(dataInicial, "yyyy-MM-dd", null);

                    // Comando para obter o saldo anterior
                    comando.CommandText = "SELECT COALESCE ((SELECT saldo FROM registros_caixa WHERE data < @data ORDER BY data DESC LIMIT 1), 0) AS saldo;";

                    // Se a data do saldo obtido for antes da inicial (referencia): obter o saldo antes da data do saldo obtido (ou igualar a 0 se não houver registros)
                    if (dataSaldoAtual < dataReferencia)
                    {
                        comando.Parameters.AddWithValue("@data", dataSaldoAtual.ToString("yyyy-MM-dd"));
                    }
                    // Se a data do saldo obtido for igual ou depois da inicial (referencia): obter o saldo antes da data inicial (referencia) (ou igualar a 0 se não houver registros)
                    else if (dataSaldoAtual == dataReferencia || dataSaldoAtual > dataReferencia)
                    {
                        comando.Parameters.AddWithValue("@data", dataInicial);
                    }
                    else
                    {
                        throw new CustomException("Houve um erro na hora de obter o saldo inicial do caixa, por favor, tente novamente ou contate o desenvolvedor");
                    }

                    var saldoAnterior = Convert.ToDecimal(comando.ExecuteScalar());

                    // Calcular balanço final do periodo
                    var balanco = creditos + debitos + saldoAnterior - saldoFinal;

                    // Exibir resultados ao usuário
                    var periodo = $"{dataInicialFormatada} a {dataFinalFormatada}";
                    var formExibir = new frmCalcularExibir(periodo, creditos, debitos, saldoAnterior, saldoFinal, balanco);
                    formExibir.ShowDialog();
                }
            }
            catch (CustomException ex)
            {
                MessageBox.Show($"{ex.Message?.ToString()}", "Erro ao criar o lançamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao calcular valores", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            try
            {
                // Verifica se uma linha foi selecionada
                if (dgvLancamentos.SelectedRows.Count > 0)
                {
                    // Obtem a linha selecionada
                    DataGridViewRow selectedRow = dgvLancamentos.SelectedRows[0];

                    // Obter valores do lançamento antigo (antes da edição)
                    var idAntigo = selectedRow.Cells["ID"].Value.ToString();
                    var dataAntigo = Convert.ToDateTime(selectedRow.Cells["Data"].Value);
                    var contaAntiga = selectedRow.Cells["Conta"].Value.ToString();
                    var descricaoAntiga = selectedRow.Cells["Descrição"].Value.ToString();
                    var valorAntigo = Convert.ToDecimal(selectedRow.Cells["Valor"].Value);
                    var id_historicoAntigo = selectedRow.Cells["ID_Historico"].Value.ToString();
                    var historicoAntigo = selectedRow.Cells["Histórico"].Value.ToString();

                    // Criar uma instância do formulário de dados e aguardar um retorno
                    using (var frmDados = new frmLancamentosDados(con, "Editar conta", contaAntiga, descricaoAntiga, valorAntigo, id_historicoAntigo, historicoAntigo, dataAntigo))
                    {
                        // O usuário apertou o botão de salvar
                        if (frmDados.ShowDialog() == DialogResult.OK)
                        {
                            // Iniciar transação
                            using (var transacao = con.conn.BeginTransaction())
                            {
                                // Inciar o comando
                                using (var comando = new SqliteCommand("", con.conn))
                                {
                                    // Atribuir o comando a transação
                                    comando.Transaction = transacao;

                                    try
                                    {
                                        // Excluir registro anterior e atualizar valoress
                                        excluirLancamento(con, idAntigo, dataAntigo.ToString("yyyy-MM-dd"), valorAntigo, transacao);

                                        // Converter dataNova para o formato do banco (sem as horas)
                                        var dataConvertida = data.ToString("yyyy-MM-dd");

                                        // Criar lançamento com os valores novos
                                        criarLancamento(comando, transacao);

                                        // Efetivar operações
                                        transacao.Commit();

                                        // Adicionar dados na tabela - Recarregar completamente
                                        atualizarDataGrid();

                                        // Remover dados das variáveis
                                        conta = "";
                                        valor = 0;
                                        id_historico = "";
                                        data = DateTime.Today;

                                        MessageBox.Show("Lançamento editado com sucesso!", "Edição bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                    }
                                    catch (CustomException ex)
                                    {
                                        transacao.Rollback();
                                        MessageBox.Show($"{ex.Message?.ToString()}", "Erro ao editar o lançamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    catch (Exception ex)
                                    {
                                        transacao.Rollback();
                                        MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao editar o lançamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao editar o lançamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificar se o usuário realmente quer excluir o lançamento
                var dialogResult = MessageBox.Show("Deseja realmente excluir esse lançamento? Essa operação não poderá ser desfeita!", "Confirmação de exclusão do lançamento", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Yes)
                {
                    // Verifica se uma linha foi selecionada
                    if (dgvLancamentos.SelectedRows.Count > 0)
                    {
                        // Obtem a linha selecionada
                        DataGridViewRow selectedRow = dgvLancamentos.SelectedRows[0];

                        // Obtem os valores do lançamento a ser excluido
                        var idExcluir = selectedRow.Cells["ID"].Value.ToString();
                        var dataExcluir = Convert.ToDateTime(selectedRow.Cells["Data"].Value);
                        var valorAntigo = Convert.ToDecimal(selectedRow.Cells["Valor"].Value);

                        using (var transacao = con.conn.BeginTransaction())
                        {
                            try
                            {
                                using (var comando = new SqliteCommand("", con.conn))
                                {
                                    comando.Transaction = transacao;

                                    excluirLancamento(con, idExcluir, dataExcluir.ToString("yyyy-MM-dd"), valorAntigo, transacao);

                                    // Efetivar alterações
                                    transacao.Commit();

                                    // Adicionar dados na tabela - Recarregar completamente
                                    atualizarDataGrid();

                                    MessageBox.Show("Registro excluido com sucesso.", "Operação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            catch (CustomException ex)
                            {
                                transacao.Rollback();
                                MessageBox.Show($"{ex.Message?.ToString()}", "Erro ao excluir o lançamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            catch (Exception ex)
                            {
                                transacao.Rollback();
                                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao excluir o lançamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao excluir o lançamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
