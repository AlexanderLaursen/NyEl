namespace Common.Exceptions
{
    public class UnkownUserException : Exception
    {
        public UnkownUserException()
        {
        }

        public UnkownUserException(string? message) : base(message)
        {
        }

        public UnkownUserException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}