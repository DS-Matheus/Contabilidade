using Contabilidade.Forms.Cadastros;
using Contabilidade.Models;
using System.Data;
using System.Data.SQLite;
using System.Runtime.InteropServices;

namespace Contabilidade.Forms.Lancamentos
{
    public partial class frmLancamentosDadosHistorico : Form
    {
        // Funções usadas para permitir que a janela se movimente através da barra superior customizada
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        Conexao con;
        static DataTable dtDados = new DataTable();
        DataView dv = dtDados.DefaultView;
        public frmLancamentosDadosHistorico(Conexao conexaoBanco)
        {
            InitializeComponent();

            this.con = conexaoBanco;

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

                dv.RowFilter = $"historico LIKE '{txtHistorico.Text}%'";
                dgvHistoricos.DataSource = dv;
            }
        }

        private bool verificarExistenciaHistorico(string historico)
        {
            return dtDados.AsEnumerable().Any(row => string.Equals(historico, row.Field<string>("historico"), StringComparison.OrdinalIgnoreCase));
        }

        private void btnCriar_Click(object sender, EventArgs e)
        {
            try
            {
                var historicoNovo = txtHistorico.Text.TrimEnd();
                // Se o histórico já existir
                if (verificarExistenciaHistorico(historicoNovo))
                {
                    MessageBox.Show("O histórico informado já existe!", "Erro ao informar histórico", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtHistorico.Text = "";
                    txtHistorico.Focus();
                }
                else
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

                                comando.Parameters.AddWithValue("@historico", historicoNovo);

                                int retornoBD = comando.ExecuteNonQuery();

                                // Verificar se houve a criação da linha (0 = negativo)
                                if (retornoBD > 0)
                                {
                                    comando.CommandText = "SELECT last_insert_rowid();";
                                    var id = (Int64)comando.ExecuteScalar();

                                    // Adicionar dados na tabela
                                    DataRow row = dtDados.NewRow();
                                    row["id"] = id;
                                    row["historico"] = historicoNovo;
                                    dtDados.Rows.Add(row);

                                    dgvHistoricos.Refresh();

                                    txtHistorico.Text = txtHistorico.Text.Trim();

                                    MessageBox.Show("Histórico criado com sucesso!", "Criação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);

                                    // Salvar registro commitando sua alteração
                                    transacao.Commit();

                                    frmHistoricos.novoHistorico = true;
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
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao criar o histórico", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void dgvHistoricos_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvHistoricos.Rows[e.RowIndex];

                frmLancamentosDados.id_historico = row.Cells["ID"].Value.ToString();
                frmLancamentosDados.historico = row.Cells["Histórico"].Value.ToString();

                this.DialogResult = DialogResult.OK;
                this.Dispose();
            }
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void pnlBarraTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void lblTitulo_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }

        private void txtFiltrar_TextChanged(object sender, EventArgs e)
        {
            dv.RowFilter = $"historico LIKE '%{txtFiltrar.Text}%'";
            dgvHistoricos.DataSource = dv;
        }

        private void dgvHistoricos_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvHistoricos.Rows[e.RowIndex];

                txtHistorico.Text = row.Cells["Histórico"].Value.ToString();
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

        private void txtHistorico_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verifica se a tecla pressionada é Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Impede a quebra de linha
                e.Handled = true;

                btnCriar.PerformClick();
            }
        }
    }
}
