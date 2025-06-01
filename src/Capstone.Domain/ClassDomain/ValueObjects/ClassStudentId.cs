namespace Capstone.Domain.ClassDomain.ValueObjects
{
    public record ClassStudentId
    {
        public Guid Value { get; }
        private ClassStudentId(Guid value) => Value = value;
        public static ClassStudentId Of(Guid value)
        {
            return new ClassStudentId(value);
        }
    }
}
