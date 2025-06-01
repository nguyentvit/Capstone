using Capstone.Domain.ClassDomain.Entities;
using Capstone.Domain.ClassDomain.ValueObjects;

namespace Capstone.Application.TeacherDomain.Commands.AddStudent
{
    public class AddStudentHandler(IApplicationDbContext dbContext) : ICommandHandler<AddStudentCommand, AddStudentResult>
    {
        public async Task<AddStudentResult> Handle(AddStudentCommand command, CancellationToken cancellationToken)
        {
            var classId = ClassId.Of(command.ClassId);
            var userId = UserId.Of(command.UserId);
            var studentId = UserId.Of(command.StudentId);

            var classExist = await dbContext.Classes
                                        .GroupJoin(
                                            dbContext.TeacherSubjects,
                                            c => c.SubjectId,
                                            s => s.Id,
                                            (classEntity, subjects) => new { classEntity, subjects })
                                        .SelectMany(
                                            x => x.subjects.DefaultIfEmpty(),
                                            (x, subject) => new
                                            {
                                                Class = x.classEntity,
                                                Subject = subject
                                            })
                                        .Where(x => x.Class.Id == classId && x.Subject != null && x.Subject.OwnerId == userId)
                                        .Select(x => x.Class)
                                        .FirstOrDefaultAsync(cancellationToken);

            if (classExist == null)
                throw new ClassNotFoundException(classId.Value);

            var student = await dbContext.Students
                                        .AsNoTracking()
                                        .Where(s => s.Id == studentId)
                                        .Select(s => new { s.StudentId })
                                        .FirstOrDefaultAsync(cancellationToken);

            if (student == null)
                throw new StudentNotFoundException(studentId.Value);

            if (classExist.Students.Any(x => x.StudentId == studentId))
                throw new TeacherBadRequestException($"Student với MSSV: {student.StudentId.Value} đã tồn tại trong danh sách của lớp");

            var classStudent = ClassStudent.Of(studentId);
            classExist.AddStudent(classStudent);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new AddStudentResult(true);
        }
    }
}
