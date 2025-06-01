namespace Capstone.Application.ExamTemplateModule.Queries.GetExamTemplates
{
    public record GetExamTemplatesQuery(Guid UserId, PaginationRequest PaginationRequest) : IQuery<GetExamTemplatesResult>;
    public record GetExamTemplatesResult(PaginationResult<GetExamTemplatesDto> ExamTemplates);
    public record GetExamTemplatesDto(Guid Id, string Title, string Description, int Duration, Guid SubjectId, string SubjectName);
}
