namespace Contabilidade.Models
{
    public static class TemaCores
    {
        // Variáveis estáticas públicas com getters e setters
        public static System.Drawing.Color CorBotaoSelecionado { get; private set; }
        public static System.Drawing.Color CorBotaoMenu { get; private set; }
        public static System.Drawing.Color CorBotaoSubMenu { get; private set; }
        public static System.Drawing.Color CorPainel { get; private set; }

        // Dicionário associando índices a listas de strings (agora com cores em hexadecimal)
        private static Dictionary<string, List<string>> dicionarioStrings = new Dictionary<string, List<string>>
        {
            // Barra de títulos, Botões menus, Botões sub-menus, Fundo painel lateral
            { "padrão", new List<string> { "#777787", "#33334C", "#3D3D5B", "#27263A" } },
            { "cadastros", new List<string> { "#794ee6", "#362367", "#4d3291", "#20153c" } },
            { "lançamentos", new List<string> { "#20AA87", "#105544", "#188066", "#082a22" } },
            { "relatórios", new List<string> { "#d4176a", "#3b2144", "#6e1e51", "#082438" } },
            { "logoff", new List<string> { "#cc4040", "#003f5c", "#2c4875", "#00202e" } },
        };

        // Método para atribuir as cores às variáveis públicas
        public static void Selecionar(string indice)
        {
            if (dicionarioStrings.ContainsKey(indice))
            {
                CorBotaoSelecionado = ColorTranslator.FromHtml(dicionarioStrings[indice][0]);
                CorBotaoMenu = ColorTranslator.FromHtml(dicionarioStrings[indice][1]);
                CorBotaoSubMenu = ColorTranslator.FromHtml(dicionarioStrings[indice][2]);
                CorPainel = ColorTranslator.FromHtml(dicionarioStrings[indice][3]);
            }
            else
            {
                Console.WriteLine($"Tema '{indice}' não encontrado.");
            }
        }
    }
}