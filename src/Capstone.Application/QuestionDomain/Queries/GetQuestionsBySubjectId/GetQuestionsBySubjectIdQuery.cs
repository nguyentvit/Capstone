namespace Capstone.Application.QuestionDomain.Queries.GetQuestionsBySubjectId
{
    public record GetQuestionsBySubjectIdQuery(Guid UserId, string Role, Guid SubjectId, PaginationRequest PaginationRequest) : IQuery<GetQuestionsBySubjectIdResult>;
    public record GetQuestionsBySubjectIdResult(PaginationResult<QuestionBaseDto> Questions);
}
