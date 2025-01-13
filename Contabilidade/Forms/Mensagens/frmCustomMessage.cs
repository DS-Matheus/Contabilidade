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

namespace Contabilidade.Forms.Mensagens
{
    public partial class frmCustomMessage : Form
    {
        // Funções usadas para permitir que a janela se movimente através da barra superior customizada
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private int numBotoes = 0;

        public frmCustomMessage(string titulo, string mensagem, string imagem, int numBotoes, int botaoPadrao = 1)
        {
            InitializeComponent();

            lblTitulo.Text = titulo;
            lblMensagem.Text = mensagem;

            if (imagem == "erro") {
                imgErro.Visible = true;
            }
            else if (imagem == "sucesso")
            {
                imgSucesso.Visible = true;
            }
            else if (imagem == "aviso")
            {
                imgAviso.Visible = true;
            }
            else
            {
                imgInfo.Visible = true;
            }

            this.numBotoes = numBotoes;

            if (numBotoes == 3)
            {
                button1.Visible = true;
                button2.Visible = true;
                button3.Visible = true;
            }
            else if (numBotoes == 2)
            {
                button1.Location = new Point(197, 4);
                button2.Location = new Point(278, 4);

                button1.Visible = true;
                button2.Visible = true;
            }
            else if (numBotoes == 1)
            {
                button1.Text = "OK";
                button1.Location = new Point(238, 4);
                button1.Visible = true;
            }

            if (botaoPadrao == 1)
            {
                button1.Focus();
            }
            else if (botaoPadrao == 2)
            {
                button2.Focus();
            }
            else if (botaoPadrao == 3)
            {
                button3.Focus();
            }
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

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (numBotoes == 1)
            {
                this.DialogResult= DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Yes;
            }

            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Dispose();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Dispose();
        }
    }
}
