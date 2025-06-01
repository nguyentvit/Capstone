namespace Capstone.Domain.ExamSessionModule.ValueObjects
{
    public record ExamSessionCode
    {
        public string Value { get; }
        private ExamSessionCode()
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();

            Value = new string(Enumerable.Range(0, 6)
                .Select(_ => chars[random.Next(chars.Length)])
                .ToArray());
        }
        private ExamSessionCode(string value) => Value = value;
        public static ExamSessionCode Of(string value)
        {
            return new ExamSessionCode(value);
        }
        public static ExamSessionCode Generate()
        {
            return new ExamSessionCode();
        }
    }
}
