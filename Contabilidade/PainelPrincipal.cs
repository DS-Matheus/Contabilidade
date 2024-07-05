using Contabilidade.Models;
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

namespace Contabilidade
{
    public partial class frmPainelPrincipal : Form
    {
        private Button botaoAtual;
        private Random aleatorio;
        private int indexTemporario;
        private Form formularioAtivo;

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        public frmPainelPrincipal(string nomeBD, string usuario)
        {
            InitializeComponent();
            lblUsuario.Text = usuario;
            lblBanco.Text = nomeBD;
            aleatorio = new Random();
            btnFecharFormFilho.Visible = false;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private Color selecionarCorTema()
        {
            int index = aleatorio.Next(TemaCores.listaCores.Count);
            // Se a cor já foi selecionada, muda para outra
            while (indexTemporario == index)
            {
                index = aleatorio.Next(TemaCores.listaCores.Count);
            }
            indexTemporario = index;
            string cor = TemaCores.listaCores[index];
            return ColorTranslator.FromHtml(cor);
        }

        private void selecionarBotao(object btnSender)
        {
            if (btnSender != null)
            {
                if (botaoAtual != (Button)btnSender)
                {
                    desselecionarBotao();

                    Color cor = selecionarCorTema();
                    botaoAtual = (Button)btnSender;
                    botaoAtual.BackColor = cor;
                    botaoAtual.ForeColor = Color.White;
                    botaoAtual.Font = new Font("Lucida Sans", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);

                    pnlTitulo.BackColor = cor;
                    pnlInfo.BackColor = TemaCores.alterarBrilhoCor(cor, -0.3);

                    TemaCores.corPrimaria = cor;
                    TemaCores.corSecundaria = TemaCores.alterarBrilhoCor(cor, -0.3);
                    btnFecharFormFilho.Visible = true;
                }
            }
        }

        private void desselecionarBotao()
        {
            foreach (Control botaoAnterior in pnlMenuLateral.Controls)
            {
                if (botaoAnterior.GetType() == typeof(Button))
                {
                    botaoAnterior.BackColor = Color.FromArgb(51, 51, 76);
                    botaoAnterior.ForeColor = Color.Gainsboro;
                    botaoAnterior.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
                }
            }
        }

        private void abrirFormulario(Form formularioFilho, object btnSender)
        {
            if (formularioAtivo != null)
            {
                formularioAtivo.Close();
            }
            selecionarBotao(btnSender);
            formularioAtivo = formularioFilho;
            formularioFilho.TopLevel = false;
            formularioFilho.FormBorderStyle = FormBorderStyle.None;
            formularioFilho.Dock = DockStyle.Fill;
            this.pnlDesktop.Controls.Add(formularioFilho);
            this.pnlDesktop.Tag = formularioFilho;
            formularioFilho.BringToFront();
            formularioFilho.Show();
            lblTitulo.Text = formularioFilho.Text;
        }

        private void btnCadastro_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.frmCadastros(), sender);
        }

        private void btnLancamentos_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.frmLancamentos(), sender);
        }

        private void btnRelatorios_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.frmRelatorios(), sender);
        }

        private void btnConsultas_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.frmConsultas(), sender);
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.frmUsuarios(), sender);
        }

        private void btnLogoff_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.frmLogoff(), sender);
        }

        private void btnFecharFormFilho_Click(object sender, EventArgs e)
        {
            if (formularioAtivo != null)
            {
                formularioAtivo.Close();
                Reset();
            }
        }

        private void Reset()
        {
            desselecionarBotao();
            lblTitulo.Text = "Painel Principal";
            pnlTitulo.BackColor = Color.FromArgb(0, 150, 136);
            pnlInfo.BackColor = Color.FromArgb(39, 39, 58);
            botaoAtual = null;
            btnFecharFormFilho.Visible = false;
        }

        private void pnlTitulo_MouseDown(object sender, MouseEventArgs e)
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
