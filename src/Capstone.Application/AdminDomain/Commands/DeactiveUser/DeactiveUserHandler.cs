using Microsoft.AspNetCore.Identity;

namespace Capstone.Application.AdminDomain.Commands.DeactiveUser
{
    public class DeactiveUserHandler(IApplicationDbContext dbContext, UserManager<ApplicationUser> userManager) : ICommandHandler<DeactiveUserCommand, DeactiveUserResult>
    {
        public async Task<DeactiveUserResult> Handle(DeactiveUserCommand command, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(command.Id);

            var user = await dbContext.AppUsers.FindAsync([userId], cancellationToken);
            var userAccount = await dbContext.ApplicationUsers.Where(ua => ua.UserId == userId.Value).FirstOrDefaultAsync(cancellationToken);

            if (user == null || userAccount == null)
                throw new UserNotFoundException(userId.Value);


            if (!userAccount.IsActive || !user.IsActive.Value)
                throw new UserBadRequestException("Tài khoản hoặc người dùng đã dừng hoạt động");

            var roleAccount = await userManager.GetRolesAsync(userAccount);
            if (roleAccount.Count != 0 && roleAccount.Contains(RoleConstant.ADMIN))
                throw new UserBadRequestException("Bạn không thể dừng hoạt động với Admin");

            if (userAccount.IsActive && user.IsActive.Value)
            {
                userAccount.IsActive = false;
                user.DeactiveUser();

                await dbContext.SaveChangesAsync(cancellationToken);
            }

            return new DeactiveUserResult(true);

        }
    }
}
