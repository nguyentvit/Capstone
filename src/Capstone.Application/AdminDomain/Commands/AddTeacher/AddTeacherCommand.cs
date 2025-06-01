    namespace Capstone.Application.AdminDomain.Commands.AddTeacher
{
    public record AddTeacherCommand(string UserName, string TeacherId, string? Email, string? PhoneNumber, IFormFile? Avatar) : ICommand<AddTeacherResult>;
    public record AddTeacherResult(Guid Id);
    public class AddTeacherCommandValidator : AbstractValidator<AddTeacherCommand>
    {
        public AddTeacherCommandValidator()
        {

        }
    }
}
