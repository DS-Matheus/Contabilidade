using Contabilidade.Models;
using DGVPrinterHelper;
using System.Data;
using Microsoft.Data.Sqlite;
using Contabilidade.Classes;
using System.Drawing;

namespace Contabilidade.Forms.Cadastros
{
    public partial class frmHistoricos : Form
    {
        Conexao con;
        static DataTable dtDados = new DataTable();
        DataView dv = dtDados.DefaultView;
        public static string historico { get; set; } = "";

        public frmHistoricos(Conexao conexaoBanco)
        {
            InitializeComponent();

            con = conexaoBanco;

            atualizarDataGrid();
        }

        public void atualizarDataGrid()
        {
            // Query de pesquisa
            string sql = "SELECT * FROM historicos;";
            using (var command = new SqliteCommand(sql, con.conn))
            {
                dtDados.Clear();
                using (var reader = command.ExecuteReader())
                {
                    dtDados.Load(reader);
                }

                dgvHistoricos.DataSource = dtDados;

                dv.RowFilter = $"historico LIKE '{txtFiltrar.Text}%'";
                dgvHistoricos.DataSource = dv;
            }
        }

        public static bool verificarExistenciaHistorico(string historico)
        {
            return dtDados.AsEnumerable().Any(row => historico == row.Field<string>("historico"));
        }

