using Contabilidade.Models;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using Contabilidade.Forms.Relatorios;
using System.Text;

namespace Contabilidade.Forms.Cadastros
{
    public partial class frmUsuarios : Form
    {
        string usuarioAtual;
        Conexao con;
        static DataTable dtDados = new DataTable();
        DataView dv = dtDados.DefaultView;
        public static string usuario { get; set; } = "";
        public static string senha { get; set; } = "";

        public frmUsuarios(string usuarioBD, Conexao conexaoBanco)
        {
            InitializeComponent();

            con = conexaoBanco;
            usuarioAtual = usuarioBD;

            atualizarDataGrid();

            dgvUsuarios.Columns["ID"].Visible = false;
        }

        private void atualizarDataGrid()
        {
            // Query de pesquisa
            string sql = "SELECT * FROM usuarios;";
            using (var command = new SQLiteCommand(sql, con.conn))
            {
                dtDados.Clear();
                using (var reader = command.ExecuteReader())
                {
                    dtDados.Load(reader);
                }

                dgvUsuarios.DataSource = dtDados;

                dv.RowFilter = $"nome LIKE '{txtFiltrar.Text}%'";
                dgvUsuarios.DataSource = dv;

                txtFiltrar.MaxLength = 20;
                cbbFiltrar.SelectedIndex = 0;
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
                            using (var command = new SQLiteCommand("SELECT last_insert_rowid();", con.conn))
                            {
                                var id = (Int64)command.ExecuteScalar();

                                // Adicionar dados na tabela
                                DataRow row = dtDados.NewRow();
                                row["id"] = id;
                                row["nome"] = usuario;
                                row["senha"] = senha;
                                dtDados.Rows.Add(row);

                                dgvUsuarios.Refresh();

                                // Remover dados das variáveis
                                usuario = "";
                                senha = "";

                                MessageBox.Show("Usuário criado com sucesso!", "Criação bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Não foi possível criar o novo usuário.", "Usuário não criado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        private void excluirUsuario(string id)
        {
            using (var comando = new SQLiteCommand("DELETE FROM usuarios WHERE id = @id", con.conn))
            {
                comando.Parameters.AddWithValue("@id", id);

                int retornoBD = comando.ExecuteNonQuery();

                // Verificar se houve a exclusão de alguma linha (0 = negativo)
                if (retornoBD > 0)
                {
                    // Encontrar registro no DataTable
                    DataRow[] rows = dtDados.Select($"ID = {id}");
                    // Excluir do DataTable
                    if (rows.Length > 0)
                    {
                        // Encontrou o usuário, podemos excluí-lo
                        rows[0].Delete();
                        dtDados.AcceptChanges();
                        MessageBox.Show("Usuário excluído com sucesso!", "Exclusão bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("DataGridView não atualizado, comunique o desenvolvedor!", "Exclusão com erros", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    dgvUsuarios.Refresh();
                }
                else
                {
                    MessageBox.Show("Não foi possível encontrar o usuário ou ocorreu um erro na exclusão.", "Exclusão não realizada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
        }

        private void btnExcluir_Click(object sender, EventArgs e)
        {
            int numLinha = obterNumLinhaSelecionada(dgvUsuarios);
            string id = dgvUsuarios.Rows[numLinha].Cells["ID"].Value.ToString();
            string usuario = dgvUsuarios.Rows[numLinha].Cells["Usuário"].Value?.ToString();

            DialogResult input = MessageBox.Show($"Deseja realmente excluir o usuário {usuario}?", "Confirmação de exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

            if (input == DialogResult.Yes)
            {
                if (!verificarUsuarioAtual(usuario))
                {
                    excluirUsuario(id);
                }
                else
                {
                    if (dtDados.Rows.Count <= 1)
                    {
                        MessageBox.Show("Não é permitido excluir o único usuário do banco de dados!", "Exclusão abortada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else
                    {
                        input = MessageBox.Show($"Você está prestes a excluir o usuário atual! Se confirmar você retornará para a tela de login, deseja continuar?", "Confirmação de exclusão", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);

                        if (input == DialogResult.Yes)
                        {
                            excluirUsuario(id);
                            this.Owner.Dispose();
                            this.Dispose();
                        }
                    }
                }
            }
        }

        public static bool verificarUsuarioAtual(string usuario)
        {
            if (frmPainelPrincipal.usuarioAtual == usuario)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Obter usuário selecionado
            int numLinha = obterNumLinhaSelecionada(dgvUsuarios);
            var id = dgvUsuarios.Rows[numLinha].Cells["ID"].Value;
            var (usuarioAntigo, senhaAntiga) = obterDadosDGV(numLinha);

            // Criar uma instância do formulário de dados e aguardar um retorno
            using (var frmDados = new frmUsuariosDados("Editar usuário", usuarioAntigo, senhaAntiga))
            {
                // O usuário apertou o botão de salvar
                if (frmDados.ShowDialog() == DialogResult.OK)
                {
                    // Editar usuário
                    using (var comando = new SQLiteCommand("UPDATE usuarios SET nome = @nome, senha = @senha WHERE id = @id", con.conn))
                    {
                        comando.Parameters.AddWithValue("@nome", usuario);
                        comando.Parameters.AddWithValue("@senha", senha);
                        comando.Parameters.AddWithValue("@id", id);

                        int retornoBD = comando.ExecuteNonQuery();

                        // Verificar se houve a edição de alguma linha (0 = negativo)
                        if (retornoBD > 0)
                        {
                            // Caso o usuário atual foi editado, atualizar no painel principal
                            if (verificarUsuarioAtual(usuarioAntigo))
                            {
                                frmPainelPrincipal.usuarioAtual = usuario;
                            }

                            // Atualizar DataTable
                            dgvUsuarios.Rows[numLinha].Cells["Usuário"].Value = usuario;
                            dgvUsuarios.Rows[numLinha].Cells["Senha"].Value = senha;

                            dgvUsuarios.Refresh();

                            // Remover dados das variáveis
                            usuario = "";
                            senha = "";

                            MessageBox.Show("Usuário editado com sucesso!", "Edição bem sucedida", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Não foi possível encontrar o usuário ou ocorreu um erro na edição.", "Exclusão não realizada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }

        public static int obterNumLinhaSelecionada(DataGridView dataGridView)
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
            // Filtrar por usuário
            if (cbbFiltrar.SelectedIndex == 0)
            {
                txtFiltrar.MaxLength = 20;
                dv.RowFilter = $"nome LIKE '%{txtFiltrar.Text}%'";
                dgvUsuarios.DataSource = dv;
            }
            // Filtrar por senha
            else
            {
                txtFiltrar.MaxLength = 30;
                dv.RowFilter = $"senha LIKE '%{txtFiltrar.Text}%'";
                dgvUsuarios.DataSource = dv;
            }
        }

        private void cbbFiltrar_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFiltrar.Text = "";
            txtFiltrar_TextChanged(sender, e);
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                // Exibir caixa de diálogo para o usuário definir o cabeçalho
                string titulo, subtitulo = "";

                using (var formDados = new frmDefinirCabecalho($"Usuários cadastrados em {DateTime.Today.ToString("dd/MM/yyyy")}"))
                {
                    var resultado = formDados.ShowDialog();

                    // Se clicar em salvar
                    if (resultado == DialogResult.OK)
                    {
                        (titulo, subtitulo) = (formDados.titulo, formDados.subtitulo);
                    }
                    // Se cancelar ou fechar de alguma forma
                    else
                    {
                        return;
                    }
                }

                // Exibir caixa de diálogo para o usuário escolher onde salvar o arquivo PDF
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PDF Files|*.pdf";
                    saveFileDialog.Title = "Salvar relatório como";
                    saveFileDialog.FileName = "relatorio.pdf";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        // Caminho do arquivo PDF selecionado pelo usuário
                        string pdfPath = saveFileDialog.FileName;

                        // Criação do documento
                        var pdf = new iTextSharp.text.Document(PageSize.A4, 25, 25, 25, 25); // Margens padrão (36 pontos)

                        // Caminho do arquivo de fonte Consolas
                        string fontPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Fontes", "consola.ttf");

                        // Verifica se o arquivo de fonte existe
                        if (!File.Exists(fontPath))
                        {
                            MessageBox.Show("Arquivo de fonte não encontrado, faça backup dos bancos de dados e reinstale o programa", "Erro ao buscar aquivo de fonte", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Usar `using` para garantir que o arquivo será liberado após ser usado
                        using (FileStream fileStream = new FileStream(pdfPath, FileMode.Create, FileAccess.Write, FileShare.None))
                        {
                            // Criação do escritor de PDF
                            PdfWriter writer = PdfWriter.GetInstance(pdf, fileStream);

                            // Abrindo o documento
                            pdf.Open();

                            // Configuração da fonte Consola
                            BaseFont bf = BaseFont.CreateFont(fontPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                            var fonte = new iTextSharp.text.Font(bf, 9);

                            var linhasDisponiveis = 57;

                            // Função local para adicionar o cabeçalho
                            void adicionarCabecalho(string titulo, string subtitulo)
                            {
                                // Adicionando parágrafos ao documento
                                pdf.Add(new Paragraph($"{titulo?.PadRight(98)} PÁGINA: {(pdf.PageNumber + 1).ToString("D3")}", fonte));
                                if (!string.IsNullOrWhiteSpace(subtitulo))
                                {
                                    pdf.Add(new Paragraph($"{subtitulo}", fonte));
                                    linhasDisponiveis--;
                                }
                                pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                pdf.Add(new Paragraph("USUÁRIO              SENHA                            |    USUÁRIO              SENHA                         ", fonte));
                                pdf.Add(new Paragraph("━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━", fonte));
                                pdf.Add(new Paragraph("    ", fonte));

                                // Contar linhas usadas após adição do cabeçalho
                                linhasDisponiveis -= 5;
                            };

                            // Operações com o subtítulo
                            if (!string.IsNullOrWhiteSpace(subtitulo))
                            {
                                subtitulo = frmSaldo.CentralizarString(subtitulo, 110);
                            }

                            // Adicionar cabeçalho da primeira página
                            adicionarCabecalho(titulo, subtitulo);

                            // Iniciar string builder
                            var stringLinha = new StringBuilder();
                            var qtdLinhas = dgvUsuarios.Rows.Count;

                            // Testar se haverá uma linha com apenas 1 registro (número ímpar) ou não (par)
                            var numeroImpar = true;
                            if (qtdLinhas % 2 == 0)
                            {
                                numeroImpar = false;
                            }

                            for (int i = 0; i < qtdLinhas; i+=2) {
                                // Testar se possuí uma linha disponível para usar
                                if (linhasDisponiveis - 1 < 0)
                                {
                                    pdf.NewPage();
                                    linhasDisponiveis = 57;
                                    adicionarCabecalho(titulo, subtitulo);
                                }

                                // Imprimir última linha de um dgv com número de registros ímpar
                                if (numeroImpar && (i + 2 > qtdLinhas))
                                {
                                    var usuario = dgvUsuarios.Rows[i].Cells["Usuário"].Value?.ToString();
                                    var senha = dgvUsuarios.Rows[i].Cells["Senha"].Value?.ToString();

                                    pdf.Add(new Paragraph($"{usuario?.PadRight(20)} {senha?.PadRight(30)}", fonte));
                                }
                                else
                                {
                                    var usuario1 = dgvUsuarios.Rows[i].Cells["Usuário"].Value?.ToString();
                                    var usuario2 = dgvUsuarios.Rows[i+1].Cells["Usuário"].Value?.ToString();
                                    var senha1 = dgvUsuarios.Rows[i].Cells["Senha"].Value?.ToString();
                                    var senha2 = dgvUsuarios.Rows[i+1].Cells["Senha"].Value?.ToString();

                                    pdf.Add(new Paragraph($"{usuario1?.PadRight(20)} {senha1?.PadRight(30)}   |    {usuario2?.PadRight(20)} {senha2?.PadRight(30)}", fonte));
                                }

                                linhasDisponiveis--;
                            }

                            // Fechando o documento
                            pdf.Close();
                        }
                        // Abrir o arquivo PDF gerado
                        Process.Start(new ProcessStartInfo(pdfPath) { UseShellExecute = true });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Por favor anote a mensagem de erro: \n\n{ex.Message?.ToString()}", "Erro ao gerar o relatório", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
