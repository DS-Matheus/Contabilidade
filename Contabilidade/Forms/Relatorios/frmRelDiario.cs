using Contabilidade.Forms.Cadastros;
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
    public partial class frmRelDiario : Form
    {
        Conexao con;
        public frmRelDiario(Conexao conexaoBanco)
        {
            InitializeComponent();

            con = conexaoBanco;
        }

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            var sql = "SELECT l.conta, c.descricao, h.historico, CASE WHEN l.valor >= 0 THEN l.valor ELSE NULL END AS creditos, CASE WHEN l.valor < 0 THEN l.valor ELSE NULL END AS debitos FROM lancamentos l JOIN contas c ON l.conta = c.conta JOIN historicos h ON l.id_historico = h.id WHERE date(l.data) = @data UNION ALL SELECT ' ' AS conta, NULL AS descricao, NULL AS historico, NULL AS creditos, NULL AS debitos UNION ALL SELECT 'Total do Dia' AS conta, NULL AS descricao, NULL AS historico, SUM(CASE WHEN l.valor >= 0 THEN l.valor ELSE 0 END) AS creditos, SUM(CASE WHEN l.valor < 0 THEN l.valor ELSE 0 END) AS debitos FROM lancamentos l JOIN contas c ON l.conta = c.conta JOIN historicos h ON l.id_historico = h.id WHERE date(l.data) = @data;";
            var comando = new SQLiteCommand(sql, con.conn);
            comando.Parameters.AddWithValue("@data", dtpData.Value);

            // Criar uma instância do formulário de dados e aguardar um retorno
            using (var frmDados = new frmExibirRelatorio("Relatório Diário", comando))
            {
                // O usuário apertou o botão de salvar
                if (frmDados.ShowDialog() == DialogResult.OK)
                {
                    dtpData.Focus();
                }
            }
        }
    }
}
