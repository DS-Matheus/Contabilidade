namespace Contabilidade.Classes
{
    internal class CustomException : Exception
    {
        public CustomException(string mensagem) : base(mensagem)
        {
        }
    }
}