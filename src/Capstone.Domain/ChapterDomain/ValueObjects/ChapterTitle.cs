namespace Capstone.Domain.ChapterDomain.ValueObjects
{
    public record ChapterTitle
    {
        public string Value { get; }
        private ChapterTitle(string value) => Value = value;
        public static ChapterTitle Of(string value)
        {
            return new ChapterTitle(value);
        }
    }
}
