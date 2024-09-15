using Contabilidade.Models;
using DGVPrinterHelper;
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
            using (var command = new SQLiteCommand(sql, con.conn))
            {
                SQLiteDataAdapter sqlDA = new SQLiteDataAdapter(sql, con.conn);
                dtDados.Clear();
                sqlDA.Fill(dtDados);

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
                    using (var comando = new SQLiteCommand(sql, con.conn))
                    {
                        comando.Parameters.AddWithValue("@historico", historico);

                        int retornoBD = comando.ExecuteNonQuery();

                        // Verificar se houve a criação da linha (0 = negativo)
                        if (retornoBD > 0)
                        {
                            var id = comando.Connection.LastInsertRowId;

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
                        else
                        {
                            MessageBox.Show("Não foi possível criar o novo histórico.", "Histórico não criado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                    using (var comando = new SQLiteCommand("UPDATE historicos SET historico = @historico WHERE id = @id;", con.conn))
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
                            MessageBox.Show("Não foi possível encontrar o histórico ou ocorreu um erro na edição.", "Exclusão não realizada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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

        private void btnExcluir_Click(object sender, EventArgs e)
        {

        }
    }
}
