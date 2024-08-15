using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Contabilidade.Forms.Cadastros
{
    public partial class frmContasDados : Form
    {
        // Funções usadas para permitir que a janela se movimente através da barra superior customizada
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);
        private int operacao = 0;

        public frmContasDados(string titulo, string conta, string descricao, string nivel, int operacao = 0)
        {
            InitializeComponent();

            this.Text = titulo;
            this.lblTitulo.Text = titulo;

            txtConta.Text = conta;
            txtDescricao.Text = descricao;
            this.operacao = operacao;

            txtConta.Select();
            if (nivel == "S")
            {
                cbbNivel.SelectedIndex = 1;
            }
            else
            {
                cbbNivel.SelectedIndex = 0;
            }

            // Usar diferente de 0 para quando o registro já existir (ex: operação de editar registro)
            if (operacao != 0)
            {
                txtConta.Enabled = false;
                cbbNivel.Enabled = false;
            }
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

        public static bool verificarConta(string conta)
        {
            string[] formatos = {
                @"^[A-Z0-9]{2}$", // Formato 1: XX
                @"^[A-Z0-9]{2}\.[A-Z0-9]{3}$", // Formato 2: XX.XXX
                @"^[A-Z0-9]{2}\.[A-Z0-9]{3}\.[A-Z0-9]{3}$", // Formato 3: XX.XXX.XXX
                @"^[A-Z0-9]{2}\.[A-Z0-9]{3}\.[A-Z0-9]{3}\.[A-Z0-9]{4}$" // Formato 4: XX.XXX.XXX.XXXX
            };

            foreach (string formato in formatos)
            {
                if (Regex.IsMatch(conta, formato))
                {
                    return true;
                }
            }

            MessageBox.Show("O formato da conta informada está incorreto!\n" +
                "\n" +
                "Padrões definidos:\n" +
                "XX\n" +
                "XX.XXX\n" +
                "XX.XXX.XXX\n" +
                "XX.XXX.XXX.XXXX",
                "Conta inválida",
                MessageBoxButtons.OK,
                MessageBoxIcon.Warning
            );
            return false;
        }

        private void btnSalvar_Click(object sender, EventArgs e)
        {
            // Se a descrição não for válida
            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
            {
                MessageBox.Show("A descrição não pode ser vázia!", "Descrição inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescricao.Text = "";
                txtDescricao.Focus();
            }
            // Se estiver no modo de criação
            else if (operacao == 0)
            {
                // Se a conta não for válida
                if (!verificarConta(txtConta.Text))
                {
                    txtConta.Text = "";
                    txtConta.Focus();
                }
                // Se a conta já existir
                else if (frmContas.verificarExistenciaConta(txtConta.Text))
                {
                    MessageBox.Show("A conta informada já existe!", "Erro ao informar conta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConta.Text = "";
                    txtConta.Focus();
                }
                // Se a conta for do tipo analítica mas tentou ser criada na "raiz" (apenas 1 número)
                else if (Regex.IsMatch(txtConta.Text, @"^[A-Z0-9]{2}$") && cbbNivel.SelectedIndex == 0)
                {
                    MessageBox.Show("Não é possível ter uma conta analítica antes da sintética!", "'Numero/Tipo de conta inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbbNivel.Focus();
                }
                // Se a conta for do tipo analítica e tentou se criar antes da sintética
                else if (!Regex.IsMatch(txtConta.Text, @"^[A-Z0-9]{2}$") && cbbNivel.SelectedIndex == 0 && !frmContas.verificarContaSintetica(txtConta.Text))
                {
                    MessageBox.Show("Não é possível ter uma conta analítica antes da sintética!", "'Numero/Tipo de conta inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbbNivel.Focus();
                }
                // Se a conta for do tipo sintética maior que 1 e não existe uma conta sintética de nível menor
                else if (!Regex.IsMatch(txtConta.Text, @"^[A-Z0-9]{2}$") && cbbNivel.SelectedIndex == 1 && !frmContas.verificarContaSintetica(txtConta.Text))
                {
                    MessageBox.Show("Hierarquia de contas inválida!\n\nÉ preciso criar uma conta sintética de nível menor antes.", "'Numero de conta inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    cbbNivel.Focus();
                }
                else
                {
                    var tipoConta = getTipoConta(cbbNivel.SelectedIndex);

                    // Envia os dados para o formulário pai
                    frmContas.conta = txtConta.Text;
                    frmContas.descricao = txtDescricao.Text;
                    frmContas.nivel = tipoConta;

                    this.DialogResult = DialogResult.OK;
                    this.Dispose();
                }    
            }
            // Caso seja outra operação
            else
            {
                // Envia os dados para o formulário pai
                frmContas.descricao = txtDescricao.Text;

                this.DialogResult = DialogResult.OK;
                this.Dispose();
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

        private void txtConta_TextChanged(object sender, EventArgs e)
        {
            // Impedir da máscara ser aplicada quando não se tem dados inseridos (o que ocasiona erro)
            if (txtConta.Text.Length > 0)
            {
                TextBox textBox = sender as TextBox;
                textBox.Text = AplicarMascara(textBox.Text);
                textBox.SelectionStart = textBox.Text.Length; // Mantém o cursor no final
            }
        }

        private string AplicarMascara(string input)
        {
            // Remove todos os caracteres não alfanuméricos
            string cleanInput = Regex.Replace(input, @"[^a-zA-Z0-9]", "");

            if (cleanInput.Length <= 2)
            {
                return cleanInput; // Caso a string tenha 2 ou menos caracteres
            }
            else if (cleanInput.Length <= 5)
            {
                return cleanInput.Insert(2, ".");
            }
            else if (cleanInput.Length <= 8)
            {
                return cleanInput.Insert(2, ".").Insert(6, ".");
            }
            else if (cleanInput.Length <= 12)
            {
                return cleanInput.Insert(2, ".").Insert(6, ".").Insert(10, ".");
            }
            else
            {
                return cleanInput; // Caso a string seja maior que 12 caracteres
            }
        }


        private void txtConta_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ' ')
            {
                e.Handled = true; // Impede a digitação do espaço
            }
        }

        public static string getTipoConta(int opcTipoConta)
        {
            if (opcTipoConta == 0)
            {
                return "A";
            }   
            else
            {
                return "S";
            }
        }
    }
}
