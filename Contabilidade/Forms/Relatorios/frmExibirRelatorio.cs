using Contabilidade.Models;
using DGVPrinterHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Contabilidade.Forms.Relatorios
{
    public partial class frmExibirRelatorio : Form
    {
        SQLiteCommand comando;
        static DataTable dtDados = new DataTable();
        private string sql = "";
        // Funções usadas para permitir que a janela se movimente através da barra superior customizada
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        public frmExibirRelatorio(string titulo, SQLiteCommand comandoBanco)
        {
            InitializeComponent();

            comando = comandoBanco;

            lblTitulo.Text = titulo;

            atualizarDataGrid();
        }

        public void atualizarDataGrid()
        {
            SQLiteDataAdapter sqlDA = new SQLiteDataAdapter(comando);
            dtDados.Clear();
            sqlDA.Fill(dtDados);

            dgvRelatorio.DataSource = dtDados;
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
    }
}
