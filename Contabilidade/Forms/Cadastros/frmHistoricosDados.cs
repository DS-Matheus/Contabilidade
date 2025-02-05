using System.Runtime.InteropServices;

namespace Contabilidade.Forms.Cadastros
{
    public partial class frmHistoricosDados : Form
    {
        // Funções usadas para permitir que a janela se movimente através da barra superior customizada
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private bool copiarCriar = false;

        public frmHistoricosDados(string titulo, string historico)
        {
            InitializeComponent();

            this.Text = titulo;
            this.lblTitulo.Text = titulo;

            if (!string.IsNullOrWhiteSpace(historico)) {
                txtHistorico.Text = historico;
                copiarCriar = true;
            }

            txtHistorico.Select();
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

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            var historicoNovo = txtHistorico.Text.Trim();
            // Se o histórico já existir
            if (frmHistoricos.verificarExistenciaHistorico(historicoNovo))
            {
                MessageBox.Show("O histórico informado já existe!", "Erro ao informar histórico", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                
                // Apenas excluir o texto se não estiver usando a opção copiar e criar
                if (!copiarCriar)
                {
                    txtHistorico.Text = "";
                }
                
                txtHistorico.Focus();
            }
            else
            {
                // Envia os dados para o formulário pai
                frmHistoricos.historico = historicoNovo;

                this.DialogResult = DialogResult.OK;
                this.Dispose();
            }
        }

        private void txtHistorico_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verifica se a tecla pressionada é Enter
            if (e.KeyChar == (char)Keys.Enter)
            {
                // Impede a quebra de linha
                e.Handled = true;

                btnSalvar.PerformClick();
            }
        }
    }
}
