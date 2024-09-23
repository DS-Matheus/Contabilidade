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
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Contabilidade.Forms.Relatorios
{
    public partial class frmRazaoAnalitico : Form
    {
        Conexao con;
        static DataTable dtDados = new DataTable();
        DataView dv = dtDados.DefaultView;
        public frmRazaoAnalitico(Conexao conexaoBanco)
        {
            InitializeComponent();

            con = conexaoBanco;

            atualizarDataGrid();
        }

        public void atualizarDataGrid()
        {
            // Query de pesquisa
            string sql = "SELECT * FROM contas ORDER BY conta;";
            using (var command = new SQLiteCommand(sql, con.conn))
            {
                SQLiteDataAdapter sqlDA = new SQLiteDataAdapter(sql, con.conn);
                dtDados.Clear();
                sqlDA.Fill(dtDados);

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
                txtFiltrar2.Visible = false;
            }
            // Filtrar por Saldo entre
            else if (cbbFiltrar.SelectedIndex == 5)
            {
                cbbNivel.Visible = false;
                txtFiltrar.Visible = true;
                txtFiltrar2.Visible = true;
                txtFiltrar.Width = 115;
            }
            else
            {
                cbbNivel.Visible = false;
                txtFiltrar.Visible = true;
                txtFiltrar2.Visible = false;
                txtFiltrar.Width = 238;
            }

            txtFiltrar_TextChanged(sender, e);
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
            // Saldo menor que
            else if (cbbFiltrar.SelectedIndex == 3)
            {
                if (IsNumeric(txtFiltrar.Text))
                {
                    dv.RowFilter = $"saldo <= {txtFiltrar.Text}";
                    dgvContas.DataSource = dv;
                }
            }
            // Saldo maior que
            else if (cbbFiltrar.SelectedIndex == 4)
            {
                if (IsNumeric(txtFiltrar.Text))
                {
                    dv.RowFilter = $"saldo >= {txtFiltrar.Text}";
                    dgvContas.DataSource = dv;
                }
            }
            // Saldo entre
            else if (cbbFiltrar.SelectedIndex == 5)
            {
                if (IsNumeric(txtFiltrar.Text) && IsNumeric(txtFiltrar2.Text))
                {
                    var valores = ObterMenorEMaior(int.Parse(txtFiltrar.Text), int.Parse(txtFiltrar2.Text));
                    dv.RowFilter = $"saldo BETWEEN {valores.Item1} AND {valores.Item2}";
                    dgvContas.DataSource = dv;
                }
            }
        }

        private void txtFiltrar2_TextChanged(object sender, EventArgs e)
        {
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

        private bool IsNumeric(string text)
        {
            return int.TryParse(text, out _);
        }

        private void dgvContas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvContas.Rows[e.RowIndex];
                txtConta.Text = row.Cells["Conta"].Value.ToString();
            }
        }

        private class Lancamento
        {
            public DateTime data {  get; set; }
            public string historico { get; set; }
            public decimal valor { get; set; }
            public decimal saldo {  get; set; }
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

        public static (string, string, string, string) ordenarDatasEObterTodasStrings(DateTime data1, DateTime data2)
        {
            // Se a data1 for menor ou igual a data2
            if (data1 <= data2)
            {
                return (data1.ToString("yyyy-MM-dd"), data1.ToString("dd/MM/yyyy"), data2.ToString("yyyy-MM-dd"), data2.ToString("dd/MM/yyyy"));
            }
            // Se a data2 for menor que a data1
            else
            {
                return (data2.ToString("yyyy-MM-dd"), data2.ToString("dd/MM/yyyy"), data1.ToString("yyyy-MM-dd"), data1.ToString("dd/MM/yyyy"));
            }
        }

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            // Verificar se a conta foi preenchida
            if (string.IsNullOrWhiteSpace(txtConta.Text))
            {
                MessageBox.Show("Selecione uma conta antes de gerar o relatório, dê um duplo clique na linha desejada.", "Uma conta não foi selecionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Obter datas
                var (dataInicial, dataInicialFormatada, dataFinal, dataFinalFormatada) = ordenarDatasEObterTodasStrings(dtpInicial.Value, dtpFinal.Value);

                // Consulta de lançamentos para a conta no período informado
                var sql = "SELECT l.data, h.historico, l.valor, l.saldo_atualizado FROM lancamentos l JOIN historicos h ON l.id_historico = h.id WHERE l.conta = @conta AND l.data BETWEEN @dataInicial AND @dataFinal ORDER BY l.data ASC, l.id ASC;";
                using (var comando = new SQLiteCommand(sql, con.conn))
                {
                    var conta = txtConta.Text;
                    comando.Parameters.AddWithValue("@conta", conta);
                    comando.Parameters.AddWithValue("@dataInicial", dataInicial);
                    comando.Parameters.AddWithValue("@dataFinal", dataFinal);

                    // Verificar se existe pelo menos 1 registro
                    using (SQLiteDataReader reader = comando.ExecuteReader())
                    {
                        List<Lancamento> listLancamentos = new List<Lancamento>();

                        while (reader.Read())
                        {
                            Lancamento lancamento = new Lancamento
                            {
                                data = Convert.ToDateTime(reader["data"]),
                                historico = reader["historico"].ToString(),
                                valor = Convert.ToDecimal(reader["valor"]),
                                 saldo = Convert.ToDecimal(reader["saldo"])
                            };
                            listLancamentos.Add(lancamento);
                        }

                        // Verificar se pelo menos 1 registro foi encontrado
                        if (listLancamentos.Count > 0)
                        {
                            // Obter a descrição da conta
                            comando.CommandText = "SELECT descricao FROM contas WHERE conta = @conta;";
                            comando.Parameters.AddWithValue("@conta", conta);
                            var descricao = comando.ExecuteScalar()?.ToString();

                            // Obter saldo anterior (ou deixar como 0 se não possuir nenhum registro no período ou antes do período) - CONFERIR SE ESTA CORRETO !!!!!!!!!!!!!!!
                            comando.CommandText = "SELECT COALESCE((SELECT saldo_anterior FROM lancamentos WHERE conta = @conta AND data BETWEEN @dataInicial AND @dataFinal ORDER BY data ASC, id ASC LIMIT 1), (SELECT saldo_anterior FROM lancamentos WHERE conta = @conta AND data < @dataInicial ORDER BY data DESC, id DESC LIMIT 1), 0) AS saldo_anterior;";
                            var saldoAnterior = comando.ExecuteScalar();
                        }
                        else
                        {
                            MessageBox.Show($"Nenhum lançamento foi realizado entre as datas {dataInicialFormatada} e {dataFinalFormatada}", "O relatório não foi gerado", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }                    
                }
            }
        }
    }
}
