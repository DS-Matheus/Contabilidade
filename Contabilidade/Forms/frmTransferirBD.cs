using System.Runtime.InteropServices;

namespace Contabilidade.Forms
{
    public partial class frmTransferirBD : Form
    {
        // Funções usadas para permitir que a janela se movimente através da barra superior customizada
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private string nomeAntigo;
        public string nomeBD { get; private set; }
        private string pastaDatabases;

        public frmTransferirBD(string pastaDatabases, string nomeAntigo)
        {
            InitializeComponent();

            this.pastaDatabases = pastaDatabases;
            this.nomeAntigo = nomeAntigo;
            txtBancoDados.Text = nomeAntigo;

            txtBancoDados.Select();
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

        private void btnTransferir_Click(object sender, EventArgs e)
        {
            var nomeNovoSimples = txtBancoDados.Text.Trim();
            var nomeNovoCompleto = frmLogin.validarExtensaoBD(nomeNovoSimples);
            var caminhoBD = $"{pastaDatabases}\\{nomeNovoCompleto}";

            // Verifica se é nulo
            if (nomeNovoSimples == "" || nomeNovoSimples == null || string.IsNullOrWhiteSpace(nomeNovoSimples))
            {
                MessageBox.Show("Não foi informado um nome para o banco!", "Erro ao transferir banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBancoDados.Text = "";
                txtBancoDados.Focus();
            }
            // Verificar se o nome informado é igual ao que deseja-se renomear
            else if (nomeNovoSimples.ToLower() == nomeAntigo.ToLower())
            {
                MessageBox.Show("O nome informado é igual ao anterior!", "Erro ao transferir banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBancoDados.Text = "";
                txtBancoDados.Focus();
            }
            // Verifica se o arquivo já existe
            else if (File.Exists(caminhoBD))
            {
                MessageBox.Show("Já existe um banco de dados com esse nome!", "Erro ao transferir banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBancoDados.Text = "";
                txtBancoDados.Focus();
            }
            // Verifica se começa com número, hífen ou underline
            else if (frmCriarBD.verificarInicio(nomeNovoSimples[0]))
            {
                MessageBox.Show("O nome informado para o banco de dados não pode começar com um número, hífen ou sublinhado (também conhecido como underline)!", "Erro ao transferir o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBancoDados.Text = "";
                txtBancoDados.Focus();
            }
            // Verifica se a string é maior que 30 caracteres
            else if (nomeNovoSimples.Length > 30)
            {
                MessageBox.Show("O nome do banco não deve conter mais que 30 caracteres!", "Erro ao transferir o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBancoDados.Text = "";
                txtBancoDados.Focus();
            }
            // Verifica se utiliza apenas letras e números
            else if (!frmLogin.verificarNomeBD(nomeNovoSimples))
            {
                MessageBox.Show("O nome ínformado deve conter apenas letras e números (sem espaços)!", "Erro ao transferir o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBancoDados.Text = "";
                txtBancoDados.Focus();
            }
            // Passou nas verificações: enviar dados para o formulário pai
            else
            {
                nomeBD = nomeNovoCompleto;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void txtBancoDados_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnTransferir.PerformClick();
            }
            else
            {
                frmLogin.ImpedirPressionarBarraEspaco(e);
            }
        }
    }
}
