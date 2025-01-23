using Contabilidade.Forms.Cadastros;
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
using System.Globalization;
using System.Text.RegularExpressions;

namespace Contabilidade
{
    public partial class frmCriarBD : Form
    {
        // Funções usadas para permitir que a janela se movimente através da barra superior customizada
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        public string nomeBD { get; private set; }
        public string usuario { get; private set; }
        public string senha { get; private set; }
        private string pastaDatabases;

        public frmCriarBD(string pastaDatabases)
        {
            InitializeComponent();

            this.pastaDatabases = pastaDatabases;
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

        public static bool verificarInicio(char input)
        {
            // Define os caracteres proibidos
            string padrao = @"[0-9-_]";

            // Converte o char para string para usar a expressão regular
            string inputString = input.ToString();

            // Verifica se o caractere está no conjunto proibido
            if (Regex.IsMatch(inputString, padrao))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void btnCriar_Click(object sender, EventArgs e)
        {
            var nomeBancoSimples = txtBancoDados.Text;
            var nomeBancoCompleto = frmLogin.validarExtensaoBD(nomeBancoSimples);
            var caminhoBD = $"{pastaDatabases}\\{nomeBancoCompleto}";

            // Verifica se o nome do banco é nulo
            if (nomeBancoSimples == "" || nomeBancoSimples == null || string.IsNullOrWhiteSpace(nomeBancoSimples))
            {
                MessageBox.Show("Não foi informado um nome para o banco de dados!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBancoDados.Text = "";
                txtBancoDados.Focus();
            }
            // Verifica se começa com número, hífen ou underline
            else if (verificarInicio(nomeBancoSimples[0]))
            {
                MessageBox.Show("O nome informado para o banco de dados não pode começar com um número, hífen ou sublinhado (também conhecido como underline)!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBancoDados.Text = "";
                txtBancoDados.Focus();
            }
            // Verifica se a string é maior que 30 caracteres
            else if (nomeBancoSimples.Length > 30)
            {
                MessageBox.Show("O nome do banco não deve conter mais que 30 caracteres!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBancoDados.Text = "";
                txtBancoDados.Focus();
            }
            // Verifica se utiliza apenas letras e números
            else if (!frmLogin.verificarNomeBD(nomeBancoSimples))
            {
                MessageBox.Show("O nome ínformado para o banco de dados deve conter apenas letras e números (sem espaços ou acentos)!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBancoDados.Text = "";
                txtBancoDados.Focus();
            }
            // Verifica se o arquivo já existe
            else if (File.Exists(caminhoBD))
            {
                MessageBox.Show("Já existe um banco de dados com o nome informado!", "Erro ao criar o banco de dados", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtBancoDados.Text = "";
                txtBancoDados.Focus();
            }
            // Se o usuário não for válido
            else if (!frmLogin.verificarUsuario(txtUsuario.Text))
            {
                txtUsuario.Text = "";
                txtUsuario.Focus();
            }
            // Se a senha não for válida
            else if (!frmLogin.verificarSenha(txtSenha.Text))
            {
                txtSenha.Text = "";
                txtSenha.Focus();
            }
            else
            {
                // Armazenar dados para o formulário pai
                nomeBD = nomeBancoCompleto;
                usuario = txtUsuario.Text;
                senha = txtSenha.Text;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }

        private void txtBancoDados_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtUsuario.Select();
            }
            else
            {
                frmLogin.ImpedirPressionarBarraEspaco(e);
            }
        }

        private void txtUsuario_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                txtSenha.Select();
            }
            else
            {
                frmLogin.ImpedirPressionarBarraEspaco(e);
            }
        }

        private void txtSenha_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                btnCriar.PerformClick();
            }
            else
            {
                frmLogin.ImpedirPressionarBarraEspaco(e);
            }
        }
    }
}
