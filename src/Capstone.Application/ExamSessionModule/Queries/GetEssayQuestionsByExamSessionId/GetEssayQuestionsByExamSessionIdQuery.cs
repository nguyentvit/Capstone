using Capstone.Domain.ExamSessionModule.Enums;

namespace Capstone.Application.ExamSessionModule.Queries.GetEssayQuestionsByExamSessionId
{
    public record GetEssayQuestionsByExamSessionIdQuery(Guid UserId, Guid ExamSessionId) : IQuery<GetEssayQuestionsByExamSessionIdResult>;
    public record GetEssayQuestionsByExamSessionIdResult(List<GetEssayQuestionsByExamSessionIdDto> EssayQuestions);
    public record GetEssayQuestionsByExamSessionIdDto(Guid ParticipantId, string? StudentId, string UserName, bool IsFree, List<GetEssayQuestionsByExamSessionIdAnswer> Answer);
    public record GetEssayQuestionsByExamSessionIdAnswer(Guid QuestionId, string QuestionTitle, string QuestionContent, GradingStatus GradingStatus, string Answer, double? Score);
}
