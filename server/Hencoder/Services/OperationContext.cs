namespace Hencoder.Services
{
    /// <summary>
    /// Контекст исполнения операций
    /// </summary>
    public sealed class OperationContext
    {
        /// <summary>
        /// Инициатор операции
        /// </summary>
        public string CurrentUserToken { get; private set; }

        public OperationContext(string token)
        {
            CurrentUserToken = token;
        }
    }
}
