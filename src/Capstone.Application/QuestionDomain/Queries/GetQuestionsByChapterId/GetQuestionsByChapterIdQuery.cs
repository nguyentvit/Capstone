namespace Capstone.Application.QuestionDomain.Queries.GetQuestionsByChapterId
{
    public record GetQuestionsByChapterIdQuery(Guid UserId, string Role, Guid ChapterId, PaginationRequest PaginationRequest) : IQuery<GetQuestionsByChapterIdResult>;
    public record GetQuestionsByChapterIdResult(PaginationResult<QuestionBaseDto> Questions);
}
