﻿using Contabilidade.Models;
using DGVPrinterHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Contabilidade.Forms.Cadastros
{
    public partial class frmContas : Form
    {
        Conexao con;
        static DataTable dtDados = new DataTable();
        DataView dv = dtDados.DefaultView;
        public static string conta { get; set; } = "";
        public static string descricao { get; set; } = "";
        public static string nivel { get; set; } = "";

        public frmContas(Conexao conexaoBanco)
        {
            InitializeComponent();

            con = conexaoBanco;

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

        private void cbbFiltrar_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Filtar por nível 
            if (cbbFiltrar.SelectedIndex == 2)
            {
                cbbNivel.Visible = true;
                txtFiltrar.Visible = false;
                txtFiltrar2.Visible = false;
            }
            // Filtrar por Saldo entre
            else if (cbbFiltrar.SelectedIndex == 5)
            {
                cbbNivel.Visible = false;
                txtFiltrar.Visible = true;
                txtFiltrar2.Visible = true;
                txtFiltrar.Width = 115;
            }
            else
            {
                cbbNivel.Visible = false;
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
                if (cbbNivel.SelectedIndex == 0)
                {
                    dv.RowFilter = "nivel = 'A'";
                }
                // Sintetico
                else if (cbbNivel.SelectedIndex == 1)
                {
                    dv.RowFilter = "nivel = 'S'";
                }
                // Ambos
                else if (cbbNivel.SelectedIndex == 2)
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

        private void btnCriar_Click(object sender, EventArgs e)
        {
            // Criar uma instância do formulário de dados e aguardar um retorno
            using (var frmDados = new frmContasDados("Criar conta", "", "", "A"))
            {
                // O usuário apertou o botão de salvar
                if (frmDados.ShowDialog() == DialogResult.OK)
                {
                    // Criar conta
                    string sql = "INSERT INTO contas (conta, descricao, nivel, saldo) VALUES(@conta, @descricao, @nivel, @saldo);";
                    using (var comando = new SQLiteCommand(sql, con.conn))
                    {
                        comando.Parameters.AddWithValue("@conta", conta);
                        comando.Parameters.AddWithValue("@descricao", descricao);
                        comando.Parameters.AddWithValue("@nivel", nivel);
                        comando.Parameters.AddWithValue("@saldo", nivel == "S" ? null : 0);

                        int retornoBD = comando.ExecuteNonQuery();

                        // Verificar se houve a criação da linha (0 = negativo)
                        if (retornoBD > 0)
                        {
                            // Adicionar dados na tabela
                            DataRow row = dtDados.NewRow();
                            row["conta"] = conta;
                            row["descricao"] = descricao;
                            row["nivel"] = nivel;
                            row["saldo"] = (nivel == "S" ? DBNull.Value : 0);
                            dtDados.Rows.Add(row);

                            dgvContas.Refresh();

                            // Remover dados das variáveis
                            conta = "";
                            descricao = "";
                            nivel = "";

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

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            // Solicita o título do arquivo ao usuário
            string inputTitle = Microsoft.VisualBasic.Interaction.InputBox("Digite o título do arquivo:", "Título do Arquivo", "");

            // Verifica se o título está vazio ou contém apenas espaços
            string title = string.IsNullOrWhiteSpace(inputTitle) ? "Contas Cadastradas" : inputTitle;

            var printer = new DGVPrinter();
            printer.Title = "Contas Cadastradas";
            printer.SubTitle = string.Format("Data: {0}", System.DateTime.Now.ToString("dd/MM/yyyy"));
            printer.SubTitleFormatFlags = StringFormatFlags.LineLimit | StringFormatFlags.NoClip;
            printer.PageNumbers = true;
            printer.PageNumberInHeader = false;
            printer.PorportionalColumns = true;
            printer.HeaderCellAlignment = StringAlignment.Near;
            printer.FooterSpacing = 15;
            printer.PrintDataGridView(dgvContas);
        }
    }
}
