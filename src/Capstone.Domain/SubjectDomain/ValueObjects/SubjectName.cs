namespace Capstone.Domain.SubjectDomain.ValueObjects
{
    public record SubjectName
    {
        public string Value { get; }
        private SubjectName(string value) => Value = value;
        public static SubjectName Of(string value)
        {
            return new SubjectName(value);
        }
    }
}
