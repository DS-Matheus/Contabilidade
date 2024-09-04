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

namespace Contabilidade.Forms.Relatorios
{
    public partial class frmRelAnalitico : Form
    {
        Conexao con;
        static DataTable dtDados = new DataTable();
        DataView dv = dtDados.DefaultView;
        public frmRelAnalitico(Conexao conexaoBanco)
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
    }
}
