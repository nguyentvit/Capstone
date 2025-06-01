namespace Capstone.Domain.ChapterDomain.ValueObjects
{
    public record ChapterOrder
    {
        public int Value { get; }
        private ChapterOrder(int value) => Value = value;
        public static ChapterOrder Of(int value)
        {
            return new ChapterOrder(value);
        }
    }
}
