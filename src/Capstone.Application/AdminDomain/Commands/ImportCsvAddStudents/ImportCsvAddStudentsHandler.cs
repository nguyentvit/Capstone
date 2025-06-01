using Capstone.Application.Interface.Services;
using Microsoft.AspNetCore.Identity;
using System.Transactions;

namespace Capstone.Application.AdminDomain.Commands.ImportCsvAddStudents
{
    public class ImportCsvAddStudentsHandler(IApplicationDbContext dbContext, UserManager<ApplicationUser> userManager, ICSVService csvService) : ICommandHandler<ImportCsvAddStudentsCommand, ImportCsvAddStudentsResult>
    {
        public async Task<ImportCsvAddStudentsResult> Handle(ImportCsvAddStudentsCommand command, CancellationToken cancellationToken)
        {
            var studentsFromCsvFile = csvService.ReadCSV<ImportCsvAddStudentsDto>(command.CsvFile.OpenReadStream());

            var errorsValidate = ValidateStudentsFromCsvFile(studentsFromCsvFile);

            if (errorsValidate.Count != 0)
                throw new Exception();

            var studentIdsFromCsvFile = studentsFromCsvFile
                                            .Where(s => s.StudentId != null)
                                            .Select(s => StudentId.Of(s.StudentId))
                                            .ToList();

            var studentsExistedList = await dbContext.Students
                                            .Where(s => studentIdsFromCsvFile.Contains(s.StudentId))
                                            .Select(t => t.StudentId.Value)
                                            .ToListAsync(cancellationToken);

            if (studentIdsFromCsvFile.Count != 0)
                throw new Exception();

            var students = new List<Student>();
            var studentsAccount = new List<ApplicationUser>();
            var result = new List<ImportCsvAddStudentsDto>();

            foreach (var studentFromCsvFile in studentsFromCsvFile)
            {
                var student = StudentFromCsvFileToStudent(studentFromCsvFile);
                var studentAccount = new ApplicationUser
                {
                    UserName = student.StudentId.Value,
                    EmailConfirmed = true,
                    UserId = student.Id.Value
                };

                students.Add(student);
                studentsAccount.Add(studentAccount);
                result.Add(new ImportCsvAddStudentsDto
                {
                    Id = student.Id.Value,
                    Email = student.Email?.Value,
                    PhoneNumber = student.Phone?.Value,
                    StudentId = student.StudentId.Value,
                    UserName = student.UserName.Value
                });
            }
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            foreach (var studentAccount in studentsAccount)
            {
                var resultAddAccount = await userManager.CreateAsync(studentAccount, studentAccount.UserName!);
                if (resultAddAccount.Succeeded)
                {
                    await userManager.AddToRoleAsync(studentAccount, RoleConstant.STUDENT);
                }
                else
                {
                    errorsValidate.Add(new ImportCsvAddStudentsError
                    {

                    });
                }
            }

            if (errorsValidate.Count != 0)
                throw new Exception();

            await dbContext.Students.AddRangeAsync(students, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            scope.Complete();

            return new ImportCsvAddStudentsResult(result);

        }
        private static List<ImportCsvAddStudentsError> ValidateStudentsFromCsvFile(IEnumerable<ImportCsvAddStudentsDto> students)
        {
            var errors = new List<ImportCsvAddStudentsError>();

            return errors;
        }
        private static Student StudentFromCsvFileToStudent(ImportCsvAddStudentsDto studentFromCsvFile)
        {
            return Student.Of(
                UserName.Of(studentFromCsvFile.UserName),
                (studentFromCsvFile.Email != null) ? Email.Of(studentFromCsvFile.Email) : null,
                (studentFromCsvFile.PhoneNumber != null) ? PhoneNumber.Of(studentFromCsvFile.PhoneNumber) : null,
                null,
                StudentId.Of(studentFromCsvFile.StudentId));
        }
    }
}
