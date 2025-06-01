namespace Capstone.Application.ExamDomain.Queries.GetExams
{
    public record GetExamsQuery(Guid UserId, PaginationRequest PaginationRequest) : IQuery<GetExamsResult>;
    public record GetExamsResult(PaginationResult<GetExamsDto> Exams);
    public record GetExamsDto(Guid Id, string Title, Guid ExamTemplateId, string ExamTemplateName, Guid SubjectId, string SubjectName, int Duration, int CountOfExam);
}
