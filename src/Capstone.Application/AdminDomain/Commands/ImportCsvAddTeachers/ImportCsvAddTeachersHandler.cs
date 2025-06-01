using Capstone.Application.Interface.Services;
using Microsoft.AspNetCore.Identity;
using System.Transactions;

namespace Capstone.Application.AdminDomain.Commands.ImportCsvAddTeachers
{
    public class ImportCsvAddTeachersHandler(IApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, ICSVService csvService) : ICommandHandler<ImportCsvAddTeachersCommand, ImportCsvAddTeachersResult>
    {
        public async Task<ImportCsvAddTeachersResult> Handle(ImportCsvAddTeachersCommand command, CancellationToken cancellationToken)
        {
            var teachersFromCsvFile = csvService.ReadCSV<ImportCsvAddTeachersDto>(command.CsvFile.OpenReadStream());

            var errorsValidate = ValidateTeachersFromCsvFile(teachersFromCsvFile);

            if (errorsValidate.Count != 0)
                throw new Exception();
            

            var teacherIdsFromCsvFile = teachersFromCsvFile
                                            .Where(t => t.TeacherId != null)
                                            .Select(t => TeacherId.Of(t.TeacherId))
                                            .ToList();

            var teachersExistedList = await dbContext.Teachers
                                            .Where(t => teacherIdsFromCsvFile.Contains(t.TeacherId))
                                            .Select(t => t.TeacherId.Value)
                                            .ToListAsync(cancellationToken);

            if (teachersExistedList.Count != 0)
                throw new Exception();
            

            var teachers = new List<Teacher>();
            var teachersAccount = new List<ApplicationUser>();
            var result = new List<ImportCsvAddTeachersDto>();

            foreach (var teacherFromCsvFile in teachersFromCsvFile)
            {
                var teacher = TeacherFromCsvFileToTeacher(teacherFromCsvFile);
                var teacherAccount = new ApplicationUser
                {
                    UserName = teacher.TeacherId.Value,
                    EmailConfirmed = true,
                    UserId = teacher.Id.Value
                };

                teachers.Add(teacher);
                teachersAccount.Add(teacherAccount);
                result.Add(new ImportCsvAddTeachersDto
                {
                    Id = teacher.Id.Value,
                    Email = teacher.Email?.Value,
                    PhoneNumber = teacher.Phone?.Value,
                    TeacherId = teacher.TeacherId.Value,
                    UserName = teacher.UserName.Value
                });
            }

            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            foreach (var teacherAccount in teachersAccount)
            {
                var resultAddAccount = await userManager.CreateAsync(teacherAccount, teacherAccount.UserName!);
                if (resultAddAccount.Succeeded)
                {
                    await userManager.AddToRoleAsync(teacherAccount, RoleConstant.TEACHER);
                }
                else
                {
                    errorsValidate.Add(new ImportCsvAddTeachersError
                    {

                    });
                }
            }

            if (errorsValidate.Count != 0)
                throw new Exception();

            await dbContext.Teachers.AddRangeAsync(teachers, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            scope.Complete();

            return new ImportCsvAddTeachersResult(result);
        }
        private static List<ImportCsvAddTeachersError> ValidateTeachersFromCsvFile(IEnumerable<ImportCsvAddTeachersDto> teachers)
        {
            var errors = new List<ImportCsvAddTeachersError>();

            // Check Validate Field


            // Check Validate Duplicate TeacherId
            return errors;
        }
        
        private static Teacher TeacherFromCsvFileToTeacher(ImportCsvAddTeachersDto teacherFromCsvFile)
        {
            return Teacher.Of(
                UserName.Of(teacherFromCsvFile.UserName),
                (teacherFromCsvFile.Email != null) ? Email.Of(teacherFromCsvFile.Email) : null,
                (teacherFromCsvFile.PhoneNumber != null) ? PhoneNumber.Of(teacherFromCsvFile.PhoneNumber) : null,
                null,
                TeacherId.Of(teacherFromCsvFile.TeacherId)
                );
        }

    }
}
