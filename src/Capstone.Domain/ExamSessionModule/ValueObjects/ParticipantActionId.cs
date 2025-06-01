namespace Capstone.Domain.ExamSessionModule.ValueObjects
{
    public record ParticipantActionId
    {
        public Guid Value { get; }
        private ParticipantActionId(Guid value) => Value = value;
        public static ParticipantActionId Of(Guid value)
        {
            return new ParticipantActionId(value);
        }
    }
}
