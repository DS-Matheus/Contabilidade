using Contabilidade.Models;
using System.Data.SQLite;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

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
        private string contaAntiga = "";
        private string nivelAntigo = "";
        private bool alterouNivel = false;
        Conexao con;

        public frmContasDados(string titulo, string conta, string descricao, string nivel, int operacao = 0, Conexao conexaoBanco = null)
        {
            InitializeComponent();

            if (conexaoBanco != null)
            {
                this.con = conexaoBanco;
            }

            this.Text = titulo;
            this.lblTitulo.Text = titulo;

            // 0 = criar | diferente de 0 = editar
            this.operacao = operacao;

            // Dados da conta
            txtConta.Text = conta;
            this.contaAntiga = conta;

            // Bloquear alteração do número/nível de conta se for o caixa
            if (conta == "0")
            {
                txtConta.Enabled = false;
                cbbNivel.Enabled = false;
            }

            txtDescricao.Text = descricao;
            txtConta.Select();
            this.nivelAntigo = nivel;
            if (nivel == "S")
            {
                cbbNivel.SelectedIndex = 1;
            }
            else
            {
                cbbNivel.SelectedIndex = 0;
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
            var contaNova = txtConta.Text;
            var nivelConta = getNivelConta(cbbNivel.SelectedIndex);

            // Se a descrição não for válida
            if (string.IsNullOrWhiteSpace(txtDescricao.Text))
            {
                MessageBox.Show("A descrição não pode ser vázia!", "Descrição inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtDescricao.Text = "";
                txtDescricao.Focus();
                return;
            }
            // Se a conta não for válida
            else if (!verificarConta(txtConta.Text))
            {
                txtConta.Text = "";
                txtConta.Focus();
                return;
            }
            // Se esteja criando: verificar se a conta já existe
            else if (operacao == 0 && frmContas.verificarExistenciaConta(txtConta.Text))
            {
                MessageBox.Show("A conta informada já existe!", "Erro ao informar conta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConta.Text = "";
                txtConta.Focus();
                return;
            }
            // Se a conta for do tipo analítica mas tentou ser criada na "raiz" (apenas 1 número)
            else if (Regex.IsMatch(txtConta.Text, @"^[A-Z0-9]{2}$") && cbbNivel.SelectedIndex == 0)
            {
                MessageBox.Show("Não é possível ter uma conta analítica antes da sintética!", "'Numero/Tipo de conta inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbNivel.Focus();
                return;
            }
            // Se a conta for do tipo analítica e tentou se criar antes da sintética
            else if (!Regex.IsMatch(txtConta.Text, @"^[A-Z0-9]{2}$") && cbbNivel.SelectedIndex == 0 && !frmContas.verificarContaSintetica(txtConta.Text))
            {
                MessageBox.Show("Não é possível ter uma conta analítica antes da sintética!", "'Numero/Tipo de conta inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbNivel.Focus();
                return;
            }
            // Se a conta for do tipo sintética maior que 1 e não existe uma conta sintética de nível menor
            else if (!Regex.IsMatch(txtConta.Text, @"^[A-Z0-9]{2}$") && cbbNivel.SelectedIndex == 1 && !frmContas.verificarContaSintetica(txtConta.Text))
            {
                MessageBox.Show("Hierarquia de contas inválida!\n\nÉ preciso criar uma conta sintética de nível menor antes.", "'Numero de conta inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbNivel.Focus();
                return;
            }
            // Se a conta seguir o padrão xx.xxx.xxx.xxxx e for do tipo sintética
            else if (Regex.IsMatch(txtConta.Text, @"^[A-Z0-9]{2}\.[A-Z0-9]{3}\.[A-Z0-9]{3}\.[A-Z0-9]{4}$") && cbbNivel.SelectedIndex == 1)
            {
                MessageBox.Show("Hierarquia de contas inválida!\n\nNão é possível criar uma conta sintética no último nível de conta, apenas analíticas.", "Número de conta inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cbbNivel.Focus();
                return;
            }
            // Se estiver em modo de edição, realizar perguntas de verificação antes de enviar os dados para edição
            else if (operacao != 0)
            {
                // verificar se a conta já existe (desconsiderar a conta anterior, pois o usuário pode ter alterado apenas a descrição/nível)
                if (frmContas.verificarExistenciaConta(txtConta.Text, contaAntiga))
                {
                    MessageBox.Show("A conta informada já existe!", "Erro ao informar conta", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtConta.Text = "";
                    txtConta.Focus();
                    return;
                }

                // Se o número de conta é diferente do novo, mas o nível se manteve sintético: confirmar a alteração do número 
                if (contaAntiga != contaNova && nivelAntigo == nivelConta && nivelConta == "S")
                {
                    var sql = "SELECT count(conta) FROM contas WHERE conta LIKE @conta || '.%';";
                    using (var comando = new SQLiteCommand(sql, con.conn))
                    {
                        comando.Parameters.AddWithValue("@conta", contaAntiga);
                        var registros = Convert.ToInt32(comando.ExecuteScalar());

                        if (registros > 0)
                        {
                            // ao alterar o número de uma conta sintética, alterar o número de todas as suas contas filhas também
                            var dialogResult = MessageBox.Show($"Ao alterar o número de uma conta sintética, você estará alterando também o número de {registros} {(registros == 1 ? "conta" : "contas")} analíticas que estão nesse grupo de chave, deseja continuar?", "Confirmação de alteração do número de conta", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                            if (dialogResult != DialogResult.Yes)
                            {
                                return;
                            }
                        }
                    }
                }

                // Se o nível antigo é diferente do novo
                if (nivelAntigo != nivelConta)
                {
                    alterouNivel = true;

                    using (var comando = new SQLiteCommand("", con.conn))
                    {
                        // Se alterou o nível de sintético para analitico -> verificar se tem alguma conta analitica no grupo antes, se tiver: avisar antes de excluir todas e os seus lançamentos
                        if (nivelConta == "A")
                        {
                            comando.CommandText = "SELECT count(conta) FROM contas WHERE conta LIKE @conta || '.%';";
                            comando.Parameters.AddWithValue("@conta", contaAntiga);
                            var registros = Convert.ToInt32(comando.ExecuteScalar());

                            if (registros > 0)
                            {
                                var dialogResult = MessageBox.Show($"Você deseja realmente alterar a conta para o tipo analítico? Isso resultará na exclusão das contas dentro desse grupo de chaves, esse processo NÃO É reversível!", "Confirmação de alteração do tipo de conta", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (dialogResult != DialogResult.Yes)
                                {
                                    return;
                                }

                                dialogResult = MessageBox.Show($"Ao alterar a conta para o tipo analítico você irá excluir {registros} {(registros == 1 ? "conta" : "contas")} dentro desse grupo sintético, deseja ainda assim continuar? Esta é a última confirmação!", "Confirmação de alteração do tipo de conta", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (dialogResult != DialogResult.Yes)
                                {
                                    return;
                                }
                            }
                        }
                        // Se alterou o nível de analítico para sintético -> verificar se tem algum lançamento, se tiver: avisar antes de excluir todos
                        else if (nivelConta == "S")
                        {
                            comando.CommandText = "SELECT count(id) FROM lancamentos WHERE conta = @conta;";
                            comando.Parameters.AddWithValue("@conta", contaAntiga);
                            var registros = Convert.ToInt32(comando.ExecuteScalar());

                            if (registros > 0)
                            {
                                var dialogResult = MessageBox.Show($"Você deseja realmente alterar a conta para o tipo sintético? Isso resultará na exclusão de todos os lançamentos realizados por ela e esse processo NÃO PODE ser desfeito!", "Confirmação de alteração do tipo de conta", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (dialogResult != DialogResult.Yes)
                                {
                                    return;
                                }

                                dialogResult = MessageBox.Show($"Ao alterar a conta para o tipo sintético você irá excluir {registros} {(registros == 1 ? "registro feito" : "registros feitos")} por essa conta, deseja ainda assim continuar? Esta é a última confirmação!", "Confirmação de alteração do tipo de conta", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (dialogResult != DialogResult.Yes)
                                {
                                    return;
                                }
                            }
                        }
                        // Caso o tipo da conta não é S ou A
                        else
                        {
                            MessageBox.Show("Erro ao obter o tipo de conta, por favor, tente novamente.", "Erro ao editar a conta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
            }

            // Envia os dados para o formulário pai se tudo foi bem sucedido
            frmContas.conta = contaNova;
            frmContas.descricao = txtDescricao.Text;
            frmContas.nivel = nivelConta;
            frmContas.alterouNivel = alterouNivel;

            this.DialogResult = DialogResult.OK;
            this.Dispose();
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

        public static string AplicarMascara(string input)
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

        public static string getNivelConta(int indexSelecionado)
        {
            if (indexSelecionado == 0)
            {
                return "A";
            }
            else
            {
                return "S";
            }
        }

        private void txtDescricao_KeyPress(object sender, KeyPressEventArgs e)
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
