namespace Capstone.Application.ExamDomain.Queries.GetExamVersionsByExamId
{
    public record GetExamVersionsByExamIdQuery(Guid UserId, Guid ExamId) : IQuery<GetExamVersionsByExamIdResult>;
    public record GetExamVersionsByExamIdResult(List<GetExamVersionsByExamIdDto> ExamVersions);
    public record GetExamVersionsByExamIdDto(Guid Id, string Code);
}
