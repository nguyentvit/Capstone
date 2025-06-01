using Capstone.Domain.ExamModule.Models;
using Capstone.Domain.ExamModule.ValueObjects;
using Capstone.Domain.ExamTemplateModule.ValueObjects;

namespace Capstone.Application.ExamDomain.Commands.CreateExam
{
    public class CreateExamHandler(IApplicationDbContext dbContext) : ICommandHandler<CreateExamCommand, CreateExamResult>
    {
        public async Task<CreateExamResult> Handle(CreateExamCommand command, CancellationToken cancellationToken)
        {
            var examTemplateId = ExamTemplateId.Of(command.ExamTemplateId);

            var examTemplate = await dbContext.ExamTemplates
                                              .AsNoTracking()
                                              .Join(
                                                dbContext.TeacherSubjects.AsNoTracking(),
                                                et => et.SubjectId,
                                                ts => ts.Id,
                                                (et, ts) => new
                                                {
                                                    et.SubjectId,
                                                    ts.OwnerId
                                                }
                                              )
                                              .FirstOrDefaultAsync(cancellationToken);

            if (examTemplate == null)
                throw new ExamTemplateNotFoundException(examTemplateId.Value);

            if (examTemplate.OwnerId.Value != command.UserId)
                throw new BadRequestException("Bạn không có quyền tương tác với khung đề này");

            var exam = Exam.Of(
                examTemplateId,
                ExamDuration.Of(command.Duration),
                ExamTitle.Of(command.Title),
                UserId.Of(command.UserId)
                );

            dbContext.Exams.Add(exam);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new CreateExamResult(exam.Id.Value);
        }
    }
}
