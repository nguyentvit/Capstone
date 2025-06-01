namespace Capstone.Domain.SubjectDomain.ValueObjects
{
    public record TeacherQuestionRequestId
    {
        public Guid Value { get; }
        private TeacherQuestionRequestId(Guid value) => Value = value;
        public static TeacherQuestionRequestId Of(Guid value)
        {
            return new TeacherQuestionRequestId(value);
        }
    }
}
