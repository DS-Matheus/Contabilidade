using Microsoft.Data.Sqlite;

namespace Contabilidade.Models
{
    public class Conexao(string caminho)
    {
        public SqliteConnection conn = new SqliteConnection("Data Source=" + caminho);

        public void conectar()
        {
            conn.Open();
        }

        public void desconectar()
        {
            conn.Close();
        }

        public void excluir()
        {
            conn.Dispose();
        }
    }
}
