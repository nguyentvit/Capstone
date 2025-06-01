namespace Capstone.Domain.SubjectDomain.ValueObjects
{
    public record SubjectId
    {
        public Guid Value { get; }
        private SubjectId(Guid value) => Value = value;
        public static SubjectId Of(Guid value)
        {
            return new SubjectId(value);
        }
    }
}
