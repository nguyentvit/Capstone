namespace Capstone.Application.QuestionDomain.Queries.GetQuestionById
{
    public record GetQuestionByIdQuery(Guid UserId, Guid QuestionId) : IQuery<GetQuestionByIdResult>;
    public record GetQuestionByIdResult(QuestionBaseDto Question);
}
