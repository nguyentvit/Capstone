namespace Capstone.Application.TeacherDomain.Queries.GetTeacherSubjectById
{
    public record GetTeacherSubjectByIdQuery(Guid UserId, Guid SubjectId) : IQuery<GetTeacherSubjectByIdResult>;
    public record GetTeacherSubjectByIdResult(Guid SubjectId, string SubjectName);
}
