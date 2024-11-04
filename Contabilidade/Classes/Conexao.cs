using Microsoft.Data.Sqlite;

namespace Contabilidade.Models
{
    public class Conexao(string caminho)
    {
        public SqliteConnection conn = new SqliteConnection("Data Source=" + caminho);

        public void Conectar()
        {
            conn.Open();
        }

        public void Desconectar()
        {
            conn.Dispose();
        }
    }
}
