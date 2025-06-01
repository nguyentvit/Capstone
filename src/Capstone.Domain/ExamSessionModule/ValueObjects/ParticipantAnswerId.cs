namespace Capstone.Domain.ExamSessionModule.ValueObjects
{
    public record ParticipantAnswerId
    {
        public Guid Value { get; }
        private ParticipantAnswerId(Guid value) => Value = value;
        public static ParticipantAnswerId Of(Guid value)
        {
            return new ParticipantAnswerId(value);
        }
    }
}
