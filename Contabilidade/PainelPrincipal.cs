using Contabilidade.Forms.Cadastros;
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
        private Form formularioAtivo;
        Conexao con;
        public static string usuarioAtual = "";

        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        public frmPainelPrincipal(string nomeBD, string usuario, Conexao conexaoBanco)
        {
            InitializeComponent();
            
            // Salvar informações da conexão
            con = conexaoBanco;
            usuarioAtual = usuario;
            lblUsuario.Text = usuarioAtual;
            lblBanco.Text = nomeBD;

            btnFecharFormFilho.Visible = false;
            this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;

            timerRelogio.Start();
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            con.desconectar();
            this.Dispose();
            Application.Exit();
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void selecionarBotao(object btnSender)
        {
            if (btnSender != null)
            {
                if (botaoAtual != (Button)btnSender)
                {
                    List<Panel> listaPaineisMenu = [
                        pnlMenuLateral,
                        pnlCadastros,
                        pnlLancamentos,
                        pnlRelatorios,
                    ];

                    List<Button> listaBotoesMenu = [
                        btnCadastro,
                        btnLancamentos,
                        btnRelatorios,
                        btnLogoff,
                    ];

                    List<Button> listaBotoesSubMenu = [
                        btnContas,
                        btnHistoricos,
                        btnUsuarios,
                        btnMovimentacao,
                        btnTransporte,
                        btnRelDiario,
                        btnRelAnalitico,
                        btnBalanceteGeral,
                        btnBalanceteConta,
                        btnSaldo,
                    ];

                    // Testar se um botão já foi pressionado anteriormente
                    if (botaoAtual != null)
                    {
                        // Botão anterior: Voltar fonte ao padrão
                        botaoAtual.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
                        botaoAtual.ForeColor = Color.Gainsboro;
                    }

                    // Atualizar botão selecionado
                    botaoAtual = (Button)btnSender;

                    // Aplicar cores do tema
                    // Botões de menu
                    foreach (Button botao in listaBotoesMenu)
                    {
                        botao.BackColor = TemaCores.CorBotaoMenu;
                    }
                    // Botões dropdown
                    foreach (Button botao in listaBotoesSubMenu)
                    {
                        botao.BackColor = TemaCores.CorBotaoSubMenu;
                    }
                    // Painel título
                    pnlTitulo.BackColor = TemaCores.CorBotaoSelecionado;
                    // Painel logo
                    pnlLogo.BackColor = TemaCores.CorPainel;
                    // Painéis do menu lateral
                    foreach (Panel painel in listaPaineisMenu)
                    {
                        painel.BackColor = TemaCores.CorPainel;
                    }
                    // Botão selecionado
                    botaoAtual.BackColor = TemaCores.CorBotaoSelecionado;
                    botaoAtual.ForeColor = Color.White;
                    botaoAtual.Font = new Font("Lucida Sans", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);

                    btnFecharFormFilho.Visible = true;
                }
            }
        }

        private void desselecionarBotao()
        {
            List<Panel> listaPaineis = [
                pnlMenuLateral,
                pnlCadastros,
                pnlLancamentos,
                pnlRelatorios,
            ];

            List<Button> listaBotoesMenu = [
                btnCadastro,
                btnLancamentos,
                btnRelatorios,
                btnLogoff,
            ];

            List<Button> listaBotoesSubMenu = [
                btnContas,
                btnHistoricos,
                btnUsuarios,
                btnMovimentacao,
                btnTransporte,
                btnRelDiario,
                btnRelAnalitico,
                btnBalanceteGeral,
                btnBalanceteConta,
                btnSaldo,
            ];

            // Botão anterior: Voltar fonte ao padrão
            botaoAtual.Font = new Font("Lucida Sans", 9.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            botaoAtual.ForeColor = Color.Gainsboro;

            // Desselecionar botaoAtual
            botaoAtual = null;

            // Selecionar tema
            TemaCores.Selecionar("padrão");

            // Aplicar cores do tema
            // Botões de menu
            foreach (Button botao in listaBotoesMenu)
            {
                botao.BackColor = TemaCores.CorBotaoMenu;
            }
            // Botões dropdown
            foreach (Button botao in listaBotoesSubMenu)
            {
                botao.BackColor = TemaCores.CorBotaoSubMenu;
            }
            // Painel título
            pnlTitulo.BackColor = Color.FromArgb(0, 150, 136);
            // Painel logo
            pnlLogo.BackColor = TemaCores.CorPainel;
            // Painéis restantes
            foreach (Panel painel in listaPaineis)
            {
                painel.BackColor = TemaCores.CorPainel;
            }
        }

        private void abrirFormulario(Form formularioFilho, object btnSender, string tema)
        {
            if (formularioAtivo != null)
            {
                formularioAtivo.Dispose();
            }

            TemaCores.Selecionar(tema);
            selecionarBotao(btnSender);

            formularioAtivo = formularioFilho;
            formularioFilho.Owner = this;

            formularioFilho.TopLevel = false;
            formularioFilho.FormBorderStyle = FormBorderStyle.None;
            formularioFilho.Dock = DockStyle.Fill;

            this.pnlDesktop.Controls.Add(formularioFilho);
            this.pnlDesktop.Tag = formularioFilho;

            formularioFilho.BringToFront();
            formularioFilho.Show();

            lblTitulo.Text = formularioFilho.Text;
        }

        public void Reset()
        {
            desselecionarBotao();
            esconderSubMenus();

            lblTitulo.Text = "Painel Principal";

            botaoAtual = null;
            btnFecharFormFilho.Visible = false;
        }
        private void esconderSubMenus()
        {
            pnlCadastros.Visible = false;
            pnlLancamentos.Visible = false;
            pnlRelatorios.Visible = false;
        }

        private void exibirSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                esconderSubMenus();
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;
            }
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

        private void btnFecharFormFilho_Click(object sender, EventArgs e)
        {
            if (formularioAtivo != null)
            {
                if (formularioAtivo.Text == "Usuários")
                {
                    lblUsuario.Text = usuarioAtual;
                }
                formularioAtivo.Dispose();
                Reset();
            }
        }

        private void btnCadastro_Click(object sender, EventArgs e)
        {
            exibirSubMenu(pnlCadastros);
        }

        private void btnContas_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.Cadastros.frmContas(), sender, "cadastros");
        }

        private void btnHistoricos_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.Cadastros.frmHistoricos(), sender, "cadastros");
        }

        private void btnUsuarios_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.Cadastros.frmUsuarios(lblUsuario.Text, con), sender, "cadastros");
        }

        private void btnLancamentos_Click(object sender, EventArgs e)
        {
            exibirSubMenu(pnlLancamentos);
        }

        private void btnMovimentacao_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.Lancamentos.frmMovimentacao(), sender, "lançamentos");
        }

        private void btnTransporte_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.Lancamentos.frmTransportarSaldo(), sender, "lançamentos");
        }

        private void btnRelatorios_Click(object sender, EventArgs e)
        {
            exibirSubMenu(pnlRelatorios);
        }

        private void btnRelDiario_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.Relatorios.frmRelDiario(), sender, "relatórios");
        }

        private void btnRelAnalitico_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.Relatorios.frmRelAnalitico(), sender, "relatórios");
        }

        private void btnBalanceteGeral_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.Relatorios.frmBalanceteGeral(), sender, "relatórios");
        }

        private void btnBalanceteConta_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.Relatorios.frmBalanceteConta(), sender, "relatórios");
        }

        private void btnSaldo_Click(object sender, EventArgs e)
        {
            abrirFormulario(new Forms.Relatorios.frmSaldo(), sender, "relatórios");
        }

        private void btnLogoff_Click(object sender, EventArgs e)
        {
            DialogResult input = MessageBox.Show("Deseja realmente voltar à tela de login?", "Você está prestes a sair", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (input == DialogResult.Yes)
            {
                con.desconectar();


                this.Owner.Show(); // Exibe o Formulário de Login
                this.Hide(); // Esconde o formulário atual
                this.Dispose(); // Fecha o formulário atual
            }
            else
            {
                btnFecharFormFilho.PerformClick();
            }
        }

        private void timerRelogio_Tick(object sender, EventArgs e)
        {
            lblRelogio.Text = DateTime.Now.ToString("hh:mm:ss");
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
