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

namespace Contabilidade.Forms
{
    public partial class frmRenomearBD : Form
    {
        // Funções usadas para permitir que a janela se movimente através da barra superior customizada
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        private string nomeAntigo;
        public string nomeBD { get; private set; }
        private string pastaDatabases;

        public frmRenomearBD(string pastaDatabases, string nomeAntigo)
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

        private void btnRenomear_Click(object sender, EventArgs e)
        {
            var nomeNovoSimples = txtBancoDados.Text;
            var nomeNovoCompleto = frmLogin.validarExtensaoBD(nomeNovoSimples);
            var caminhoBD = $"{pastaDatabases}\\{nomeNovoCompleto}";

            // Adiantando alguns testes
            var arquivoExiste = File.Exists(caminhoBD);
            var nomeExatamenteIgual = nomeNovoSimples == nomeAntigo;

            // Verifica se é nulo
            if (nomeNovoSimples == "" || nomeNovoSimples == null || string.IsNullOrWhiteSpace(nomeNovoSimples))
            {
                MessageBox.Show("Não foi informado um nome para o banco!", "Erro ao renomear banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBancoDados.Text = "";
                txtBancoDados.Focus();
            }
            // Verificar se o nome informado é exatamente igual ao que deseja-se renomear
            else if (nomeExatamenteIgual)
            {
                MessageBox.Show("O nome informado é exatamente igual ao anterior!", "Erro ao renomear banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBancoDados.Text = "";
                txtBancoDados.Focus();
            }
            // Verifica se o arquivo já existe (considerar que não caso o nome novo e o antigo sejam iguais mas tenham letras em maiusculo/minusculo diferentes)
            // Se o arquivo não existe: falso (vai pro próximo if)
            // Se o arquivo existe e o nome é igual: true (erro)
            // Se o arquivo existe e o nome é diferente: false (vai pro próximo if)
            else if (arquivoExiste && nomeExatamenteIgual)
            {
                MessageBox.Show("Já existe um banco de dados com esse nome!", "Erro ao renomear banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBancoDados.Text = "";
                txtBancoDados.Focus();
            }
            // Verifica se começa com número, hífen ou underline
            else if (frmCriarBD.verificarInicio(nomeNovoSimples[0]))
            {
                MessageBox.Show("O nome informado para o banco de dados não pode começar com um número, hífen ou sublinhado (também conhecido como underline)!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBancoDados.Text = "";
                txtBancoDados.Focus();
            }
            // Verifica se a string é maior que 30 caracteres
            else if (nomeNovoSimples.Length > 30)
            {
                MessageBox.Show("O nome do banco não deve conter mais que 30 caracteres!", "Erro ao renomear banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBancoDados.Text = "";
                txtBancoDados.Focus();
            }
            // Verifica se utiliza apenas letras e números
            else if (!frmLogin.verificarNomeBD(nomeNovoSimples))
            {
                MessageBox.Show("O nome ínformado deve conter apenas letras e números (sem espaços)!", "Erro ao renomear banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
    }
}
