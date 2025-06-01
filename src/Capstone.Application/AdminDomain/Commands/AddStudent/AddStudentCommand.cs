namespace Capstone.Application.AdminDomain.Commands.AddStudent
{
    public record AddStudentCommand(string UserName, string StudentId, string? Email, string? PhoneNumber, IFormFile? Avatar) : ICommand<AddStudentResult>;
    public record AddStudentResult(Guid Id);
    public class AddStudentCommandValidator : AbstractValidator<AddStudentCommand>
    {
        public AddStudentCommandValidator()
        {

        }
    }
}
