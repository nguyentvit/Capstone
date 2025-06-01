namespace Capstone.Application.ExamDomain.Queries.GetExamsBySubjectId
{
    public record GetExamsBySubjectIdQuery(Guid UserId, Guid SubjectId, PaginationRequest PaginationRequest) : IQuery<GetExamsBySubjectIdResult>;
    public record GetExamsBySubjectIdResult(PaginationResult<GetExamsBySubjectIdDto> Exams);
    public record GetExamsBySubjectIdDto(Guid Id, string Title, string ExamTemplateName, int Duration, int CountOfExam);
}
