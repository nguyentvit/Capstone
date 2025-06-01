namespace Capstone.Application.AdminDomain.Commands.ResetPassword
{
    public record ResetPasswordCommand(Guid Id) : ICommand<ResetPasswordResult>;
    public record ResetPasswordResult(bool IsSuccess);
}
