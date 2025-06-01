namespace Capstone.Application.UserAccess.Commands.ChangePassword
{
    public record ChangePasswordCommand(Guid Id, string Password, string ConfirmPassword, string OldPassword) : ICommand<ChangePasswordResult>;
    public record ChangePasswordResult(bool IsSuccess);
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {

        }
    }
}
