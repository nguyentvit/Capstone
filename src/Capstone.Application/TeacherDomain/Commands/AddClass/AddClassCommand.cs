namespace Capstone.Application.TeacherDomain.Commands.AddClass
{
    public record AddClassCommand(Guid UserId, Guid SubjectId, string ClassName) : ICommand<AddClassResult>;
    public record AddClassResult(Guid Id);
}
