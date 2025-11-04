using System.Runtime.InteropServices;

namespace Contabilidade.Forms.Lancamentos
{
    public partial class frmCalcularObterDados : Form
    {
        // Funções usadas para permitir que a janela se movimente através da barra superior customizada
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        public string dataInicial { get; private set; }
        public string dataInicialFormatada { get; private set; }
        public string dataFinal { get; private set; }
        public string dataFinalFormatada { get; private set; }
        public frmCalcularObterDados()
        {
            InitializeComponent();

            dtpDataInicial.Select();
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

        private void btnConfirmar_Click(object sender, EventArgs e)
        {
            try
            {
                // Obter datas
                (dataInicial, dataInicialFormatada, dataFinal, dataFinalFormatada) = Contabilidade.Forms.Relatorios.frmRazaoAnalitico.ordenarDatasEObterStringsFormatadas(dtpDataInicial.Value, dtpDataFinal.Value);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao calcular valores", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
