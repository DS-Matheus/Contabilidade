using Contabilidade.Forms.Cadastros;
using Contabilidade.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

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
        }

        public void atualizarDataGrid()
        {
            // Query de pesquisa
            string sql = "SELECT * FROM contas ORDER BY conta;";
            using (var command = new SQLiteCommand(sql, con.conn))
            {
                SQLiteDataAdapter sqlDA = new SQLiteDataAdapter(sql, con.conn);
                dtDados.Clear();
                sqlDA.Fill(dtDados);

                dgvContas.DataSource = dtDados;

                dv.RowFilter = $"conta LIKE '{txtFiltrar.Text}%'";
                dgvContas.DataSource = dv;

                cbbFiltrar.SelectedIndex = 0;
            }
        }

        private void btnCriar_Click(object sender, EventArgs e)
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
                    var conta = txtConta.Text;
                    var descricao = txtDescricao.Text;
                    var tipoConta = getTipoConta(cbbNivel.SelectedIndex);

                    // Criar conta
                    string sql = "INSERT INTO contas (conta, descricao, nivel, saldo) VALUES(@conta, @descricao, @nivel, @saldo);";
                    using (var comando = new SQLiteCommand(sql, con.conn))
                    {
                        comando.Parameters.AddWithValue("@conta", txtConta.Text);
                        comando.Parameters.AddWithValue("@descricao", txtDescricao.Text);
                        comando.Parameters.AddWithValue("@nivel", tipoConta);
                        comando.Parameters.AddWithValue("@saldo", tipoConta == "S" ? null : 0);

                        int retornoBD = comando.ExecuteNonQuery();

                        // Verificar se houve a criação da linha (0 = negativo)
                        if (retornoBD > 0)
                        {
                            // Adicionar dados na tabela
                            DataRow row = dtDados.NewRow();
                            row["conta"] = conta;
                            row["descricao"] = descricao;
                            row["nivel"] = tipoConta;
                            row["saldo"] = (tipoConta == "S" ? DBNull.Value : 0);
                            dtDados.Rows.Add(row);

                            dgvContas.Refresh();

                            MessageBox.Show("Conta criada com sucesso!", "Criação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else
                        {
                            MessageBox.Show("Não foi possível criar a nova conta.", "Conta não criada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
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
            // Filtar por nível 
            if (cbbFiltrar.SelectedIndex == 2)
            {
                cbbNivel2.Visible = true;
                txtFiltrar.Visible = false;
                txtFiltrar2.Visible = false;
            }
            // Filtrar por Saldo entre
            else if (cbbFiltrar.SelectedIndex == 5)
            {
                cbbNivel2.Visible = false;
                txtFiltrar.Visible = true;
                txtFiltrar2.Visible = true;
                txtFiltrar.Width = 115;
            }
            else
            {
                cbbNivel2.Visible = false;
                txtFiltrar.Visible = true;
                txtFiltrar2.Visible = false;
                txtFiltrar.Width = 238;
            }

            txtFiltrar_TextChanged(sender, e);
        }

        private void txtFiltrar2_TextChanged(object sender, EventArgs e)
        {
            txtFiltrar_TextChanged(sender, e);
        }

        private void txtFiltrar_TextChanged(object sender, EventArgs e)
        {
            // Conta
            if (cbbFiltrar.SelectedIndex == 0)
            {
                dv.RowFilter = $"conta LIKE '{txtFiltrar.Text}%'";
                dgvContas.DataSource = dv;
            }
            // Descrição
            else if (cbbFiltrar.SelectedIndex == 1)
            {
                dv.RowFilter = $"descricao LIKE '{txtFiltrar.Text}%'";
                dgvContas.DataSource = dv;
            }
            // Nível
            else if (cbbFiltrar.SelectedIndex == 2)
            {
                // Analitico
                if (cbbNivel2.SelectedIndex == 0)
                {
                    dv.RowFilter = "nivel = 'A'";
                }
                // Sintetico
                else if (cbbNivel2.SelectedIndex == 1)
                {
                    dv.RowFilter = "nivel = 'S'";
                }
                // Ambos
                else if (cbbNivel2.SelectedIndex == 2)
                {
                    dv.RowFilter = "";
                }

                dgvContas.DataSource = dv;
            }
            // Saldo menor que
            else if (cbbFiltrar.SelectedIndex == 3)
            {
                if (IsNumeric(txtFiltrar.Text))
                {
                    dv.RowFilter = $"saldo <= {txtFiltrar.Text}";
                    dgvContas.DataSource = dv;
                }
            }
            // Saldo maior que
            else if (cbbFiltrar.SelectedIndex == 4)
            {
                if (IsNumeric(txtFiltrar.Text))
                {
                    dv.RowFilter = $"saldo >= {txtFiltrar.Text}";
                    dgvContas.DataSource = dv;
                }
            }
            // Saldo entre
            else if (cbbFiltrar.SelectedIndex == 5)
            {
                if (IsNumeric(txtFiltrar.Text) && IsNumeric(txtFiltrar2.Text))
                {
                    var valores = ObterMenorEMaior(int.Parse(txtFiltrar.Text), int.Parse(txtFiltrar2.Text));
                    dv.RowFilter = $"saldo BETWEEN {valores.Item1} AND {valores.Item2}";
                    dgvContas.DataSource = dv;
                }
            }
        }

        private bool IsNumeric(string text)
        {
            return int.TryParse(text, out _);
        }

        public static (int, int) ObterMenorEMaior(int valor1, int valor2)
        {
            if (valor1 < valor2)
            {
                return (valor1, valor2);
            }
            else
            {
                return (valor2, valor1);
            }
        }

        public static bool verificarExistenciaConta(string conta)
        {
            return dtDados.AsEnumerable().Any(row => conta == row.Field<string>("conta"));
        }

        public static bool verificarContaSintetica(string conta)
        {
            // Remove o último grupo de caracteres após o último ponto (assim se obtêm a conta sintética associada)
            int ultimoPonto = conta.LastIndexOf('.');
            string contaSintetica = conta;
            if (ultimoPonto != -1)
            {
                contaSintetica = conta.Substring(0, ultimoPonto);
            }

            // Verificar se a conta existe e retornar
            return verificarExistenciaConta(contaSintetica);
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

                frmLancamentosDados.conta = row.Cells["Conta"].Value.ToString();
                frmLancamentosDados.descricao = row.Cells["Descrição"].Value.ToString();

                this.DialogResult = DialogResult.OK;
                this.Dispose();
            }
        }
    }
}
