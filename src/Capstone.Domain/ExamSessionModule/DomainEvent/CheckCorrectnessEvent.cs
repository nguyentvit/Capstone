using Capstone.Domain.ExamSessionModule.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;

namespace Capstone.Domain.ExamSessionModule.DomainEvent
{
    public record CheckCorrectnessEvent(QuestionId QuestionId, AnswerRaw AnswerRaw);
}
