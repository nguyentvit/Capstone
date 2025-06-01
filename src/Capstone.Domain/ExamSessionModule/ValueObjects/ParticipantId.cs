namespace Capstone.Domain.ExamSessionModule.ValueObjects
{
    public record ParticipantId
    {
        public Guid Value { get; }
        private ParticipantId(Guid value) => Value = value;
        public static ParticipantId Of(Guid value)
        {
            return new ParticipantId(value);
        }
    }
}
