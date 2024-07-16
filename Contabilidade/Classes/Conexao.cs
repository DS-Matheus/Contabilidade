using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;

namespace Contabilidade.Models
{
    public class Conexao(string caminho)
    {
        public SQLiteConnection conn = new SQLiteConnection("Data Source=" + caminho);

        public void conectar()
        {
            conn.Open();
        }

        public void desconectar()
        {
            conn.Close();
        }
    }
}
