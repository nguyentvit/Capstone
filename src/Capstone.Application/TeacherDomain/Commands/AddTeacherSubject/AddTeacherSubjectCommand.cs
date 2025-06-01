namespace Capstone.Application.TeacherDomain.Commands.AddTeacherSubject
{
    public record AddTeacherSubjectCommand(Guid UserId, string SubjectName) : ICommand<AddTeacherSubjectResult>;
    public record AddTeacherSubjectResult(Guid Id);
}
