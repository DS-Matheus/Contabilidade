﻿using Contabilidade.Models;
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

namespace Contabilidade.Forms.Relatorios
{
    public partial class frmExibirRelatorio : Form
    {
        Conexao con;
        static DataTable dtDados = new DataTable();
        private string sql = "";
        public frmExibirRelatorio(Conexao conexaoBanco, string consultaSQL)
        {
            InitializeComponent();

            con = conexaoBanco;
            sql = consultaSQL;

            atualizarDataGrid();
        }

        public void atualizarDataGrid()
        {
            using (var command = new SQLiteCommand(sql, con.conn))
            {
                SQLiteDataAdapter sqlDA = new SQLiteDataAdapter(sql, con.conn);
                dtDados.Clear();
                sqlDA.Fill(dtDados);

                dgvRelatorio.DataSource = dtDados;
            }
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            var printer = new DGVPrinter();
            printer.Title = txtTitulo.Text;
            printer.SubTitle = txtSubtitulo.Text;
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.FooterSpacing = 15;
            printer.PrintDataGridView(dgvRelatorio);
        }
    }
}