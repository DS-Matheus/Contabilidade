using Contabilidade.Models;
using DGVPrinterHelper;
using System.Data;
using Microsoft.Data.Sqlite;

namespace Contabilidade.Forms.Cadastros
{
    public partial class frmUsuarios : Form
    {
        string usuarioAtual;
        Conexao con;
        static DataTable dtDados = new DataTable();
        DataView dv = dtDados.DefaultView;
        public static string usuario { get; set; } = "";
        public static string senha { get; set; } = "";

        public frmUsuarios(string usuarioBD, Conexao conexaoBanco)
        {
            InitializeComponent();

            con = conexaoBanco;
            usuarioAtual = usuarioBD;

            atualizarDataGrid();
        }

        public void atualizarDataGrid()
        {
            // Query de pesquisa
            string sql = "SELECT * FROM usuarios;";
            using (var command = new SqliteCommand(sql, con.conn))
            {
                dtDados.Clear();
                using (var reader = command.ExecuteReader())
                {
                    dtDados.Load(reader);
                }

                dgvUsuarios.DataSource = dtDados;

                dv.RowFilter = $"nome LIKE '{txtFiltrar.Text}%'";
                dgvUsuarios.DataSource = dv;

                txtFiltrar.MaxLength = 20;
                cbbFiltrar.SelectedIndex = 0;
            }
        }

        public static bool verificarExistenciaUsuario(string usuario)
        {
            return dtDados.AsEnumerable().Any(row => usuario == row.Field<string>("nome"));
        }

