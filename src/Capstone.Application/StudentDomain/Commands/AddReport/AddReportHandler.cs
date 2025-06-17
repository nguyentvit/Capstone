using Capstone.Domain.ExamSessionModule.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;

namespace Capstone.Application.StudentDomain.Commands.AddReport
{
    public class AddReportHandler(IApplicationDbContext dbContext) : ICommandHandler<AddReportCommand, AddReportResult>
    {
        public async Task<AddReportResult> Handle(AddReportCommand command, CancellationToken cancellationToken)
        {
            var examSessionId = ExamSessionId.Of(command.ExamSessionId);
            var examSession = await dbContext.ExamSessions.Where(es => es.Id == examSessionId)
                                                          .FirstOrDefaultAsync(cancellationToken);

            if (examSession == null)
                throw new ExamSessionNotFoundException(examSessionId.Value);

            var studentId = await dbContext.Students.AsNoTracking()
                                                    .Where(s => s.Id == UserId.Of(command.UserId))
                                                    .Select(s => s.StudentId)
                                                    .FirstOrDefaultAsync(cancellationToken);

            if (studentId == null)
                throw new StudentNotFoundException(command.UserId);

            var participant = examSession.Participants.FirstOrDefault(p => p.StudentId != null && p.StudentId == studentId);

            if (participant == null)
                throw new BadRequestException("Bạn không nằm trong kíp thi này");

            if (examSession.IsDone.Value || !examSession.IsClosePoint.Value)
                throw new BadRequestException("Bạn chỉ có thể phúc khảo trong thời gian phúc khảo");

            if (!participant.IsDone.Value)
                throw new BadRequestException("Bạn chỉ có thể phúc khảo khi bài thi đã hoàn thành");

            var questionId = QuestionId.Of(command.QuestionId);

            var answer = participant.Answers.FirstOrDefault(a => a.QuestionId == questionId);

            if (answer == null)
                throw new BadRequestException("Bạn không trả lời câu hỏi này nên không thể phúc khảo");

            answer.AddReport();

            await dbContext.SaveChangesAsync(cancellationToken);

            return new AddReportResult(true);
        }
    }
}
