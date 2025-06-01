namespace BuildingBlocks.Exceptions
{
    public class BussinessException : Exception
    {
        public List<string> Errors { get; }
        public BussinessException(string message, List<string> errors) : base(message) 
        {
            Errors = errors;
        }
    }
}
