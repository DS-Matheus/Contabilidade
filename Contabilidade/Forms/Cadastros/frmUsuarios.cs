using Contabilidade.Models;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Contabilidade.Forms.Cadastros
{
    public partial class frmUsuarios : Form
    {
        string usuarioAtual;
        Conexao con;

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
            SQLiteCommand comando = new SQLiteCommand(sql, con.conn);

            // Consultar usuários com os parâmetros informados
            SQLiteDataReader reader = comando.ExecuteReader();
            // Passando os dados encontrados pelo DataAdapter para o DataTable
            while (reader.Read())
            {
                // Obtém os valores das colunas
                string nome = reader.GetString(0);
                string senha = reader.GetString(1);

                // Adiciona uma nova linha ao DataGridView
                dgvUsuarios.Rows.Add(nome, senha);
            }

            // Liberar recursos
            reader.Close();
            comando.Dispose();
        }

        private void dgvUsuarios_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (e.RowIndex >= 0) // Verifica se o clique foi em uma célula válida
            {
                DataGridViewRow row = dgvUsuarios.Rows[rowIndex];

                txtUsuario.Text = row.Cells[0].Value.ToString();
                txtSenha.Text = row.Cells[1].Value.ToString();
            }
        }

        private bool verificarExistenciaUsuario(string usuario)
        {
            return dgvUsuarios.Rows.Cast<DataGridViewRow>().Any(row => row.Cells["nome"].Value?.ToString() == usuario);
        }

        private void btnCriar_Click(object sender,  EventArgs e)
        {
            // Se o usuário não for válido
            if (!frmLogin.verificarUsuario(txtUsuario.Text))
            {
                txtUsuario.Text = "";
                txtUsuario.Focus();
            }
            // Se a senha não fo válida
            else if (!frmLogin.verificarSenha(txtSenha.Text))
            {
                txtSenha.Text = "";
                txtSenha.Focus();
            }
            // Se o usuário já existir
            else if (verificarExistenciaUsuario(txtUsuario.Text))
            {
                MessageBox.Show("O usuário informado já existe!", "Erro ao criar usuário", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsuario.Text = "";
                txtUsuario.Focus();
            }
            else
            {
                string sql = "INSERT INTO usuarios (nome, senha) VALUES(@nome, @senha);";
                SQLiteCommand comando = new SQLiteCommand(sql, con.conn);

                comando.Parameters.AddWithValue("@nome", txtUsuario.Text);
                comando.Parameters.AddWithValue("@senha", txtSenha.Text);
                comando.ExecuteNonQuery();

                comando.Dispose();

                dgvUsuarios.Rows.Add(txtUsuario.Text, txtSenha.Text);
            }    
        }
    }
}
