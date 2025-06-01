namespace Capstone.Application.TeacherDomain.Commands.AddStudent
{
    public record AddStudentCommand(Guid UserId, Guid ClassId, Guid StudentId) : ICommand<AddStudentResult>;
    public record AddStudentResult(bool IsSuccess);
}
