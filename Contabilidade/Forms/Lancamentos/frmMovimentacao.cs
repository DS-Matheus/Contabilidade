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
    public partial class frmMovimentacao : Form
    {
        Conexao con;
        static DataTable dtDados = new DataTable();
        DataView dv = dtDados.DefaultView;
        public static string usuario { get; set; } = "";
        public static string senha { get; set; } = "";

        public frmMovimentacao(string usuarioBD, Conexao conexaoBanco)
        {
            InitializeComponent();

            con = conexaoBanco;

            atualizarDataGrid();
        }

        public void atualizarDataGrid()
        {
            // Query de pesquisa
            string sql = "SELECT l.id, l.conta, c.descricao, l.valor, l.data, h.historico FROM lancamentos l JOIN contas c ON l.conta = c.conta JOIN historicos h ON l.id_historico = h.id ORDER BY conta;";
            using (var command = new SQLiteCommand(sql, con.conn))
            {
                SQLiteDataAdapter sqlDA = new SQLiteDataAdapter(sql, con.conn);
                dtDados.Clear();
                sqlDA.Fill(dtDados);

                dgvMovimentacao.DataSource = dtDados;

                dv.RowFilter = $"conta LIKE '{txtFiltrar.Text}%'";
                dgvMovimentacao.DataSource = dv;

                cbbFiltrar.SelectedIndex = 0;
            }
        }
    }
}
