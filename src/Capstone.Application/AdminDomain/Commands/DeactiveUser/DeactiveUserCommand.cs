namespace Capstone.Application.AdminDomain.Commands.DeactiveUser
{
    public record DeactiveUserCommand(Guid Id) : ICommand<DeactiveUserResult>;
    public record DeactiveUserResult(bool IsSuccess);
}
