namespace Capstone.Application.TeacherDomain.Queries.GetClassById
{
    public record GetClassByIdQuery(Guid UserId, Guid ClassId) : IQuery<GetClassByIdResult>;
    public record GetClassByIdResult(string ClassName);
}
