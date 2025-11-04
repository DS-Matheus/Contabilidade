using System.Runtime.InteropServices;

namespace Contabilidade.Forms.Lancamentos
{
    public partial class frmCalcularExibir : Form
    {
        // Funções usadas para permitir que a janela se movimente através da barra superior customizada
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        public frmCalcularExibir(string periodo, decimal creditos, decimal debitos, decimal saldoAnterior, decimal saldoFinal, decimal diferenca)
        {
            InitializeComponent();

            txtPeriodo.Text = periodo;
            txtCreditos.Text = creditos.ToString("#,##0.00");
            txtDebitos.Text = debitos.ToString("#,##0.00");
            txtSaldoAnterior.Text = saldoAnterior.ToString("#,##0.00");
            txtSaldoFinal.Text = saldoFinal.ToString("#,##0.00");
            txtDiferenca.Text = diferenca.ToString("#,##0.00");
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

        private void btnFechar2_Click(object sender, EventArgs e)
        {
            btnFechar.PerformClick();
        }
    }
}
