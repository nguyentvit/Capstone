namespace Capstone.Application.AdminDomain.Queries.GetSystemSubjectById
{
    public record GetSystemSubjectByIdQuery(Guid Id) : IQuery<GetSystemSubjectByIdResult>;
    public record GetSystemSubjectByIdResult(Guid Id, string SubjectName);
}
