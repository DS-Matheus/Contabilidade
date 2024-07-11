using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Contabilidade.Models
{
    public static class TemaCores
    {
        // Variáveis estáticas públicas com getters e setters
        public static System.Drawing.Color CorBotaoSelecionado { get; private set; }
        public static System.Drawing.Color CorBotaoMenu { get; private set; }
        public static System.Drawing.Color CorBotaoSubMenu { get; private set; }
        public static System.Drawing.Color CorPainelMenu { get; private set; }
        public static System.Drawing.Color CorPainelLogo { get; private set; }
        public static System.Drawing.Color CorPainelTitulo { get; private set; }

        // Dicionário associando índices a listas de strings (agora com cores em hexadecimal)
        private static Dictionary<string, List<string>> dicionarioStrings = new Dictionary<string, List<string>>
        {
            { "padrão", new List<string> { "#777787", "#33334C", "#3D3D5B", "#27263A", "#27263A", "#777787" } },
            { "cadastros", new List<string> { "#FFFFFF", "#FF0000", "#00FF00", "#0000FF", "#FF00FF", "#FFFF00" } },
            { "lançamentos", new List<string> { "#000000", "#00FF00", "#FF0000", "#FF00FF", "#FFFF00", "#0000FF" } },
            { "relatórios", new List<string> { "#0000FF", "#6699FF", "#3366FF", "#0033CC", "#0A2239", "#777787" } },
            { "logoff", new List<string> { "#FF0000", "#FF6666", "#FF3333", "#CC0000", "#0A2239", "#777787" } },
        };

        // Método para atribuir as cores às variáveis públicas
        public static void Selecionar(string indice)
        {
            if (dicionarioStrings.ContainsKey(indice))
            {
                CorBotaoSelecionado = ColorTranslator.FromHtml(dicionarioStrings[indice][0]);
                CorBotaoMenu = ColorTranslator.FromHtml(dicionarioStrings[indice][1]);
                CorBotaoSubMenu = ColorTranslator.FromHtml(dicionarioStrings[indice][2]);
                CorPainelMenu = ColorTranslator.FromHtml(dicionarioStrings[indice][3]);
                CorPainelLogo = ColorTranslator.FromHtml(dicionarioStrings[indice][4]);
                CorPainelTitulo = ColorTranslator.FromHtml(dicionarioStrings[indice][5]);
            }
            else
            {
                Console.WriteLine($"Tema '{indice}' não encontrado.");
            }
        }
    }
}