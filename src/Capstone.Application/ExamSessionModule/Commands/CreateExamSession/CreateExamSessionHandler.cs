using Capstone.Domain.ClassDomain.ValueObjects;
using Capstone.Domain.ExamModule.ValueObjects;
using Capstone.Domain.ExamSessionModule.Entities;
using Capstone.Domain.ExamSessionModule.Models;
using Capstone.Domain.ExamSessionModule.ValueObjects;

namespace Capstone.Application.ExamSessionModule.Commands.CreateExamSession
{
    public class CreateExamSessionHandler(IApplicationDbContext dbContext) : ICommandHandler<CreateExamSessionCommand, CreateExamSessionResult>
    {
        public async Task<CreateExamSessionResult> Handle(CreateExamSessionCommand command, CancellationToken cancellationToken)
        {
            var examId = ExamId.Of(command.ExamId);
            var exam = await dbContext.Exams
                                      .AsNoTracking()
                                      .Where(e => e.Id == examId)
                                      .Select(e => new { e.UserId.Value })
                                      .FirstOrDefaultAsync(cancellationToken);

            if (exam == null)
                throw new ExamNotFoundException(examId.Value);

            if (exam.Value != command.UserId)
                throw new BadRequestException("Bạn không có quyền truy cập vào gói đề thi này");

            var participants = new List<Participant>();

            if (command.ClassId != null)
            {
                var classId = ClassId.Of(command.ClassId.Value);
                var classQuery = await dbContext.Classes
                                  .AsNoTracking()
                                  .Where(c => c.Id == classId)
                                  .Join(
                                      dbContext.TeacherSubjects,
                                      c => c.SubjectId,
                                      s => s.Id,
                                      (classEntity, subjects) => new { classEntity, subjects })
                                  .Select(t => new {t.subjects.OwnerId, t.classEntity.Students})
                                  .FirstOrDefaultAsync(cancellationToken);

                if (classQuery == null)
                    throw new ClassNotFoundException(classId.Value);

                if (classQuery.OwnerId.Value != command.UserId)
                    throw new BadRequestException("Bạn không có quyền truy cập vào gói đề thi này");

                var userIds = classQuery.Students.Select(s => s.StudentId).ToList();

                var studentIds = await dbContext.Students
                                                .AsNoTracking()
                                                .Where(s => userIds.Contains(s.Id))
                                                .Select(s => s.StudentId)
                                                .ToListAsync(cancellationToken);

                foreach (var studentId in studentIds)
                {
                    participants.Add(Participant.OfStudent(studentId));
                }
            }

            var examSession = ExamSession.Of(
                ExamSessionName.Of(command.Name),
                Date.Of(command.StartTime),
                Date.Of(command.EndTime),
                ExamSessionDuration.Of(command.Duration),
                IsCodeBased.Of(command.IsCodeBased),
                ExamId.Of(command.ExamId),
                UserId.Of(command.UserId),
                (command.ClassId != null) ? ClassId.Of(command.ClassId.Value) : null,
                participants
                );

            dbContext.ExamSessions.Add(examSession);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new CreateExamSessionResult(examSession.Id.Value);
        }
    }
}
