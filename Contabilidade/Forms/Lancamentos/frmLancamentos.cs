﻿using Contabilidade.Forms.Cadastros;
using Contabilidade.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Contabilidade.Forms.Lancamentos
{
    public partial class frmLancamentos : Form
    {
        Conexao con;
        static DataTable dtDados = new DataTable();
        DataView dv = dtDados.DefaultView;
        public static string conta { get; set; } = "";
        public static decimal valor { get; set; } = 0;
        public static string id_historico { get; set; } = "";
        public static DateTime data { get; set; } = DateTime.Today;

public frmLancamentos(Conexao conexaoBanco)
        {
            InitializeComponent();

            con = conexaoBanco;

            atualizarDataGrid();
        }

        public void atualizarDataGrid()
        {
            // Query de pesquisa
            string sql = "SELECT l.id, l.conta, c.descricao, l.valor, l.data, h.historico FROM lancamentos l JOIN contas c ON l.conta = c.conta JOIN historicos h ON l.id_historico = h.id ORDER BY l.conta, l.data";
            using (var command = new SQLiteCommand(sql, con.conn))
            {
                SQLiteDataAdapter sqlDA = new SQLiteDataAdapter(sql, con.conn);
                dtDados.Clear();
                sqlDA.Fill(dtDados);

                dgvLancamentos.DataSource = dtDados;

                txtFiltrar.Text = "";
                dv.RowFilter = $"conta LIKE '{txtFiltrar.Text}%'";
                dgvLancamentos.DataSource = dv;

                cbbFiltrar.SelectedIndex = 0;
            }
        }

        private void btnCriar_Click(object sender, EventArgs e)
        {
            // Criar uma instância do formulário de dados e aguardar um retorno
            using (var frmDados = new frmLancamentosDados(con, "Criar conta", "", "", 0, "", "", DateTime.Today))
            {
                // O usuário apertou o botão de salvar
                if (frmDados.ShowDialog() == DialogResult.OK)
                {
                    // Variável para armazenar o saldo da conta antes do lançamento
                    decimal saldo = 0;

                    // Verificar qual o dia do lançamento (se é hoje ou em um dia anterior)
                    var diferenciaDias = DateTime.Compare(data, DateTime.Today);
                    // Data selecionada é anterior a de hoje (-1 = antes, 0 = hoje, 1 = depois)
                    if (diferenciaDias < 0)
                    {
                        // Obter valor de saldo da conta antes do lançamento
                        string sql = "SELECT saldo_atualizado FROM lancamentos WHERE data <= @data ORDER BY data DESC, id DESC LIMIT 1;";
                        using (var comando = new SQLiteCommand(sql, con.conn))
                        {
                            comando.Parameters.AddWithValue("@data", data);

                            using (SQLiteDataReader reader = comando.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    saldo = reader.GetDecimal(0);
                                }
                            }

                            // Criar lançamento
                            comando.CommandText = "INSERT INTO lancamentos (conta, valor, id_historico, data, saldo_anterior, saldo_atualizado) VALUES(@conta, @valor, @id_historico, @data, @saldo_anterior, @saldo_atualizado);";

                            comando.Parameters.Clear();
                            comando.Parameters.AddWithValue("@conta", conta);
                            comando.Parameters.AddWithValue("@valor", valor);
                            comando.Parameters.AddWithValue("@id_historico", id_historico);
                            comando.Parameters.AddWithValue("@data", data);
                            comando.Parameters.AddWithValue("@saldo_anterior", saldo);
                            comando.Parameters.AddWithValue("@saldo_atualizado", saldo + valor);

                            int retornoBD = comando.ExecuteNonQuery();

                            // Verificar se houve a criação da linha (0 = negativo)
                            if (retornoBD > 0)
                            {
                                // Seleciona todos os registros após a data especificada para atualizar os valores de saldo_anterior e saldo_atualizado (após)
                                comando.CommandText = "SELECT id, saldo_anterior, saldo_atualizado FROM lancamentos WHERE data > @data";
                                comando.Parameters.Clear();
                                comando.Parameters.AddWithValue("@data", data);

                                // Inicia leitor de registros
                                using (var reader = comando.ExecuteReader())
                                {
                                    // Criar comando para atualizar os campos
                                    sql = "UPDATE lancamentos SET saldo_anterior = (saldo_anterior + @valor), saldo_atualizado = (saldo_atualizado + @valor) WHERE id = @id";
                                    using (var comando2 = new SQLiteCommand(sql, con.conn))
                                    {
                                        // Para cada campo encontrado
                                        while (reader.Read())
                                        {
                                            int id = reader.GetInt32(0);

                                            // Atualizar os campos no banco de dados
                                            comando2.Parameters.Clear();
                                            comando2.Parameters.AddWithValue("@valor", valor);
                                            comando2.Parameters.AddWithValue("@id", id);

                                            comando2.ExecuteNonQuery();
                                        }
                                    }
                                }

                                // Alterar saldo da conta
                                comando.CommandText = "UPDATE contas SET saldo = saldo + @valor WHERE conta = @conta";
                                comando.Parameters.AddWithValue("@valor", valor);
                                comando.Parameters.AddWithValue("@conta", conta);
                                comando.ExecuteNonQuery();

                                // Adicionar dados na tabela - Recarregar completamente
                                atualizarDataGrid();

                                // Remover dados das variáveis
                                conta = "";
                                valor = 0;
                                id_historico = "";
                                data = DateTime.Today;

                                MessageBox.Show("Lançamento criado com sucesso!", "Criação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            else
                            {
                                MessageBox.Show("Não foi possível criar um novo lançamento, por favor, anote os dados inseridos e tire um print da tela para contatar o desenvolvedor", "Lançamento não criado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    // Se a data seleciona é a de hoje
                    else
                    {
                        // Obter valor de saldo da conta antes do lançamento
                        string sql = "SELECT saldo FROM contas WHERE conta = @conta";
                        using (var comando = new SQLiteCommand(sql, con.conn))
                        {
                            comando.Parameters.AddWithValue("@conta", conta);

                            // Atribuir valor encontrado na variável de saldo
                            var result = comando.ExecuteScalar();
                            if (result != null)
                            {
                                saldo = Convert.ToDecimal(result);
                            }
                            else
                            {
                                MessageBox.Show("Não foi possível obter o saldo da conta, por favor anotes os valores inseridos e contate o desenvolvedor", "Erro ao buscar saldo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            // Criar lançamento
                            comando.CommandText = "INSERT INTO lancamentos (conta, valor, id_historico, data, saldo_anterior, saldo_atualizado) VALUES(@conta, @valor, @id_historico, @data, @saldo_anterior, @saldo_atualizado);";

                            comando.Parameters.AddWithValue("@conta", conta);
                            comando.Parameters.AddWithValue("@valor", valor);
                            comando.Parameters.AddWithValue("@id_historico", id_historico);
                            comando.Parameters.AddWithValue("@data", data);
                            comando.Parameters.AddWithValue("@saldo_anterior", saldo);
                            comando.Parameters.AddWithValue("@saldo_atualizado", saldo + valor);

                            int retornoBD = comando.ExecuteNonQuery();

                            // Verificar se houve a criação da linha (0 = negativo)
                            if (retornoBD > 0)
                            {
                                // Alterar saldo conta
                                comando.CommandText = "UPDATE contas SET saldo = saldo + @valor WHERE conta = @conta";
                                comando.Parameters.AddWithValue("@valor", valor);
                                comando.Parameters.AddWithValue("@conta", conta);
                                comando.ExecuteNonQuery();

                                // Adicionar dados na tabela - Recarregar completamente
                                atualizarDataGrid();

                                // Remover dados das variáveis
                                conta = "";
                                valor = 0;
                                id_historico = "";
                                data = DateTime.Today;

                                MessageBox.Show("Lançamento criado com sucesso!", "Criação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            }
                            else
                            {
                                MessageBox.Show("Não foi possível criar um novo lançamento, por favor, anote os dados inseridos e tire um print da tela para contatar o desenvolvedor", "Lançamento não criado", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                }
            }
        }
    }
}