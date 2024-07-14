using Contabilidade.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Contabilidade.Forms.Cadastros
{
    public partial class frmUsuarios : Form
    {
        string usuarioAtual;
        Conexao con;
        static DataTable dtDados = new DataTable();
        public static string usuario { get; set; } = "";
        public static string senha { get; set; } = "";

        public frmUsuarios(string usuario, Conexao conexaoBanco)
        {
            InitializeComponent();

            con = conexaoBanco;
            usuarioAtual = usuario;

            atualizarDataGrid();
        }

        public void atualizarDataGrid()
        {
            // Query de pesquisa
            string sql = "SELECT nome, senha FROM usuarios;";
            using (var command = new SQLiteCommand(sql, con.conn))
            {
                SQLiteDataAdapter sqlDA = new SQLiteDataAdapter(sql, con.conn);
                dtDados.Clear();
                sqlDA.Fill(dtDados);

                dgvUsuarios.DataSource = dtDados;
            }
        }

        public static bool verificarExistenciaUsuario(string usuario)
        {
            return dtDados.AsEnumerable().Any(row => usuario == row.Field<string>("nome"));
        }

        private void btnCriar_Click(object sender, EventArgs e)
        {
            // Criar uma instância do formulário de dados e aguardar um retorno
            using (var frmDados = new frmUsuariosDados("Criar usuário", "", ""))
            {
                // O usuário apertou o botão de salvar
                if (frmDados.ShowDialog() == DialogResult.OK)
                {
                    // Criar usuário
                    string sql = "INSERT INTO usuarios (nome, senha) VALUES(@nome, @senha);";
                    using (var comando = new SQLiteCommand(sql, con.conn))
                    {
                        comando.Parameters.AddWithValue("@nome", usuario);
                        comando.Parameters.AddWithValue("@senha", senha);

                        int retornoBD = comando.ExecuteNonQuery();

                        // Verificar se houve a criação da linha (0 = negativo)
                        if (retornoBD > 0)
                        {
                            // Adicionar dados na tabela
                            DataRow row = dtDados.NewRow();
                            row["nome"] = usuario;
                            row["senha"] = senha;
                            dtDados.Rows.Add(row);

                            dgvUsuarios.Refresh();

                            // Remover dados das variáveis
                            usuario = "";
                            senha = "";

                            MessageBox.Show("Usuário criado com sucesso!", "Criação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else
                        {
                            MessageBox.Show("Não foi possível criar o novo usuário.", "Usuário não criado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }


        private bool IsTextboxEmpty(List<string> listaConteudos)
        {
            foreach (string conteudo in listaConteudos)
            {
                if (string.IsNullOrWhiteSpace(conteudo))
                {
                    return false;
                }
            }
            return true;
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {

            int numLinha = obterNumLinhaSelecionada(dgvUsuarios);
            var (usuario, senha) = obterDadosDGV(numLinha);

            DialogResult input = MessageBox.Show($"Deseja realmente excluir o usuário {usuario}?", "Confirmação de exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (input == DialogResult.Yes)
            {
                using (var comando = new SQLiteCommand("DELETE FROM usuarios WHERE nome = @nome and senha = @senha", con.conn))
                {
                    comando.Parameters.AddWithValue("@nome", usuario);
                    comando.Parameters.AddWithValue("@senha", senha);

                    int retornoBD = comando.ExecuteNonQuery();

                    // Verificar se houve a exclusão de alguma linha (0 = negativo)
                    if (retornoBD > 0)
                    {
                        // Excluir do DataTable
                        dtDados.Rows.RemoveAt(numLinha);
                        dgvUsuarios.Refresh();
                        MessageBox.Show("Usuário excluído com sucesso!", "Exclusão bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    }
                    else
                    {
                        MessageBox.Show("Não foi possível encontrar o usuário ou ocorreu um erro na exclusão.", "Exclusão não realizada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
        }

        private void frmUsuarios_Load(object sender, EventArgs e)
        {

        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Obter usuário selecionado
            int numLinha = obterNumLinhaSelecionada(dgvUsuarios);
            var (usuarioAntigo, senhaAntiga) = obterDadosDGV(numLinha);

            // Criar uma instância do formulário de dados e aguardar um retorno
            using (var frmDados = new frmUsuariosDados("Editar usuário", usuarioAntigo, senhaAntiga))
            {
                // O usuário apertou o botão de salvar
                if (frmDados.ShowDialog() == DialogResult.OK)
                {
                    // Editar usuário
                    using (var comando = new SQLiteCommand("UPDATE usuarios SET nome = @nome, senha = @senha WHERE nome = @nomeAntigo and senha = @senhaAntiga", con.conn))
                    {
                        comando.Parameters.AddWithValue("@nome", usuario);
                        comando.Parameters.AddWithValue("@senha", senha);
                        comando.Parameters.AddWithValue("@nomeAntigo", usuarioAntigo);
                        comando.Parameters.AddWithValue("@senhaAntiga", senhaAntiga);

                        int retornoBD = comando.ExecuteNonQuery();

                        // Verificar se houve a edição de alguma linha (0 = negativo)
                        if (retornoBD > 0)
                        {
                            // Atualizar DataTable
                            dgvUsuarios.Rows[numLinha].Cells["Usuário"].Value = usuario;
                            dgvUsuarios.Rows[numLinha].Cells["Senha"].Value = senha;

                            dgvUsuarios.Refresh();

                            // Remover dados das variáveis
                            usuario = "";
                            senha = "";

                            MessageBox.Show("Usuário editado com sucesso!", "Edição bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        else
                        {
                            MessageBox.Show("Não foi possível encontrar o usuário ou ocorreu um erro na edição.", "Exclusão não realizada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        public int obterNumLinhaSelecionada(DataGridView dataGridView)
        {
            return dataGridView.CurrentRow.Index;
        }

        private (string usuario, string senha) obterDadosDGV(int numLinha)
        {
            string usuario = dgvUsuarios.Rows[numLinha].Cells["Usuário"].Value?.ToString();
            string senha = dgvUsuarios.Rows[numLinha].Cells["Senha"].Value?.ToString();

            return (usuario, senha);
        }

        private void txtFiltrar_TextChanged(object sender, EventArgs e)
        {
            if (cbbFiltrar.Text == "Usuário")
            {
                DataView dv = dtDados.DefaultView;
                dv.RowFilter = "nome LIKE '" + txtFiltrar.Text + "%'";
                dgvUsuarios.DataSource = dv;
            }
            else if (cbbFiltrar.Text == "Senha")
            {
                DataView dv = dtDados.DefaultView;
                dv.RowFilter = "senha LIKE '" + txtFiltrar.Text + "%'";
                dgvUsuarios.DataSource = dv;
            }
        }

        private void cbbFiltrar_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFiltrar_TextChanged(sender, e);
        }
    }
}
