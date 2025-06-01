namespace Capstone.Domain.ChapterDomain.ValueObjects
{
    public record ChapterId
    {
        public Guid Value { get; }
        private ChapterId(Guid value) => Value = value;
        public static ChapterId Of(Guid value)
        {
            return new ChapterId(value);
        }
    }
}
