namespace Capstone.Application.AdminDomain.Commands.AddSystemSubject
{
    public record AddSystemSubjectCommand(string SubjectName) : ICommand<AddSystemSubjectResult>;
    public record AddSystemSubjectResult(Guid Id);
}