        private void btnCriar_Click(object sender, EventArgs e)
        {
            // Criar uma instância do formulário de dados e aguardar um retorno
            using (var frmDados = new frmHistoricosDados("Criar histórico", ""))
            {
                // O usuário apertou o botão de salvar
                if (frmDados.ShowDialog() == DialogResult.OK)
                {
                    // Criar histórico
                    string sql = "INSERT INTO historicos (historico) VALUES(@historico);";
                    using (var comando = new SqliteCommand(sql, con.conn))
                    {
                        try
                        {
                            comando.Parameters.AddWithValue("@historico", historico);

                            int retornoBD = comando.ExecuteNonQuery();

                            // Verificar se houve a criação da linha (0 = negativo)
                            if (retornoBD > 0)
                            {
                                using (var command = new SqliteCommand("SELECT last_insert_rowid();", con.conn))
                                {
                                    var id = (Int64)command.ExecuteScalar();

                                    // Adicionar dados na tabela
                                    DataRow row = dtDados.NewRow();
                                    row["id"] = id;
                                    row["historico"] = historico;
                                    dtDados.Rows.Add(row);

                                    dgvHistoricos.Refresh();

                                    // Remover dados das variáveis
                                    historico = "";

                                    MessageBox.Show("Histórico criado com sucesso!", "Criação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                                }
                            }
                            else
                            {
                                throw new Exception("Não foi possível criar o novo histórico.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), "Histórico não criado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        private void txtFiltrar_TextChanged(object sender, EventArgs e)
        {
            dv.RowFilter = $"historico LIKE '%{txtFiltrar.Text}%'";
            dgvHistoricos.DataSource = dv;
        }

        private string obterDadosDGV(int numLinha)
        {
            string historico = dgvHistoricos.Rows[numLinha].Cells["Histórico"].Value?.ToString();

            return historico;
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Obter histórico selecionado
            int numLinha = frmUsuarios.obterNumLinhaSelecionada(dgvHistoricos);
            var id = dgvHistoricos.Rows[numLinha].Cells["ID"].Value;
            var historicoAntigo = obterDadosDGV(numLinha);

            // Criar uma instância do formulário de dados e aguardar um retorno
            using (var frmDados = new frmHistoricosDados("Editar usuário", historicoAntigo))
            {
                // O usuário apertou o botão de salvar
                if (frmDados.ShowDialog() == DialogResult.OK)
                {
                    // Editar histórico
                    using (var comando = new SqliteCommand("UPDATE historicos SET historico = @historico WHERE id = @id;", con.conn))
                    {
                        try
                        {
                            comando.Parameters.AddWithValue("@historico", historico);
                            comando.Parameters.AddWithValue("@id", id);

                            int retornoBD = comando.ExecuteNonQuery();

                            // Verificar se houve a edição de alguma linha (0 = negativo)
                            if (retornoBD > 0)
                            {
                                // Atualizar DataTable
                                dgvHistoricos.Rows[numLinha].Cells["Histórico"].Value = historico;

                                dgvHistoricos.Refresh();

                                // Remover dados das variáveis
                                historico = "";

                                MessageBox.Show("Histórico editado com sucesso!", "Edição bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            else
                            {
                                throw new Exception("Não foi possível encontrar o histórico ou ocorreu um erro na edição.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message.ToString(), "Exclusão não realizada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                string title = string.IsNullOrWhiteSpace(inputTitle) ? "Históricos Cadastrados" : inputTitle;

                var printer = new DGVPrinter();
                printer.Title = title; // Usa o título fornecido pelo usuário
                printer.SubTitle = string.Format("Data: {0}", System.DateTime.Now.ToString("dd/MM/yyyy"));
                printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                printer.PageNumbers = true;
                printer.PageNumberInHeader = false;
                printer.PorportionalColumns = true;
                printer.HeaderCellAlignment = StringAlignment.Near;
                printer.FooterSpacing = 15;
                printer.PrintDataGridView(dgvHistoricos);
            }
        }

        private class Lancamento
        {
            public string ID { get; set; }
            public decimal Valor { get; set; }
            public string Data {  get; set; }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                var dialogResult = MessageBox.Show("Deseja realmente excluir o histórico selecionado? Esse processo não pode ser desfeito!", "Confirmação de exclusão do histórico", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes)
                {
                    // Verifica se uma linha foi selecionada
                    if (dgvHistoricos.SelectedRows.Count > 0)
                    {
                        // Obtem a linha selecionada
                        DataGridViewRow selectedRow = dgvHistoricos.SelectedRows[0];

                        // Obtem o id do histórico a ser excluido
                        var idExcluir = selectedRow.Cells["ID"].Value.ToString();

                        using (var transacao = con.conn.BeginTransaction())
                        {
                            try
                            {
                                // Verifica se tem algum lançamento usando esse histórico
                                var sql = "SELECT count(id) FROM lancamentos WHERE id_historico = @id_historico;";
                                using (var comando = new SqliteCommand(sql, con.conn))
                                {
                                    comando.Transaction = transacao;

                                    comando.Parameters.AddWithValue("@id_historico", idExcluir);
                                    var registros = Convert.ToInt32(comando.ExecuteScalar());

                                    if (registros > 0)
                                    {
                                        dialogResult = MessageBox.Show("Alguns lançamentos utilizam o histórico informado, deseja alterar todos eles para um histórico diferente?", "Confirmação de alteração do histórico de lançamentos", MessageBoxButtons.YesNoCancel);

                                        // Se sim, alterar o id_historico de todos os lançamentos
                                        if (dialogResult == DialogResult.Yes)
                                        {
                                            // Variável para armazenar o id recebido do formulário filho
                                            string idNovo = "";
                                            
                                            // Abrir formulário para seleção do id_histórico que irá substituir os demais (confirmar que o id não é o mesmo antes de retornar)
                                            using (var formFilho = new frmHistoricosSelecionar(con, idExcluir))
                                            {
                                                formFilho.DadosEnviados += (string id) =>
                                                {
                                                    idNovo = id;
                                                };

                                                formFilho.ShowDialog(); // Exibir o formulário filho
                                            }

                                            // Comando para atualizar os históricos
                                            comando.CommandText = "UPDATE lancamentos SET id_historico = @idNovo WHERE id_historico = @idExcluir;";
                                            comando.Parameters.Clear();
                                            comando.Parameters.AddWithValue("@idNovo", idNovo);
                                            comando.Parameters.AddWithValue("@idExcluir", idExcluir);

                                            // Executar o comando e verificar se todos os registros foram alterados corretamente
                                            var registrosAlterados = comando.ExecuteNonQuery();

                                            if (registros != registrosAlterados)
                                            {
                                                throw new CustomException("Houve um erro ao atualizar os históricos dos lançamentos");
                                            }
                                        }
                                        // Se não, considerar que deseja excluir, mas pedir a confirmação da exclusão mais uma vez
                                        else
                                        {
                                            dialogResult = MessageBox.Show("Você deseja então excluir todos os lançamentos associados a esse histórico? Esse processo é irreversível!", "Confirmação de exclusão de histórico e lançamentos", MessageBoxButtons.YesNoCancel);

                                            // Se aceitar, verificar uma última vez a decisão
                                            if (dialogResult == DialogResult.Yes)
                                            {
                                                dialogResult = MessageBox.Show("Você realmente deseja excluir todos os lançamentos associados a esse histórico? Esse processo é irreversível! (verificação dupla)", "Confirmação de exclusão de histórico e lançamentos", MessageBoxButtons.YesNoCancel);

                                                // Se a resposta for sim, excluir todos os lançamentos com o id_historico e atualizar os seus valores no caixa/saldos posteriores
                                                if (dialogResult == DialogResult.Yes)
                                                {
                                                    // Obter lista de lançamentos com o histórico informado
                                                    comando.CommandText = "SELECT id, data, valor FROM lancamentos WHERE id_historico = @id_historico;";

                                                    // Ler valores de lançamentos
                                                    List<Lancamento> listLancamentos = new List<Lancamento>();

                                                    using (var reader = comando.ExecuteReader())
                                                    {
                                                        while (reader.Read())
                                                        {
                                                            Lancamento lancamento = new Lancamento
                                                            {
                                                                ID = reader["id"].ToString(),
                                                                Valor = Convert.ToDecimal(reader["valor"]),
                                                                Data = reader["data"].ToString()
                                                            };
                                                            listLancamentos.Add(lancamento);
                                                        }
                                                    }

                                                    // Excluir cada lançamento na lista
                                                    foreach (var lancamento in listLancamentos)
                                                    {
                                                        Contabilidade.Forms.Lancamentos.frmLancamentos.excluirLancamento(con, lancamento.ID, lancamento.Data, lancamento.Valor, transacao);
                                                    }
                                                }
                                                else
                                                {
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                return;
                                            }
                                        }
                                    }

                                    // Excluir histórico
                                    comando.CommandText = "DELETE FROM historicos WHERE id_historico = @id_historico;";
                                    var result = comando.ExecuteNonQuery();

                                    if (result == 0)
                                    {
                                        throw new CustomException("Erro ao excluir o histórico selecionado");
                                    }

                                    // Efetivar alterações
                                    transacao.Commit();

                                    // Adicionar dados na tabela - Recarregar completamente
                                    atualizarDataGrid();

                                    MessageBox.Show("Histórico excluido com sucesso.", "Operação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            catch (CustomException ex)
                            {
                                transacao.Rollback();
                                MessageBox.Show($"{ex.Message?.ToString()}", "Erro ao excluir o histórico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            catch (Exception ex)
                            {
                                transacao.Rollback();
                                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao excluir o histórico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao excluir o histórico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
