using System.Data.SQLite;

namespace Contabilidade.Models
{
    public class Conexao(string caminho)
    {
        public SQLiteConnection conn = new SQLiteConnection("Data Source=" + caminho);

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
