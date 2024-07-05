using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contabilidade.Models
{
    public static class TemaCores
    {
        public static Color corPrimaria { get; set; }
        public static Color corSecundaria { get; set; }
        public static List<string> listaCores = new List<string>() {"#3F51B5",
                                                                    "#009688",
                                                                    "#FF5722",
                                                                    "#607D8B",
                                                                    "#FF9800",
                                                                    "#9C27B0",
                                                                    "#2196F3",
                                                                    "#EA676C",
                                                                    "#E41A4A",
                                                                    "#5978BB",
                                                                    "#018790",
                                                                    "#0E3441",
                                                                    "#00B0AD",
                                                                    "#721D47",
                                                                    "#EA4833",
                                                                    "#EF937E",
                                                                    "#F37521",
                                                                    "#A12059",
                                                                    "#126881",
                                                                    "#8BC240",
                                                                    "#364D5B",
                                                                    "#C7DC5B",
                                                                    "#0094BC",
                                                                    "#E4126B",
                                                                    "#43B76E",
                                                                    "#7BCFE9",
                                                                    "#B71C46"};
        public static Color alterarBrilhoCor(Color cor, double fatorCorrecao)
        {
            double vermelho = cor.R;
            double verde = cor.G;
            double azul = cor.B;
            //Se o fator de correção for menor que 0: escurece a cor.
            if (fatorCorrecao < 0)
            {
                fatorCorrecao = 1 + fatorCorrecao;
                vermelho *= fatorCorrecao;
                verde *= fatorCorrecao;
                azul *= fatorCorrecao;
            }
            //Se o fator de correção for maior que 0, clareia a cor.
            else
            {
                vermelho = (255 - vermelho) * fatorCorrecao + vermelho;
                verde = (255 - verde) * fatorCorrecao + verde;
                azul = (255 - azul) * fatorCorrecao + azul;
            }
            return Color.FromArgb(cor.A, (byte)vermelho, (byte)verde, (byte)azul);
        }
    }
}
