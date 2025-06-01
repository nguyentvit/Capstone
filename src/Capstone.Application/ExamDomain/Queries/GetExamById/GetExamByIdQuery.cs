namespace Capstone.Application.ExamDomain.Queries.GetExamById
{
    public record GetExamByIdQuery(Guid UserId, Guid ExamId) : IQuery<GetExamByIdResult>;
    public record GetExamByIdResult(string Title, int Duration);
}
