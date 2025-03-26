namespace Common.Exceptions
{
    public class NoStrategyException : Exception
    {
        public NoStrategyException()
        {
        }

        public NoStrategyException(string? message) : base(message)
        {
        }

        public NoStrategyException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
