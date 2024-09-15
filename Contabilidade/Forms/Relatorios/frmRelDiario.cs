using Contabilidade.Forms.Cadastros;
using Contabilidade.Models;
using iTextSharp.text;
using iTextSharp.text.pdf;
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

        private class Lancamento
        {
            public string Conta { get; set; }
            public string Descricao { get; set; }
            public string Historico { get; set; }
            public decimal Valor { get; set; }
        }

        private void btnVisualizar_Click(object sender, EventArgs e)
        {
            var sql = "SELECT l.conta, c.descricao, h.historico, l.valor FROM lancamentos l JOIN contas c ON l.conta = c.conta JOIN historicos h ON l.id_historico = h.id WHERE date(l.data) = @data ORDER BY l.conta ASC, l.id ASC;";
            var comando = new SQLiteCommand(sql, con.conn);
            comando.Parameters.AddWithValue("@data", dtpData.Value.ToString("yyyy-MM-dd"));

            using (SQLiteDataReader reader = comando.ExecuteReader())
            {
                List<Lancamento> listLancamentos = new List<Lancamento>();

                while (reader.Read())
                {
                    Lancamento lancamento = new Lancamento
                    {
                        Conta = reader["conta"].ToString(),
                        Descricao = reader["descricao"].ToString(),
                        Historico = reader["historico"].ToString(),
                        Valor = Convert.ToDecimal(reader["valor"])
                    };
                    listLancamentos.Add(lancamento);
                }

                // Caminho do arquivo PDF
                string pdfPath = "exemplo.pdf";

                // Criação do documento
                Document document = new Document(PageSize.A4, 36, 36, 36, 36); // Margens padrão (36 pontos)

                // Caminho do arquivo de fonte Consolas
                string fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fontes", "consola.ttf");

                // Verifica se o arquivo de fonte existe
                if (!File.Exists(fontPath))
                {
                    Console.Error.WriteLine("Arquivo de fonte não encontrado, faça backup dos bancos de dados e reinstale o programa");
                    return;
                }

                // Criação do escritor de PDF
                PdfWriter writer = PdfWriter.GetInstance(document, new FileStream(pdfPath, FileMode.Create));

                // Abrindo o documento
                document.Open();

                // Configuração da fonte Consola
                BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                var font = new iTextSharp.text.Font(bf, 11);

                // Adicionando parágrafos ao documento
                document.Add(new Paragraph("                            RELATÓRIO DIÁRIO - 10/09/2024                 PÁGINA: 0001", font));
                document.Add(new Paragraph("                     SUBTÍTULO PERSONALIZADO DE ATÉ 80 CARACTERES                     ", font));
                document.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", font));
                document.Add(new Paragraph("CONTA           DESCRIÇÃO                                                             ", font));
                document.Add(new Paragraph("HISTÓRICO                                                          DÉBITOS    CRÉDITOS", font));
                document.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", font));

                // Fechando o documento
                document.Close();
            }
        }
    }
}
