using Capstone.Application.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Capstone.Application.AdminDomain.Commands.ResetPassword
{
    public class ResetPasswordHandler(UserManager<ApplicationUser> userManager, IApplicationDbContext dbContext) : ICommandHandler<ResetPasswordCommand, ResetPasswordResult>
    {
        public async Task<ResetPasswordResult> Handle(ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var userAccount = await dbContext.ApplicationUsers
                                              .FirstOrDefaultAsync(u => u.UserId == command.Id, cancellationToken);
            var user = await dbContext.AppUsers
                                       .FirstOrDefaultAsync(u => u.Id == UserId.Of(command.Id), cancellationToken);

            if (userAccount == null || user == null)
                throw new UserNotFoundException(command.Id);

            var role = await userManager.GetRolesAsync(userAccount);

            if (role.Count != 0 && role.Contains(RoleConstant.ADMIN))
                throw new UserBadRequestException("Bạn không thể thiết lập lại mật khẩu của Admin");

            var schoolId = CommonExtension.GetSchoolIdFromUser(user);

            var token = await userManager.GeneratePasswordResetTokenAsync(userAccount);

            var result = await userManager.ResetPasswordAsync(userAccount, token, schoolId);

            if (!result.Succeeded)
                throw new BadRequestException("Thiết lập mật khẩu thất bại");

            return new ResetPasswordResult(true);
        }
    }
}
