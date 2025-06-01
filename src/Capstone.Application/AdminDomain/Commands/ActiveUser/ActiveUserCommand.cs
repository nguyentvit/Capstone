namespace Capstone.Application.AdminDomain.Commands.ActiveUser
{
    public record ActiveUserCommand(Guid Id) : ICommand<ActiveUserResult>;
    public record ActiveUserResult(bool IsSuccess);
}