        private void btnCriar_Click(object sender, EventArgs e)
        {
            // Criar uma instância do formulário de dados e aguardar um retorno
            using (var frmDados = new frmUsuariosDados("Criar usuário", "", ""))
            {
                // O usuário apertou o botão de salvar
                if (frmDados.ShowDialog() == DialogResult.OK)
                {
                    // Criar usuário
                    string sql = "INSERT INTO usuarios (nome, senha) VALUES(@nome, @senha);";
                    using (var comando = new SqliteCommand(sql, con.conn))
                    {
                        comando.Parameters.AddWithValue("@nome", usuario);
                        comando.Parameters.AddWithValue("@senha", senha);

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
                                row["nome"] = usuario;
                                row["senha"] = senha;
                                dtDados.Rows.Add(row);

                                dgvUsuarios.Refresh();

                                // Remover dados das variáveis
                                usuario = "";
                                senha = "";

                                MessageBox.Show("Usuário criado com sucesso!", "Criação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Não foi possível criar o novo usuário.", "Usuário não criado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        private void excluirUsuario(string id)
        {
            using (var comando = new SqliteCommand("DELETE FROM usuarios WHERE id = @id", con.conn))
            {
                comando.Parameters.AddWithValue("@id", id);

                int retornoBD = comando.ExecuteNonQuery();

                // Verificar se houve a exclusão de alguma linha (0 = negativo)
                if (retornoBD > 0)
                {
                    // Encontrar registro no DataTable
                    DataRow[] rows = dtDados.Select($"ID = {id}");
                    // Excluir do DataTable
                    if (rows.Length > 0)
                    {
                        // Encontrou o usuário, podemos excluí-lo
                        rows[0].Delete();
                        dtDados.AcceptChanges();
                        MessageBox.Show("Usuário excluído com sucesso!", "Exclusão bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        MessageBox.Show("DataGridView não atualizado, comunique o desenvolvedor!", "Exclusão com erros", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    dgvUsuarios.Refresh();
                }
                else
                {
                    MessageBox.Show("Não foi possível encontrar o usuário ou ocorreu um erro na exclusão.", "Exclusão não realizada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            int numLinha = obterNumLinhaSelecionada(dgvUsuarios);
            string id = dgvUsuarios.Rows[numLinha].Cells["ID"].Value.ToString();
            string usuario = dgvUsuarios.Rows[numLinha].Cells["Usuário"].Value?.ToString();

            DialogResult input = MessageBox.Show($"Deseja realmente excluir o usuário {usuario}?", "Confirmação de exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (input == DialogResult.Yes)
            {
                if (!verificarUsuarioAtual(usuario))
                {
                    excluirUsuario(id);
                }
                else
                {
                    if (dtDados.Rows.Count <= 1)
                    {
                        MessageBox.Show("Não é permitido excluir o único usuário do banco de dados!", "Exclusão abortada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        input = MessageBox.Show($"Você está prestes a excluir o usuário atual! Se confirmar você retornará para a tela de login, deseja continuar?", "Confirmação de exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                        if (input == DialogResult.Yes)
                        {
                            excluirUsuario(id);
                            this.Owner.Dispose();
                            this.Dispose();
                        }
                    }
                }
            }
        }

        public static bool verificarUsuarioAtual(string usuario)
        {
            if (frmPainelPrincipal.usuarioAtual == usuario)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Obter usuário selecionado
            int numLinha = obterNumLinhaSelecionada(dgvUsuarios);
            var id = dgvUsuarios.Rows[numLinha].Cells["ID"].Value;
            var (usuarioAntigo, senhaAntiga) = obterDadosDGV(numLinha);

            // Criar uma instância do formulário de dados e aguardar um retorno
            using (var frmDados = new frmUsuariosDados("Editar usuário", usuarioAntigo, senhaAntiga))
            {
                // O usuário apertou o botão de salvar
                if (frmDados.ShowDialog() == DialogResult.OK)
                {
                    // Editar usuário
                    using (var comando = new SqliteCommand("UPDATE usuarios SET nome = @nome, senha = @senha WHERE id = @id", con.conn))
                    {
                        comando.Parameters.AddWithValue("@nome", usuario);
                        comando.Parameters.AddWithValue("@senha", senha);
                        comando.Parameters.AddWithValue("@id", id);

                        int retornoBD = comando.ExecuteNonQuery();

                        // Verificar se houve a edição de alguma linha (0 = negativo)
                        if (retornoBD > 0)
                        {
                            // Caso o usuário atual foi editado, atualizar no painel principal
                            if (verificarUsuarioAtual(usuarioAntigo))
                            {
                                frmPainelPrincipal.usuarioAtual = usuario;
                            }

                            // Atualizar DataTable
                            dgvUsuarios.Rows[numLinha].Cells["Usuário"].Value = usuario;
                            dgvUsuarios.Rows[numLinha].Cells["Senha"].Value = senha;

                            dgvUsuarios.Refresh();

                            // Remover dados das variáveis
                            usuario = "";
                            senha = "";

                            MessageBox.Show("Usuário editado com sucesso!", "Edição bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else
                        {
                            MessageBox.Show("Não foi possível encontrar o usuário ou ocorreu um erro na edição.", "Exclusão não realizada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        public static int obterNumLinhaSelecionada(DataGridView dataGridView)
        {
            return dataGridView.CurrentRow.Index;
        }

        private (string usuario, string senha) obterDadosDGV(int numLinha)
        {
            string usuario = dgvUsuarios.Rows[numLinha].Cells["Usuário"].Value?.ToString();
            string senha = dgvUsuarios.Rows[numLinha].Cells["Senha"].Value?.ToString();

            return (usuario, senha);
        }

        private void txtFiltrar_TextChanged(object sender, EventArgs e)
        {
            // Filtrar por usuário
            if (cbbFiltrar.SelectedIndex == 0)
            {
                txtFiltrar.MaxLength = 20;
                dv.RowFilter = $"nome LIKE '%{txtFiltrar.Text}%'";
                dgvUsuarios.DataSource = dv;
            }
            // Filtrar por senha
            else
            {
                txtFiltrar.MaxLength = 30;
                dv.RowFilter = $"senha LIKE '%{txtFiltrar.Text}%'";
                dgvUsuarios.DataSource = dv;
            }
        }

        private void cbbFiltrar_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFiltrar.Text = "";
            txtFiltrar_TextChanged(sender, e);
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            // Solicita o título do arquivo ao usuário
            string inputTitle = Microsoft.VisualBasic.Interaction.InputBox("Digite o título do arquivo:", "Título do Arquivo", "");

            // Verifica se o usuário clicou em "Cancelar": se clicou não executa
            if (!string.IsNullOrEmpty(inputTitle))
            {
                // Verifica se o título está vazio ou contém apenas espaços
                string title = string.IsNullOrWhiteSpace(inputTitle) ? "Usuários Cadastrados" : inputTitle;

                var printer = new DGVPrinter();
                printer.Title = title;
                printer.SubTitle = string.Format("Data: {0}", System.DateTime.Now.ToString("dd/MM/yyyy"));
                printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
                printer.PageNumbers = true;
                printer.PageNumberInHeader = false;
                printer.PorportionalColumns = true;
                printer.HeaderCellAlignment = StringAlignment.Near;
                printer.FooterSpacing = 15;
                printer.PrintDataGridView(dgvUsuarios);
            }
        }
    }
}
