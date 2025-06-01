using Microsoft.AspNetCore.Identity;
using System.Transactions;

namespace Capstone.Application.AdminDomain.Commands.AddStudent
{
    public class AddStudentHandler(IApplicationDbContext dbContext, UserManager<ApplicationUser> userManager) : ICommandHandler<AddStudentCommand, AddStudentResult>
    {
        public async Task<AddStudentResult> Handle(AddStudentCommand command, CancellationToken cancellationToken)
        {
            if (await userManager.FindByNameAsync(command.StudentId) != null)
                throw new StudentDuplicationException(command.StudentId);

            var student = AddStudentCommandToStudent(command);
            var studentAccount = new ApplicationUser
            {
                UserName = student.StudentId.Value,
                EmailConfirmed = true,
                UserId = student.Id.Value
            };
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            var result = await userManager.CreateAsync(studentAccount, studentAccount.UserName);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new StudentBadRequestException(errors);
            }


            dbContext.Students.Add(student);

            await userManager.AddToRoleAsync(studentAccount, RoleConstant.STUDENT);
            await dbContext.SaveChangesAsync(cancellationToken);

            scope.Complete();

            return new AddStudentResult(student.Id.Value);
        }
        private static Student AddStudentCommandToStudent(AddStudentCommand command)
        {
            return Student.Of(
                UserName.Of(command.UserName),
                (command.Email != null) ? Email.Of(command.Email) : null,
                (command.PhoneNumber != null) ? PhoneNumber.Of(command.PhoneNumber) : null,
                (command.Avatar != null) ? FileVO.Of(command.Avatar) : null,
                StudentId.Of(command.StudentId)
                );
        }
    }
}
