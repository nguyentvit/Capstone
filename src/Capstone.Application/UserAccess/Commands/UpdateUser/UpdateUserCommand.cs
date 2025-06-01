namespace Capstone.Application.UserAccess.Commands.UpdateUser
{
    public record UpdateUserCommand(Guid Id, string? UserName, string? Email, string? PhoneNumber, IFormFile? Avatar) : ICommand<UpdateUserResult>;
    public record UpdateUserResult(bool IsSuccess);
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        public UpdateUserCommandValidator()
        {

        }
    }
}
