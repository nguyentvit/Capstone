namespace Capstone.Application.AdminDomain.Commands.ActiveUser
{
    public class ActiveUserHandler(IApplicationDbContext dbContext) : ICommandHandler<ActiveUserCommand, ActiveUserResult>
    {
        public async Task<ActiveUserResult> Handle(ActiveUserCommand command, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(command.Id);

            var user = await dbContext.AppUsers.FindAsync([userId], cancellationToken);
            var userAccount = await dbContext.ApplicationUsers.Where(ua => ua.UserId == userId.Value).FirstOrDefaultAsync(cancellationToken);

            if (user == null || userAccount == null)
                throw new UserNotFoundException(userId.Value);

            if (userAccount.IsActive || user.IsActive.Value)
                throw new UserBadRequestException("Tài khoản hoặc người dùng đã hoạt động");

            if (!userAccount.IsActive && !user.IsActive.Value)
            {
                userAccount.IsActive = true;
                userAccount.LockoutEnd = null;
                userAccount.AccessFailedCount = 0;

                user.ActiveUser();

                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return new ActiveUserResult(true);
        }
    }
}
