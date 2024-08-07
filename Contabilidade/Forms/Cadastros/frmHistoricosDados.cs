using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Contabilidade.Forms.Cadastros
{
    public partial class frmHistoricosDados : Form
    {
        // Funções usadas para permitir que a janela se movimente através da barra superior customizada
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        public frmHistoricosDados(string titulo, string historico)
        {
            InitializeComponent();

            this.Text = titulo;
            this.lblTitulo.Text = titulo;

            txtHistorico.Text = historico;

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
                // Envia os dados para o formulário pai
                frmHistoricos.historico = historicoNovo;

                this.DialogResult = DialogResult.OK;
                this.Dispose();
            }
        }
    }
}
