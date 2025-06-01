namespace Capstone.Application.ExamTemplateModule.Queries.GetExamTemplatesBySubjectId
{
    public record GetExamTemplatesBySubjectIdQuery(Guid UserId, Guid SubjectId, PaginationRequest PaginationRequest) : IQuery<GetExamTemplatesBySubjectIdResult>;
    public record GetExamTemplatesBySubjectIdResult(PaginationResult<GetExamTemplatesBySubjectIdDto> ExamTemplates);
    public record GetExamTemplatesBySubjectIdDto(Guid Id, string Title, string Description, int Duration);
}
