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

namespace Contabilidade.Forms.Relatorios
{
    public partial class frmBalanceteGeral : Form
    {
        Conexao con;
        public frmBalanceteGeral(Conexao conexaoBanco)
        {
            InitializeComponent();

            con = conexaoBanco;

            // Defina a propriedade MaxDate para a data atual
            dtpInicial.MaxDate = DateTime.Today;
            dtpFinal.MaxDate = DateTime.Today;
        }

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            var data1 = dtpInicial.Value;
            var data2 = dtpFinal.Value;

            // Se a data 2 for mais recente, trocar de variável com a mais antiga
            if (data1 < data2)
            {
                data1 = data2;
                data2 = dtpInicial.Value;
            }

            var sql = "";
            var comando = new SQLiteCommand(sql, con.conn);
            comando.Parameters.AddWithValue("@data1", data1);
            comando.Parameters.AddWithValue("@data2", data2);

            // Criar uma instância do formulário de dados e aguardar um retorno
            using (var frmDados = new frmExibirRelatorio("Balancete Geral", comando))
            {
                // O usuário apertou o botão de salvar
                if (frmDados.ShowDialog() == DialogResult.OK)
                {
                    dtpInicial.Focus();
                }
            }
        }
    }
}
