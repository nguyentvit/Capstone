using Capstone.Domain.ExamSessionModule.Enums;

namespace Capstone.Application.ExamSessionModule.Queries.GetEssayQuestionsByExamSessionId
{
    public record GetEssayQuestionsByExamSessionIdQuery(Guid UserId, Guid ExamSessionId, PaginationRequest PaginationRequest) : IQuery<GetEssayQuestionsByExamSessionIdResult>;
    public record GetEssayQuestionsByExamSessionIdResult(PaginationResult<GetEssayQuestionsByExamSessionIdDto> EssayQuestions);
    public record GetEssayQuestionsByExamSessionIdDto(Guid ParticipantId, string? StudentId, string UserName, bool IsFree, Guid QuestionId, string QuestionTitle, string QuestionContent, GradingStatus GradingStatus, string Answer, double? Score);
}
