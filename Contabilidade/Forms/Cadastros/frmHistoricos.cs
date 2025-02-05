using Contabilidade.Models;
using System.Data;
using System.Data.SQLite;
using Contabilidade.Classes;
using Contabilidade.Forms.Relatorios;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Diagnostics;

namespace Contabilidade.Forms.Cadastros
{
    public partial class frmHistoricos : Form
    {
        Conexao con;
        static DataTable dtDados = new DataTable();
        DataView dv = dtDados.DefaultView;
        public static string historico { get; set; } = "";
        public static bool novoHistorico { get; set; } = false;

        public frmHistoricos(Conexao conexaoBanco)
        {
            InitializeComponent();

            con = conexaoBanco;

            atualizarDataGrid();

            dgvHistoricos.Columns["ID"].Visible = false;

            txtFiltrar.Select();
        }

        private void atualizarDataGrid()
        {
            // Query de pesquisa
            string sql = "SELECT * FROM historicos;";
            using (var command = new SQLiteCommand(sql, con.conn))
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
            return dtDados.AsEnumerable().Any(row => string.Equals(historico, row.Field<string>("historico"), StringComparison.OrdinalIgnoreCase));

        }

        private void btnCriar_Click(object sender, EventArgs e)
        {
            try
            {
                // Criar uma instância do formulário de dados e aguardar um retorno
                using (var frmDados = new frmHistoricosDados("Criar histórico", ""))
                {
                    // O usuário apertou o botão de salvar
                    if (frmDados.ShowDialog() == DialogResult.OK)
                    {
                        using (var transacao = con.conn.BeginTransaction())
                        {
                            try
                            {
                                // Criar histórico
                                string sql = "INSERT INTO historicos (historico) VALUES(@historico);";
                                using (var comando = new SQLiteCommand(sql, con.conn))
                                {
                                    comando.Transaction = transacao;
                                    comando.Parameters.AddWithValue("@historico", historico);

                                    int retornoBD = comando.ExecuteNonQuery();

                                    // Verificar se houve a criação da linha (0 = negativo)
                                    if (retornoBD > 0)
                                    {
                                        using (var command = new SQLiteCommand("SELECT last_insert_rowid();", con.conn))
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

                                            // Efetivar alterações
                                            transacao.Commit();

                                            MessageBox.Show("Histórico criado com sucesso!", "Criação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                        }
                                    }
                                    else
                                    {
                                        throw new Exception("Não foi possível criar o novo histórico.");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                transacao.Rollback();
                                MessageBox.Show(ex.Message.ToString(), "Histórico não criado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao criar o histórico", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            try
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
                        using (var transacao = con.conn.BeginTransaction())
                        {
                            try
                            {
                                // Editar histórico
                                using (var comando = new SQLiteCommand("UPDATE historicos SET historico = @historico WHERE id = @id;", con.conn))
                                {
                                    comando.Transaction = transacao;
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

                                        // Efetivar alterações
                                        transacao.Commit();

                                        MessageBox.Show("Histórico editado com sucesso!", "Edição bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        throw new Exception("Não foi possível encontrar o histórico ou ocorreu um erro na edição.");
                                    }
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
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao editar o histórico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                // Exibir caixa de diálogo para o usuário definir o cabeçalho
                string titulo, subtitulo = "";

                using (var formDados = new frmDefinirCabecalho($"Históricos cadastrados em {DateTime.Today.ToString("dd/MM/yyyy")}"))
                {
                    var resultado = formDados.ShowDialog();

                    // Se clicar em salvar
                    if (resultado == DialogResult.OK)
                    {
                        (titulo, subtitulo) = (formDados.titulo, formDados.subtitulo);
                    }
                    // Se cancelar ou fechar de alguma forma
                    else
                    {
                        return;
                    }
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

                            // Função local para adicionar o cabeçalho
                            void adicionarCabecalho(string titulo, string subtitulo)
                            {
                                // Adicionando parágrafos ao documento
                                pdf.Add(new Paragraph($"{titulo?.PadRight(98)} PÁGINA: {writer.PageNumber.ToString("D3")}", fonte));
                                if (!string.IsNullOrWhiteSpace(subtitulo))
                                {
                                    pdf.Add(new Paragraph($"{subtitulo}", fonte));
                                    linhasDisponiveis--;
                                }
                                pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                pdf.Add(new Paragraph("NÚMERO - HISTÓRICO                                                                                            ", fonte));
                                pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                pdf.Add(new Paragraph("    ", fonte));

                                // Contar linhas usadas após adição do cabeçalho
                                linhasDisponiveis -= 5;
                            };

                            // Operações com o subtítulo
                            if (!string.IsNullOrWhiteSpace(subtitulo))
                            {
                                subtitulo = frmSaldo.CentralizarString(subtitulo, 110);
                            }

                            // Adicionar cabeçalho da primeira página
                            adicionarCabecalho(titulo, subtitulo);

                            var qtdLinhas = dgvHistoricos.Rows.Count;

                            for (int i = 0; i < qtdLinhas; i++)
                            {
                                // Obter dados
                                var id = dgvHistoricos.Rows[i].Cells["ID"].Value?.ToString();
                                var historico = dgvHistoricos.Rows[i].Cells["Histórico"].Value?.ToString();

                                // Verificar quantas linhas serão usadas
                                var linhasHistorico = Contabilidade.Forms.Relatorios.frmSaldo.QuebrarLinhaString(historico, 100);

                                // Verificar tamanho do histórico
                                var linhasNecessarias = historico.Length >= 100 ? linhasHistorico.Count : 1;

                                if (linhasNecessarias >= 2)
                                {
                                    // Testar se possuí uma linha disponível para usar
                                    if (linhasDisponiveis - linhasNecessarias < 0)
                                    {
                                        pdf.NewPage();
                                        linhasDisponiveis = 57;
                                        adicionarCabecalho(titulo, subtitulo);
                                    }

                                    // Adicionar primeira linha
                                    pdf.Add(new Paragraph($"{id} - {linhasHistorico[0]}", fonte));

                                    linhasHistorico.RemoveAt(0);
                                    linhasDisponiveis--;

                                    // Adicionar demais linhas
                                    foreach (var linhaHistorico in linhasHistorico)
                                    {
                                        pdf.Add(new Paragraph($"{" ".PadRight(id.ToString().Length + 3)}{linhaHistorico}", fonte));
                                        linhasDisponiveis--;
                                    }
                                }
                                else
                                {
                                    // Testar se possuí uma linha disponível para usar
                                    if (linhasDisponiveis - 1 < 0)
                                    {
                                        pdf.NewPage();
                                        linhasDisponiveis = 57;
                                        adicionarCabecalho(titulo, subtitulo);
                                    }

                                    // Caso seja apenas 1 linha
                                    pdf.Add(new Paragraph($"{id} - {historico}", fonte));
                                    linhasDisponiveis--;
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
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao gerar o relatório", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class Lancamento
        {
            public string ID { get; set; }
            public string Data { get; set; }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                var dialogResult = MessageBox.Show("Deseja realmente excluir o histórico selecionado? Esse processo não pode ser desfeito!", "Confirmação de exclusão do histórico", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
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
                                using (var comando = new SQLiteCommand(sql, con.conn))
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
                                            using (var formFilho = new frmHistoricosSelecionar(con, transacao, idExcluir))
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
                                        else if (dialogResult == DialogResult.No)
                                        {
                                            dialogResult = MessageBox.Show("Você deseja então excluir todos os lançamentos associados a esse histórico? Esse processo é irreversível!", "Confirmação de exclusão de histórico e lançamentos", MessageBoxButtons.YesNo);

                                            // Se aceitar, verificar uma última vez a decisão
                                            if (dialogResult == DialogResult.Yes)
                                            {
                                                dialogResult = MessageBox.Show("Você realmente deseja excluir todos os lançamentos associados a esse histórico? Esse processo é irreversível! (verificação dupla)", "Confirmação de exclusão de histórico e lançamentos", MessageBoxButtons.YesNo);

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
                                                                Data = reader["data"].ToString()
                                                            };
                                                            listLancamentos.Add(lancamento);
                                                        }
                                                    }

                                                    // Excluir cada lançamento na lista
                                                    foreach (var lancamento in listLancamentos)
                                                    {
                                                        Contabilidade.Forms.Lancamentos.frmLancamentos.excluirLancamento(con, lancamento.ID, lancamento.Data, transacao);
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
                                        else
                                        {
                                            return;
                                        }
                                    }

                                    // Excluir histórico
                                    comando.Parameters.Clear();
                                    comando.CommandText = "DELETE FROM historicos WHERE id = @id_historico;";
                                    comando.Parameters.AddWithValue("@id_historico", idExcluir);
                                    var result = comando.ExecuteNonQuery();

                                    if (result == 0)
                                    {
                                        throw new CustomException("Erro ao excluir o histórico selecionado");
                                    }

                                    // Efetivar alterações
                                    transacao.Commit();

                                    // Remover dados da tabela - Recarregar completamente
                                    atualizarDataGrid();

                                    MessageBox.Show("Histórico excluido com sucesso.", "Operação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                }
                            }
                            catch (CustomException ex)
                            {
                                transacao.Rollback();

                                // Atualizar DataGrid, apenas para os casos em que um novo histórico foi criado e deu algum erro após
                                if (novoHistorico)
                                {
                                    novoHistorico = false;
                                    atualizarDataGrid();
                                }

                                MessageBox.Show($"{ex.Message?.ToString()}", "Erro ao excluir o histórico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                            catch (Exception ex)
                            {
                                transacao.Rollback();

                                // Atualizar DataGrid, apenas para os casos em que um novo histórico foi criado e deu algum erro após
                                if (novoHistorico)
                                {
                                    novoHistorico = false;
                                    atualizarDataGrid();
                                }

                                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao excluir o histórico", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Atualizar DataGrid, apenas para os casos em que um novo histórico foi criado e deu algum erro após
                if (novoHistorico)
                {
                    novoHistorico = false;
                    atualizarDataGrid();
                }

                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao excluir o histórico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txtFiltrar_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verifica se a tecla pressionada é Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Impede a quebra de linha
                e.Handled = true;
            }
        }

        private void btnCopiarCriar_Click(object sender, EventArgs e)
        {
            try
            {
                // Verifique se há uma linha selecionada
                if (dgvHistoricos.SelectedRows.Count > 0)
                {
                    // Obtém o histórico selecionado
                    string historico = dgvHistoricos.SelectedRows[0].Cells["Histórico"].Value.ToString();

                    // Criar uma instância do formulário de dados e aguardar um retorno
                    using (var frmDados = new frmHistoricosDados("Criar histórico", historico))
                    {
                        // O usuário apertou o botão de salvar
                        if (frmDados.ShowDialog() == DialogResult.OK)
                        {
                            using (var transacao = con.conn.BeginTransaction())
                            {
                                try
                                {
                                    // Criar histórico
                                    string sql = "INSERT INTO historicos (historico) VALUES(@historico);";
                                    using (var comando = new SQLiteCommand(sql, con.conn))
                                    {
                                        comando.Transaction = transacao;
                                        comando.Parameters.AddWithValue("@historico", historico);

                                        int retornoBD = comando.ExecuteNonQuery();

                                        // Verificar se houve a criação da linha (0 = negativo)
                                        if (retornoBD > 0)
                                        {
                                            using (var command = new SQLiteCommand("SELECT last_insert_rowid();", con.conn))
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

                                                // Efetivar alterações
                                                transacao.Commit();

                                                MessageBox.Show("Histórico criado com sucesso!", "Criação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            }
                                        }
                                        else
                                        {
                                            throw new Exception("Não foi possível criar o novo histórico.");
                                        }
                                    }
                                }
                                catch (Exception ex)
                                {
                                    transacao.Rollback();
                                    MessageBox.Show(ex.Message.ToString(), "Histórico não criado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                }
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Nenhum histórico foi selecionado na tabela de dados", "Erro ao copiar histórico", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao copiar o histórico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
