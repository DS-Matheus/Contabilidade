using Contabilidade.Models;
using System.Runtime.InteropServices;

namespace Contabilidade.Forms.Lancamentos
{
    public partial class frmLancamentosDados : Form
    {
        Conexao con;
        // Funções usadas para permitir que a janela se movimente através da barra superior customizada
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        // Variáveis de dados
        public static string conta { get; set; } = "";
        public static string descricao { get; set; } = "";
        public static string id_historico { get; set; } = "";
        public static string historico { get; set; } = "";

        public frmLancamentosDados(Conexao conexaoBanco, string titulo, string conta, string descricao, decimal valor, string id_historico, string historico, DateTime data)
        {
            InitializeComponent();

            con = conexaoBanco;

            txtConta.Text = conta;
            txtDescricao.Text = descricao;
            txtHistorico.Text = historico;
            nudValor.Value = valor;
            frmLancamentosDados.conta = conta;
            frmLancamentosDados.descricao = descricao;
            frmLancamentosDados.id_historico = id_historico;
            frmLancamentosDados.historico = historico;
            dtpData.Value = data;
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

        private void btnSelecionarConta_Click(object sender, EventArgs e)
        {
            // Criar uma instância do formulário de dados e aguardar um retorno
            using (var frmDados = new frmLancamentosDadosContas(con))
            {
                // O usuário apertou o botão de salvar
                if (frmDados.ShowDialog() == DialogResult.OK)
                {
                    txtConta.Text = conta;
                    txtDescricao.Text = descricao;
                }
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

        private void btnSelecionarHistorico_Click(object sender, EventArgs e)
        {
            // Criar uma instância do formulário de dados e aguardar um retorno
            using (var frmDados = new frmLancamentosDadosHistorico(con))
            {
                // O usuário apertou o botão de salvar
                if (frmDados.ShowDialog() == DialogResult.OK)
                {
                    txtHistorico.Text = historico;
                }
            }
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtConta.Text) || string.IsNullOrWhiteSpace(txtDescricao.Text))
            {
                MessageBox.Show("Não foi selecionado uma conta para o lançamento", "Conta inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (string.IsNullOrWhiteSpace(txtHistorico.Text) || string.IsNullOrWhiteSpace(id_historico))
            {
                MessageBox.Show("Não foi selecionado um histórico para o lançamento", "Histórico inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (nudValor.Value == 0)
            {
                MessageBox.Show("O valor do lançamento não pode ser igual a R$ 0,00!", "Valor de lançamento inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                // Envia os dados para o formulário pai
                frmLancamentos.conta = txtConta.Text;
                frmLancamentos.id_historico = id_historico;
                frmLancamentos.valor = nudValor.Value;
                frmLancamentos.data = dtpData.Value;

                this.DialogResult = DialogResult.OK;
                this.Dispose();
            }
        }

        private void btnAlterarSinal_Click(object sender, EventArgs e)
        {
            var valor = nudValor.Value;

            if (valor > 9999999.99m)
            {
                decimal novoValor;
                if (decimal.TryParse(valor.ToString().Substring(1), out novoValor))
                {
                    valor = novoValor;
                }
                else
                {
                    // Trate o erro de conversão aqui, se necessário
                    Console.WriteLine("Erro ao converter o valor.");
                }
                nudValor.Value = valor;
            }
            else
            {
                nudValor.Value = valor * -1;
            }

        }
    }
}
