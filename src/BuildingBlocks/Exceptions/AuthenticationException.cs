namespace BuildingBlocks.Exceptions
{
    public class AuthenticationException : Exception
    {
        public string? Details { get; }
        public AuthenticationException(string message) : base(message) { }
        public AuthenticationException(string message, string details) : base(message)
        {
            Details = details;
        }
    }
}
