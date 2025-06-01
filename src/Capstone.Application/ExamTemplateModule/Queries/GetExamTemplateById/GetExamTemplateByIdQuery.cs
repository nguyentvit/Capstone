namespace Capstone.Application.ExamTemplateModule.Queries.GetExamTemplateById
{
    public record GetExamTemplateByIdQuery(Guid UserId, Guid ExamTemplateId) : IQuery<GetExamTemplateByIdResult>;
    public record GetExamTemplateByIdResult(string Title, string Description, int Duration, Guid SubjectId, string SubjectName);
}
