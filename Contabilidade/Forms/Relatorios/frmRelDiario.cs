using Contabilidade.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
    }
}
