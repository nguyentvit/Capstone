namespace Capstone.Domain.ClassDomain.ValueObjects
{
    public record ClassId
    {
        public Guid Value { get; }
        private ClassId(Guid value) => Value = value;
        public static ClassId Of(Guid value)
        {
            return new ClassId(value);
        }
    }
}
