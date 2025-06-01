using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.SubjectDomain.Enums;
using Capstone.Domain.SubjectDomain.ValueObjects;
using Capstone.Domain.TeacherDomain.ValueObjects;

namespace Capstone.Domain.SubjectDomain.Entities
{
    public class TeacherQuestionRequest : Entity<TeacherQuestionRequestId>
    {
        public QuestionId QuestionId { get; private set; } = default!;
        public TeacherId TeacherId { get; private set; } = default!;
        public RequestStatus Status { get; private set; } = default!;
    }
}
