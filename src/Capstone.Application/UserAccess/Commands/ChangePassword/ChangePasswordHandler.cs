
namespace Capstone.Application.UserAccess.Commands.ChangePassword
{
    public class ChangePasswordHandler : ICommandHandler<ChangePasswordCommand, ChangePasswordResult>
    {
        public Task<ChangePasswordResult> Handle(ChangePasswordCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
