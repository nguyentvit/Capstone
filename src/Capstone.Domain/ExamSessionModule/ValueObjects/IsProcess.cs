namespace Capstone.Domain.ExamSessionModule.ValueObjects
{
    public record IsProcess
    {
        public bool Value { get; private set; }
        private IsProcess(bool value) => Value = value;
        public static IsProcess Of(bool value)
        {
            return new IsProcess(value);
        }
    }
}
