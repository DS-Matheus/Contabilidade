using Contabilidade.Forms.Cadastros;
using Contabilidade.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Contabilidade.Forms.Lancamentos
{
    public partial class frmLancamentosDadosHistorico : Form
    {
        Conexao con;
        static DataTable dtDados = new DataTable();
        DataView dv = dtDados.DefaultView;
        public frmLancamentosDadosHistorico(Conexao conexaoBanco)
        {
            InitializeComponent();

            this.con = conexaoBanco;

            atualizarDataGrid();
        }

        public void atualizarDataGrid()
        {
            // Query de pesquisa
            string sql = "SELECT * FROM historicos;";
            using (var command = new SQLiteCommand(sql, con.conn))
            {
                SQLiteDataAdapter sqlDA = new SQLiteDataAdapter(sql, con.conn);
                dtDados.Clear();
                sqlDA.Fill(dtDados);

                dgvHistoricos.DataSource = dtDados;

                dv.RowFilter = $"historico LIKE '{txtHistorico.Text}%'";
                dgvHistoricos.DataSource = dv;
            }
        }

        private void btnCriar_Click(object sender, EventArgs e)
        {
            var historicoNovo = txtHistorico.Text.TrimEnd();
            // Se o histórico já existir
            if (frmHistoricos.verificarExistenciaHistorico(historicoNovo))
            {
                MessageBox.Show("O histórico informado já existe!", "Erro ao informar histórico", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtHistorico.Text = "";
                txtHistorico.Focus();
            }
            else
            {
                // Criar histórico
                string sql = "INSERT INTO historicos (historico) VALUES(@historico);";
                using (var comando = new SQLiteCommand(sql, con.conn))
                {
                    comando.Parameters.AddWithValue("@historico", historicoNovo);

                    int retornoBD = comando.ExecuteNonQuery();

                    // Verificar se houve a criação da linha (0 = negativo)
                    if (retornoBD > 0)
                    {
                        var id = comando.Connection.LastInsertRowId;

                        // Adicionar dados na tabela
                        DataRow row = dtDados.NewRow();
                        row["id"] = id;
                        row["historico"] = historicoNovo;
                        dtDados.Rows.Add(row);

                        dgvHistoricos.Refresh();

                        txtHistorico.Text = txtHistorico.Text.Trim();

                        MessageBox.Show("Histórico criado com sucesso!", "Criação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        MessageBox.Show("Não foi possível criar o novo histórico.", "Histórico não criado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void txtHistorico_TextChanged(object sender, EventArgs e)
        {
            dv.RowFilter = $"historico LIKE '%{txtHistorico.Text}%'";
            dgvHistoricos.DataSource = dv;
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
    }
}
