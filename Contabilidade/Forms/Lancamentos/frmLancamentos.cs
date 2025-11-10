using Contabilidade.Models;
using System.Data;
using System.Data.SQLite;
using Contabilidade.Classes;
using Contabilidade.Forms.Cadastros;

namespace Contabilidade.Forms.Lancamentos
{
    public partial class frmLancamentos : Form
    {
        Conexao con;
        static DataTable dtDados = new DataTable();
        DataView dv = dtDados.DefaultView;
        public static string conta { get; set; } = "";
        public static int valor { get; set; } = 0;
        public static string id_historico { get; set; } = "";
        public static DateTime data { get; set; } = DateTime.MinValue;

        public frmLancamentos(Conexao conexaoBanco)
        {
            InitializeComponent();

            con = conexaoBanco;

            configurarColunaValor();
            atualizarDataGrid();

            dgvLancamentos.Columns["ID"].Visible = false;

            txtFiltrar.Select();
        }

        private void configurarColunaValor()
        {
            // Redefinindo o tipo da coluna 'valor' para decimal
            if (dtDados.Columns.Contains("valor"))
            {
                dtDados.Columns.Remove("valor");
            }
            dtDados.Columns.Add("valor", typeof(decimal));

            // Definir a posição da coluna "valor" como a quarta coluna (índice 3)
            dtDados.Columns["valor"].SetOrdinal(0);

            // Exibir 2 casas decimais no DataGridView
            dgvLancamentos.Columns["valor"].DefaultCellStyle.Format = "N2";

            // Ajustar tamanho da coluna
            dgvLancamentos.Columns["valor"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCellsExceptHeader;
        }


        private void atualizarDataGrid()
        {
            // Query de pesquisa
            string sql = "SELECT l.id, l.conta, c.descricao, (l.valor / 100.0) as valor, l.data, l.id_historico, h.historico FROM lancamentos l JOIN contas c ON l.conta = c.conta JOIN historicos h ON l.id_historico = h.id ORDER BY l.data DESC, l.id DESC;";
            using (var command = new SQLiteCommand(sql, con.conn))
            {
                // Alterar filtros somente se for diferente - para não acionar os handlers de forma desnecessária
                if (cbbFiltrar.SelectedIndex != 0)
                {
                    cbbFiltrar.SelectedIndex = 0;
                }
                if (cbbFiltrarDatas.SelectedIndex != 0)
                {
                    cbbFiltrarDatas.SelectedIndex = 0;
                }
                if (cbbFiltrarValores.SelectedIndex != 0)
                {
                    cbbFiltrarValores.SelectedIndex = 0;
                }

                dgvLancamentos.DataSource = dtDados;

                dtDados.Clear();
                using (var reader = command.ExecuteReader())
                {
                    dtDados.Load(reader);
                }

                txtFiltrar.Text = "";
                dv.RowFilter = $"conta LIKE '{txtFiltrar.Text}%'";
                dgvLancamentos.DataSource = dv;
            }
        }

        public static void reverterLancamentos(Conexao con, SQLiteCommand comando, int valorOriginal, SQLiteTransaction transacao)
        {
            using (var reader = comando.ExecuteReader())
            {
                // Criar comando para reverter os registros
                var sql2 = "UPDATE lancamentos SET saldo = (saldo - @valor) WHERE id = @id";
                using (var comando2 = new SQLiteCommand(sql2, con.conn))
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

        public static void excluirLancamento(Conexao con, string ID, string dataConvertida, SQLiteTransaction transacao)
        {
            using (var comando = new SQLiteCommand("", con.conn))
            {
                comando.Transaction = transacao;

                // Obter valor e conta do lançamento
                comando.CommandText = "SELECT valor, conta FROM lancamentos WHERE id = @id;";
                comando.Parameters.AddWithValue("@id", ID);

                var valorLancamento = 0;
                string contaLancamento = null;

                using (var reader = comando.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        valorLancamento = Convert.ToInt32(reader["valor"]);
                        contaLancamento = reader["conta"].ToString();
                    }
                    else
                    {
                        throw new CustomException("Não foi possível obter o valor e a conta do lançamento, tente novamente ou contate o desenvolvedor do sistema.");
                    }
                }

                comando.Parameters.Clear();

                // Reverter valores do lançamento NOS SALDOS das DATAS do lançamento -> atualizar saldos nos lançamentos seguintes de mesma data
                comando.CommandText = "SELECT id FROM lancamentos WHERE conta = @conta AND data = @data AND id > @id;";
                comando.Parameters.AddWithValue("@data", dataConvertida);
                comando.Parameters.AddWithValue("@id", ID);
                comando.Parameters.AddWithValue("@conta", contaLancamento);
                reverterLancamentos(con, comando, valorLancamento, transacao);

                // Reverter valor do lançamento NOS SALDOS das DATAS POSTERIORES -> atualizar saldos dos lançamentos seguintes de datas posteriores
                comando.CommandText = "SELECT id FROM lancamentos WHERE conta = @conta AND data > @data";
                reverterLancamentos(con, comando, valorLancamento, transacao);

                // Reverter valor do lançamento nos REGISTROS DO CAIXA nas datas IGUAIS OU POSTERIORES a do lançamento
                comando.CommandText = "UPDATE registros_caixa SET saldo = (saldo - @valor) WHERE data >= @data;";
                comando.Parameters.AddWithValue("@valor", valorLancamento);
                comando.ExecuteNonQuery();

                // Excluir lançamento
                comando.CommandText = "DELETE FROM lancamentos WHERE id = @id;";
                var resultado = comando.ExecuteNonQuery();

                if (resultado == 0)
                {
                    throw new CustomException("Não foi possivel excluir o lançamento");
                }
            }
        }

        public void criarLancamento(SQLiteCommand comando, SQLiteTransaction transacao)
        {
            // Variável para armazenar o saldo da conta antes do lançamento
            int saldo = 0;

            // Verificar se a data obtida não é a padrão (se sim: houve um erro na hora de obter os dados)
            if (DateTime.MinValue == data)
            {
                MessageBox.Show("Houve um erro na hora de obter a data informada", "Erro ao processar dados", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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
                saldo = Convert.ToInt32(result);
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
                    using (var comando2 = new SQLiteCommand(sql, con.conn))
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
                var saldoAnterior = Convert.ToInt32(comando.ExecuteScalar());

                comando.CommandText = "INSERT INTO registros_caixa (data, saldo) VALUES (@data, @saldo);";
                comando.Parameters.AddWithValue("@saldo", saldoAnterior + valor);
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
                using (var comando2 = new SQLiteCommand(sql, con.conn))
                {
                    // Atribuir transação ao comando 2
                    comando2.Transaction = transacao;

                    // Variáveis para controle dos campos modificados
                    var totalCampos = 0;
                    var totalAlterado = 0;

                    // Para cada campo encontrado
                    while (reader.Read())
                    {
                        string data = Convert.ToDateTime(reader["data"]).ToString("yyyy-MM-dd");

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
                // Variável para controle de fluxo (se irá abrir novamente ou não)
                var dialogResult = DialogResult.Cancel;

                do
                {
                    // Criar uma instância do formulário de dados e aguardar um retorno
                    using (var frmDados = new frmLancamentosDados(con, "Criar lançamento", "", "", 0, "", "", DateTime.Today))
                    {
                        dialogResult = frmDados.ShowDialog();

                        // O usuário apertou o botão de salvar
                        if (dialogResult == DialogResult.OK)
                        {
                            // Iniciar transação
                            using (var transacao = con.conn.BeginTransaction())
                            {
                                // Inciar o comando
                                using (var comando = new SQLiteCommand("", con.conn))
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
                                        data = DateTime.MinValue;
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
                } while (dialogResult == DialogResult.OK);
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

                // Verificar se existe pelo menos 1 lançamento entre as datas informadas
                var sql = "SELECT EXISTS(SELECT 1 FROM lancamentos WHERE data BETWEEN @dataInicial AND @dataFinal);";
                using (var comando = new SQLiteCommand(sql, con.conn))
                {
                    comando.Parameters.AddWithValue("@dataInicial", dataInicial);
                    comando.Parameters.AddWithValue("@dataFinal", dataFinal);

                    bool existeLancamentos = Convert.ToBoolean(comando.ExecuteScalar());

                    if (!existeLancamentos)
                    {
                        throw new CustomException("Nenhum lançamento encontrado no periodo informado.");
                    }

                    // Obter valores de crédito e débito de todos os lançamentos
                    comando.CommandText = "SELECT SUM(CASE WHEN l.valor > 0 THEN l.valor ELSE 0 END) AS creditos, SUM(CASE WHEN l.valor < 0 THEN l.valor ELSE 0 END) AS debitos FROM lancamentos l WHERE l.data BETWEEN @dataInicial AND @dataFinal;";

                    // Criação de variáveis
                    var creditos = 0;
                    var debitos = 0;

                    using (var reader = comando.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Obter valor de crédito
                            var valorBD = reader.GetOrdinal("creditos");
                            creditos = reader.IsDBNull(valorBD) ? 0 : reader.GetInt32(valorBD);

                            // Obter valor de débito
                            valorBD = reader.GetOrdinal("debitos");
                            debitos = reader.IsDBNull(valorBD) ? 0 : reader.GetInt32(valorBD);
                        }
                        else
                        {
                            throw new CustomException("Nenhum lançamento encontrado no periodo informado.");
                        }
                    }

                    // Obter saldo do caixa na data final (ou obter valores na primeira data antes dela)
                    comando.CommandText = "SELECT data, saldo FROM registros_caixa WHERE data <= @dataFinal ORDER BY data DESC LIMIT 1;";

                    DateTime dataSaldoAtual = DateTime.MinValue;
                    var saldoFinal = 0;

                    using (var reader2 = comando.ExecuteReader())
                    {
                        if (reader2.Read())
                        {
                            dataSaldoAtual = Convert.ToDateTime(reader2["data"]);
                            saldoFinal = reader2.GetInt32(1);
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

                    var saldoAnterior = Convert.ToInt32(comando.ExecuteScalar());

                    // Calcular balanço final do periodo
                    var balanco = creditos + debitos + saldoAnterior - saldoFinal;

                    // Converter para decimal
                    decimal balancoDecimal = balanco / 100m;
                    decimal creditosDecimal = creditos / 100m;
                    decimal debitosDecimal = debitos / 100m;
                    decimal saldoAnteriorDecimal = saldoAnterior / 100m;
                    decimal saldoFinalDecimal = saldoFinal / 100m;

                    // Exibir resultados ao usuário
                    var periodo = $"{dataInicialFormatada} a {dataFinalFormatada}";
                    var formExibir = new frmCalcularExibir(periodo, creditosDecimal, debitosDecimal, saldoAnteriorDecimal, saldoFinalDecimal, balancoDecimal);
                    formExibir.ShowDialog();
                }
            }
            catch (CustomException ex)
            {
                MessageBox.Show($"{ex.Message?.ToString()}", "Erro ao calcular os valores de lançamentos", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    var valorAntigo = Convert.ToInt32(selectedRow.Cells["Valor"].Value);
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
                                using (var comando = new SQLiteCommand("", con.conn))
                                {
                                    // Atribuir o comando a transação
                                    comando.Transaction = transacao;

                                    try
                                    {
                                        // Excluir registro anterior e atualizar valoress
                                        excluirLancamento(con, idAntigo, dataAntigo.ToString("yyyy-MM-dd"), transacao);

                                        // Verificar se a data obtida não é a padrão (se sim: houve um erro na hora de obter os dados)
                                        if (DateTime.MinValue == data)
                                        {
                                            throw new CustomException("Houve um erro na hora de obter a data informada");
                                        }

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
                                        data = DateTime.MinValue;

                                        MessageBox.Show("Lançamento editado com sucesso!", "Edição bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                        using (var transacao = con.conn.BeginTransaction())
                        {
                            try
                            {
                                using (var comando = new SQLiteCommand("", con.conn))
                                {
                                    comando.Transaction = transacao;

                                    excluirLancamento(con, idExcluir, dataExcluir.ToString("yyyy-MM-dd"), transacao);

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

        private void handleFiltroPrincipal()
        {
            resetarTodosFiltros();

            var indexPrincipal = cbbFiltrar.SelectedIndex;

            switch (indexPrincipal)
            {
                // Conta
                case 1:
                    txtFiltrar.Visible = true;
                    txtFiltrar.MaxLength = 15;
                    break;
                // Descrição
                case 2:
                    txtFiltrar.Visible = true;
                    txtFiltrar.MaxLength = 100;
                    break;
                // Histórico
                case 3:
                    txtFiltrar.Visible = true;
                    txtFiltrar.MaxLength = 300;
                    break;
                // Data
                case 4:
                    cbbFiltrar.Width = 162;
                    cbbFiltrarDatas.Visible = true;
                    break;

                // Valores
                case 5:
                    cbbFiltrar.Width = 162;
                    cbbFiltrarValores.Visible = true;
                    break;
            }
        }

        private void resetarTodosFiltros()
        {
            // Esconder todos os campos/filtros secundários que não são padrões
            cbbFiltrarDatas.Visible = false;
            cbbFiltrarValores.Visible = false;
            esconderCampos();

            // Resetar o tamanho dos filtros/campos que sofrem mudanças para o padrão
            cbbFiltrar.Width = 332;
            resetarTamanhoCampos();

            // Resetar valores de todos os filtros e campos - com exceção do filtro principal
            // Alterar index dos filtros apenas se for diferente do padrão - assim não aciona o handleFiltroSecundario
            if (cbbFiltrarDatas.SelectedIndex != 0)
            {
                cbbFiltrarDatas.SelectedIndex = 0;
            }
            if (cbbFiltrarValores.SelectedIndex != 0)
            {
                cbbFiltrarValores.SelectedIndex = 0;
            }
            resetarValoresCampos();

            filtrarPor();
        }

        private void filtrarPor(string query = "")
        {
            dv.RowFilter = $"{query}";
            dgvLancamentos.DataSource = dv;
        }

        private void esconderCampos()
        {
            txtFiltrar.Visible = false;
            dtpData1.Visible = false;
            dtpData2.Visible = false;
            nudValor1.Visible = false;
            nudValor2.Visible = false;
        }

        private void resetarTamanhoCampos()
        {
            txtFiltrar.Width = 332;
            dtpData1.Width = 332;
            nudValor1.Width = 332;
        }

        private void resetarValoresCampos()
        {
            // Apenas alterar valor caso esteja diferente, assim não se ativa o handleMudancaValor sem necessidade
            if (txtFiltrar.Text != "")
            {
                txtFiltrar.Text = "";
            }
            if (dtpData1.Value != DateTime.Today)
            {
                dtpData1.Value = DateTime.Today;
            }
            if (dtpData2.Value != DateTime.Today)
            {
                dtpData2.Value = DateTime.Today;
            }
            if (nudValor1.Value != 0m)
            {
                nudValor1.Value = 0m;
            }
            if (nudValor2.Value != 0m)
            {
                nudValor2.Value = 0m;
            }
        }

        private void handleFiltroSecundario(int indexPrincipal)
        {
            esconderCampos();
            resetarTamanhoCampos();
            resetarValoresCampos();

            // Datas
            if (indexPrincipal == 4)
            {
                var indexSecundario = cbbFiltrarDatas.SelectedIndex;

                switch (indexSecundario)
                {
                    // > Filtros sem campos
                    // Sem filtro - apenas resetar os filtros
                    case 0:
                        filtrarPor();
                        break;

                    // > Filtros com apenas 1 campo
                    // Data igual a
                    // Datas anteriores a
                    // Datas posteriores a
                    case 1:
                    case 2:
                    case 3:
                        dtpData1.Visible = true;
                        break;

                    // > Filtros com 2 campos
                    // Datas entre
                    case 4:
                        dtpData1.Width = 162;
                        dtpData1.Visible = true;
                        dtpData2.Visible = true;
                        break;
                }
            }
            // Valores
            else if (indexPrincipal == 5)
            {
                var indexSecundario = cbbFiltrarValores.SelectedIndex;

                switch (indexSecundario)
                {
                    // > Filtros sem campos
                    // Sem filtro - apenas resetar os filtros
                    case 0:
                        filtrarPor();
                        break;
                    // Somente débitos
                    case 1:
                        dv.RowFilter = "valor < 0";
                        dgvLancamentos.DataSource = dv;
                        break;
                    // Somente créditos
                    case 2:
                        dv.RowFilter = "valor > 0";
                        dgvLancamentos.DataSource = dv;
                        break;

                    // > Filtros com apenas 1 campo
                    // Valores igual a
                    // Valores menores que
                    // Valores maiores que
                    case 3:
                    case 4:
                    case 5:
                        filtrarPor();
                        nudValor1.Visible = true;
                        break;

                    // > Filtros com 2 campos
                    // Valores entre
                    case 6:
                        filtrarPor();
                        nudValor1.Width = 162;
                        nudValor1.Visible = true;
                        nudValor2.Visible = true;
                        break;
                }
            }
        }

        private void cbbFiltrar_SelectedIndexChanged(object sender, EventArgs e)
        {
            handleFiltroPrincipal();
        }

        private void cbbFiltrarDatas_SelectedIndexChanged(object sender, EventArgs e)
        {
            handleFiltroSecundario(4);
        }

        private void cbbFiltrarValores_SelectedIndexChanged(object sender, EventArgs e)
        {
            handleFiltroSecundario(5);
        }

        public static (string, string) ordenarDatasEObterStrings(DateTime data1, DateTime data2)
        {
            // Se a data1 for menor ou igual a data2
            if (data1 <= data2)
            {
                return (data1.ToString("yyyy-MM-dd"), data2.ToString("yyyy-MM-dd"));
            }
            // Se a data2 for menor que a data1
            else
            {
                return (data2.ToString("yyyy-MM-dd"), data1.ToString("yyyy-MM-dd"));
            }
        }

        public static (decimal, decimal) ordenarValoresDecimais(decimal valor1, decimal valor2)
        {
            if (valor1 <= valor2)
            {
                return (valor1, valor2);
            }
            else
            {
                return (valor2, valor1);
            }
        }

        private void handleMudancaValor()
        {
            var indexPrincipal = cbbFiltrar.SelectedIndex;

            switch (indexPrincipal)
            {
                // Conta
                case 1:
                    var conta = txtFiltrar.Text;
                    filtrarPor($"conta LIKE '{conta}%'");
                    break;
                // Descrição
                case 2:
                    var descricao = txtFiltrar.Text;
                    filtrarPor($"descricao LIKE '%{descricao}%'");
                    break;
                // Histórico
                case 3:
                    var historico = txtFiltrar.Text;
                    filtrarPor($"historico LIKE '%{historico}%'");
                    break;
                // Data
                case 4:
                    var indexSecundario = cbbFiltrarDatas.SelectedIndex;

                    switch (indexSecundario)
                    {
                        // Data igual a
                        case 1:
                            var dataIgual = dtpData1.Value.ToString("yyyy-MM-dd");
                            filtrarPor($"data = '{dataIgual}'");
                            break;
                        // Datas anteriores a
                        case 2:
                            var dataAnterior = dtpData1.Value.ToString("yyyy-MM-dd");
                            filtrarPor($"data < '{dataAnterior}'");
                            break;
                        // Datas posteriores a
                        case 3:
                            var dataPosterior = dtpData1.Value.ToString("yyy-MM-dd");
                            filtrarPor($"data > '{dataPosterior}'");
                            break;
                        // Datas entre
                        case 4:
                            var (data1, data2) = ordenarDatasEObterStrings(dtpData1.Value, dtpData2.Value);
                            filtrarPor($"data >= '{data1}' AND data <= '{data2}'");
                            break;
                    }
                    break;
                // Valores
                case 5:
                    var indexSecundario2 = cbbFiltrarValores.SelectedIndex;

                    switch (indexSecundario2)
                    {
                        // Valores iguais a
                        case 3:
                            var valorIgual = nudValor1.Value;
                            filtrarPor($"valor = {valorIgual}");
                            break;
                        // Valores menores que
                        case 4:
                            var valorMenor = nudValor1.Value;
                            filtrarPor($"valor <= {valorMenor}");
                            break;
                        // Valores maiores que
                        case 5:
                            var valorMaior = nudValor1.Value;
                            filtrarPor($"valor >= {valorMaior}");
                            break;
                        // Valores entre
                        case 6:
                            var (valor1, valor2) = ordenarValoresDecimais(nudValor1.Value, nudValor2.Value);
                            filtrarPor($"valor >= {valor1} AND valor <= {valor2}");
                            break;
                    }
                    break;
            }
        }

        private void txtFiltrar_TextChanged(object sender, EventArgs e)
        {
            // Impedir da máscara ser aplicada quando não se tem dados inseridos (o que ocasiona erro) ou quando o filtro não é de conta
            if (txtFiltrar.Text.Length > 0 && cbbFiltrar.SelectedIndex == 1)
            {
                TextBox textBox = sender as TextBox;
                textBox.Text = frmContasDados.AplicarMascara(textBox.Text);
                textBox.SelectionStart = textBox.Text.Length; // Mantém o cursor no final
            }

            handleMudancaValor();
        }

        private void dtpData1_ValueChanged(object sender, EventArgs e)
        {
            handleMudancaValor();
        }

        private void dtpData2_ValueChanged(object sender, EventArgs e)
        {
            handleMudancaValor();
        }

        private void nudValor1_ValueChanged(object sender, EventArgs e)
        {
            handleMudancaValor();
        }

        private void nudValor2_ValueChanged(object sender, EventArgs e)
        {
            handleMudancaValor();
        }

        private void refazerLancamentos(SQLiteCommand comando)
        {
            // Obter número de todas contas
            comando.CommandText = ("SELECT conta FROM contas WHERE nivel = 'A' and conta != 0");
            contasReader = comando.ExecuteReader();

            // Para cada conta
            while (contasReader.Read())
            {
                // Obter todos lançamentos
                var lancamentos = new List<(int id, int valor)>();
                var conta = contasReader.GetInt32(0);
                int saldo = 0;

                var sql = "SELECT id, valor FROM lancamentos WHERE conta = @conta ORDER BY data ASC, id ASC;";
                using (var comando2 = new SQLiteCommand(sql, connection))
                {
                    comando2.Parameters.AddWithValue("@conta", conta);

                    using (var lancamentosReader = comando2.ExecuteReader())
                    {
                        while (lancamentosReader.Read())
                        {
                            lancamentos.Add((
                                lancamentosReader.GetInt32(0),
                                lancamentosReader.GetInt32(1)
                            ));
                        }
                    }
                    
                    // Atualizar todos lançamentos daquela conta
                    foreach (var lancamento in lancamentos)
                    {
                        saldo += lancamento.valor;

                        comando2.CommandText = "UPDATE lancamentos SET saldo = @saldo WHERE id = @id";
                        
                        comando2.Parameters.Clear();
                        comando2.Parameters.AddWithValue("@saldo", saldo);
                        comando2.Parameters.AddWithValue("@id", lancamento.id);

                        comando2.ExecuteNonQuery();
                    }
                }
            }
        }

        private void refazerCaixa(SQLiteCommand comando)
        {

            // Remover todos os valores antigos do caixa
            comando.CommandText = "DELETE FROM registros_caixa;";
            comando.ExecuteNonQuery();

            // Obter a soma de todos os lançamentos em cada data individual
            var lancamentosPorData = new List<(string data, int total)>();
            comando.CommandText = "SELECT data, SUM(valor) AS total FROM lancamentos GROUP BY data ORDER BY data ASC;";
            comando.Parameters.Clear();

            using (var reader = comando.ExecuteReader())
            {
                while (reader.Read())
                {
                    var dataObj = reader["data"];
                    var totalObj = reader["total"];

                    if (dataObj == DBNull.Value || totalObj == DBNull.Value)
                        continue;

                    var dataStr = dataObj.ToString("yyyy-MM-dd");
                    var total = Convert.ToInt32(totalObj);

                    lancamentosPorData.Add((dataStr, total));
                }
            }

            // Se não encontrou nenhum lançamento, termina a execução, já que o caixa não terá nenhum registro
            if (lancamentosPorData.Count == 0)
            {
                return;
            }

            // Inserir os valores novamente no caixa
            comando.CommandText = "INSERT INTO registros_caixa (data, saldo) VALUES (@data, @saldo);";
            comando.Parameters.Clear();

            int saldoAcumulado = 0;
            foreach (var item in lancamentosPorData)
            {
                saldoAcumulado += item.total;

                comando.Parameters.Clear();
                comando.Parameters.AddWithValue("@data", item.data);
                comando.Parameters.AddWithValue("@saldo", saldoAcumulado);

                comando.ExecuteNonQuery();
            }
        }

        private int obterSaldoAtualCaixa(Conexao con)
        {
            var saldoCaixa = 0;
            const string sql = "SELECT COALESCE(saldo, 0) FROM registros_caixa ORDER BY data DESC LIMIT 1;";
            using (var comando = new SQLiteCommand(sql, con.conn))
            {
                var result = comando.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    saldoCaixa = Convert.ToInt32(result);
                }
            }

            return saldoCaixa;
        }

        private int obterSomaTodosSaldos(Conexao con)
        {
            var somaTodosSaldos = 0;
            const string sql = "SELECT COALESCE(SUM(saldo), 0) AS total_saldos FROM (SELECT saldo FROM (SELECT conta, data, saldo, ROW_NUMBER() OVER (PARTITION BY conta ORDER BY data DESC, id DESC) AS rn FROM lancamentos) AS ultimos WHERE rn = 1) AS saldos_por_conta;";

            using (var comando = new SQLiteCommand(sql, con.conn))
            {
                var result = comando.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    somaTodosSaldos = Convert.ToInt32(result);
                }
            }

            return somaTodosSaldos;
        }

        private int obterSomaTodosLancamentos(Conexao con)
        {
            var somaTodosLancamentos = 0;
            const string sql = "SELECT COALESCE(SUM(valor), 0) FROM lancamentos;";
            using (var comando = new SQLiteCommand(sql, con.conn))
            {
                var result = comando.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    somaTodosLancamentos = Convert.ToInt32(result);
                }
            }

            return somaTodosLancamentos;
        }

        private void btnRecalcular_Click(object sender, EventArgs e)
        {
            try
            {
                // Verificar se o usuário realmente quer recalcular os lançamentos
                var dialogResult = MessageBox.Show("Deseja recalcular todos os valores dos lançamentos? Essa operação pode demorar e não poderá ser desfeita!", "Confirmação de recálculo do lançamento", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (dialogResult == DialogResult.Yes)
                {
                    dialogResult = MessageBox.Show("Você deseja realmente recalcular todos os valores dos lançamentos? Essa é a última confirmação, faça um backup antes de prosseguir!", "Confirmação de recálculo do lançamento", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (dialogResult == DialogResult.Yes)
                    {
                        int saldoCaixa = obterSaldoAtualCaixa();
                        int somaTodosSaldos = obterSomaTodosSaldos();
                        int somaTodosLancamentos = obterSomaTodosLancamentos();

                        // Testar integridade dos dados
                        bool refazerCaixa = saldoCaixa == somaTodosLancamentos ? false : true;
                        bool refazerLancamentos = somaTodosSaldos == somaTodosLancamentos ? false : true;

                        // Se estiver integro, finaliza com mensagem
                        if (!refazerCaixa && !refazerLancamentos)
                        {
                            MessageBox.Show("Todos os valores foram verificados e não precisou ser feita nenhuma correção.", "Operação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        // Se houver problemas, refazer o que for necessário
                        using (var comando = new SQLiteCommand("", con.conn))
                        {
                            using (var transacao = con.conn.BeginTransaction())
                            {
                                try
                                {
                                    comando.Transaction = transacao;

                                    if (refazerCaixa)
                                    {
                                        refazerCaixa(comando);
                                    }

                                    if (refazerLancamentos)
                                    {
                                        refazerLancamentos(comando);
                                    }

                                    // Verificar novamente a integridade dos dados
                                    saldoCaixa = obterSaldoAtualCaixa();
                                    somaTodosSaldos = obterSomaTodosSaldos();
                                    // A soma de todos lançamentos permanece igual

                                    refazerCaixa = saldoCaixa == somaTodosLancamentos ? false : true;
                                    refazerLancamentos = somaTodosSaldos == somaTodosLancamentos ? false : true;

                                    // Se estiver integro, finaliza com sucesso
                                    if (!refazerCaixa && !refazerLancamentos)
                                    {
                                        transacao.Commit();
                                        MessageBox.Show("A verificação encontrou valores incorretos e corrigiu as inconsistências com sucesso.", "Operação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    // Se não, rollback e erro
                                    else
                                    {
                                        transacao.Rollback();
                                        MessageBox.Show("A verificação encontrou valores incorretos, mas não foi possível corrigir as inconsistências. Contate o desenvolvedor do sistema.", "A operação falhou", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                }
                                catch (CustomException ex)
                                {
                                    transacao.Rollback();
                                    MessageBox.Show($"{ex.Message?.ToString()}", "Erro ao recalcular os valores de lançamentos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                                catch (Exception ex)
                                {
                                    transacao.Rollback();
                                    MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao recalcular valores", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao recalcular valores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
