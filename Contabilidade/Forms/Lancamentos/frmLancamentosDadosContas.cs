using Contabilidade.Forms.Cadastros;
using Contabilidade.Models;
using System.Data;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Data.SQLite;
using Contabilidade.Classes;

namespace Contabilidade.Forms.Lancamentos
{
    public partial class frmLancamentosDadosContas : Form
    {
        // Funções usadas para permitir que a janela se movimente através da barra superior customizada
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        private extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        private extern static void SendMessage(System.IntPtr hWnd, int wMsg, int wParam, int lParam);

        Conexao con;
        static DataTable dtDados = new DataTable();
        DataView dv = dtDados.DefaultView;

        public frmLancamentosDadosContas(Conexao conexaoBanco)
        {
            InitializeComponent();

            this.con = conexaoBanco;

            atualizarDataGrid();

            txtFiltrar.Select();
        }

        private void atualizarDataGrid()
        {
            // Query de pesquisa
            string sql = "SELECT * FROM contas WHERE conta != '0' ORDER BY conta;";
            using (var command = new SQLiteCommand(sql, con.conn))
            {
                dtDados.Clear();
                using (var reader = command.ExecuteReader())
                {
                    dtDados.Load(reader);
                }

                dgvContas.DataSource = dtDados;

                dv.RowFilter = $"conta LIKE '{txtFiltrar.Text}%'";
                dgvContas.DataSource = dv;

                cbbFiltrar.SelectedIndex = 1;
                cbbNivel.SelectedIndex = 0;
                cbbNivel2.SelectedIndex = 0;
                txtFiltrar.MaxLength = 15;
            }
        }

        private void btnCriar_Click(object sender, EventArgs e)
        {
            try
            {
                // Se a descrição não for válida
                if (string.IsNullOrWhiteSpace(txtDescricao.Text))
                {
                    MessageBox.Show("A descrição não pode ser vázia!", "Descrição inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    txtDescricao.Text = "";
                    txtDescricao.Focus();
                }
                else
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
                        using (var transacao = con.conn.BeginTransaction())
                        {
                            try
                            {
                                var conta = txtConta.Text;
                                var descricao = txtDescricao.Text;
                                var tipoConta = getTipoConta(cbbNivel.SelectedIndex);

                                // Criar conta
                                string sql = "INSERT INTO contas (conta, descricao, nivel) VALUES(@conta, @descricao, @nivel);";
                                using (var comando = new SQLiteCommand(sql, con.conn))
                                {
                                    comando.Transaction = transacao;
                                    comando.Parameters.AddWithValue("@conta", txtConta.Text);
                                    comando.Parameters.AddWithValue("@descricao", txtDescricao.Text);
                                    comando.Parameters.AddWithValue("@nivel", tipoConta);

                                    int retornoBD = comando.ExecuteNonQuery();

                                    // Verificar se houve a criação da linha (0 = negativo)
                                    if (retornoBD > 0)
                                    {
                                        // Adicionar dados na tabela
                                        DataRow row = dtDados.NewRow();
                                        row["conta"] = conta;
                                        row["descricao"] = descricao;
                                        row["nivel"] = tipoConta;
                                        dtDados.Rows.Add(row);

                                        dgvContas.Refresh();

                                        // Efetivar alterações
                                        transacao.Commit();

                                        // Redefinir valores
                                        txtConta.Text = "";
                                        cbbNivel.SelectedIndex = 0;
                                        txtDescricao.Text = "";

                                        MessageBox.Show("Conta criada com sucesso!", "Criação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                    else
                                    {
                                        throw new Exception("Não foi possível criar a nova conta.");
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                transacao.Rollback();
                                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao criar a conta", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao criar a conta", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnMinimizar_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void btnFechar_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
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

        private void cbbFiltrar_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFiltrar.Text = "";
            cbbNivel2.SelectedIndex = 0;

            // Filtar por nível 
            if (cbbFiltrar.SelectedIndex == 2)
            {
                cbbNivel2.Visible = true;
                txtFiltrar.Visible = false;
                cbbNivel2_SelectedIndexChanged(sender, e);
            }
            // Filtrar por conta ou descrição 
            else
            {
                cbbNivel2.Visible = false;
                txtFiltrar.Visible = true;
                txtFiltrar_TextChanged(sender, e);
            }
        }

        private void cbbNivel2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Ambos
            if (cbbNivel2.SelectedIndex == 0)
            {
                dv.RowFilter = "";
            }
            // Analitico
            else if (cbbNivel2.SelectedIndex == 1)
            {
                dv.RowFilter = "nivel = 'A'";
            }
            // Sintetico
            else if (cbbNivel2.SelectedIndex == 2)
            {
                dv.RowFilter = "nivel = 'S'";
            }

            dgvContas.DataSource = dv;
        }

        private void txtFiltrar_TextChanged(object sender, EventArgs e)
        {
            // Conta
            if (cbbFiltrar.SelectedIndex == 0)
            {
                // Impedir da máscara ser aplicada quando não se tem dados inseridos (o que ocasiona erro)
                if (txtFiltrar.Text.Length > 0)
                {
                    TextBox textBox = sender as TextBox;
                    textBox.Text = frmContasDados.AplicarMascara(textBox.Text);
                    textBox.SelectionStart = textBox.Text.Length; // Mantém o cursor no final
                }

                txtFiltrar.MaxLength = 15;

                dv.RowFilter = $"conta LIKE '%{txtFiltrar.Text}%'";
                dgvContas.DataSource = dv;
            }
            // Descrição
            else if (cbbFiltrar.SelectedIndex == 1)
            {
                txtFiltrar.MaxLength = 100;
                dv.RowFilter = $"descricao LIKE '%{txtFiltrar.Text}%'";
                dgvContas.DataSource = dv;
            }
        }

        public static bool verificarExistenciaConta(string conta)
        {
            return dtDados.AsEnumerable().Any(row => string.Equals(conta, row.Field<string>("conta"), StringComparison.OrdinalIgnoreCase));
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

        private void dgvContas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvContas.Rows[e.RowIndex];

                var conta = row.Cells["Conta"].Value.ToString();
                var tipoConta = row.Cells["Nível"].Value.ToString();

                // Verificar se a conta selecionada não é a do caixa
                if (conta == "0")
                {
                    MessageBox.Show("Não é possível fazer lançamentos diretamente no caixa, use as contas analíticas!", "Operação não permitida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                // Verificar o tipo da conta para não permitir a seleção de contas sintéticas
                else if (tipoConta == "A")
                {
                    frmLancamentosDados.conta = conta;
                    frmLancamentosDados.descricao = row.Cells["Descrição"].Value.ToString();

                    this.DialogResult = DialogResult.OK;
                    this.Dispose();
                }
                else
                {
                    MessageBox.Show("Não é possível fazer lançamentos em contas do tipo sintético (S), por favor, selecione uma conta analítica (A)", "O tipo da conta selecionada é inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }
    }
}
