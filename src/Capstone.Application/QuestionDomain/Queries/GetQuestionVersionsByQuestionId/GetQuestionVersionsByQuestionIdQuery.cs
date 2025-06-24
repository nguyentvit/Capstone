namespace Capstone.Application.QuestionDomain.Queries.GetQuestionVersionsByQuestionId
{
    public record GetQuestionVersionsByQuestionIdQuery(Guid UserId, Guid QuestionId) : IQuery<GetQuestionVersionsByQuestionIdResult>;
    public record GetQuestionVersionsByQuestionIdResult(List<GetQuestionVersionsByQuestionIdDto> Questions);
    public record GetQuestionVersionsByQuestionIdDto(QuestionBaseDto Question);
}
