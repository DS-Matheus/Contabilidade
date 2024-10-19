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
            string sql = "SELECT l.id, l.conta, c.descricao, l.valor, l.data, h.historico FROM lancamentos l JOIN contas c ON l.conta = c.conta JOIN historicos h ON l.id_historico = h.id ORDER BY l.conta, l.data";
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

        private void btnCriar_Click(object sender, EventArgs e)
        {
            // Criar uma instância do formulário de dados e aguardar um retorno
            using (var frmDados = new frmLancamentosDados(con, "Criar conta", "", "", 0, "", "", DateTime.Today))
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
                                    // Seleciona todos os registros após a data especificada para atualizar os valores de saldo_anterior e saldo_atualizado (após)
                                    // comando.CommandText = "SELECT id, saldo_anterior, saldo_atualizado FROM lancamentos WHERE data > @data and conta = @conta";
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
    }
}
