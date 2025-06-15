using Capstone.Domain.ExamSessionModule.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;

namespace Capstone.Application.ExamSessionModule.Commands.MarkEssayQuestion
{
    public class MarkEssayQuestionHandler(IApplicationDbContext dbContext) : ICommandHandler<MarkEssayQuestionCommand, MarkEssayQuestionResult>
    {
        public async Task<MarkEssayQuestionResult> Handle(MarkEssayQuestionCommand command, CancellationToken cancellationToken)
        {
            var userId = UserId.Of(command.UserId);
            var participantId = ParticipantId.Of(command.ParticipantId);

            var esp = await dbContext.ExamSessions
                                          .SelectMany(es => es.Participants, (es, p) => new { es, p })
                                          .Where(t => t.es.UserId == userId && t.p.Id == participantId)
                                          .FirstOrDefaultAsync(cancellationToken);

            if (esp == null)
                throw new BadRequestException("Không tìm thấy");

            if (esp.es.IsDone.Value == true)
                throw new BadRequestException("Kíp thi này đã chốt điểm, nên không thể chấm điểm");

            var participantAnswer = esp.p.Answers.FirstOrDefault(a => a.QuestionId == QuestionId.Of(command.QuestionId));

            if (participantAnswer == null)
                throw new BadRequestException("Không tìm thấy câu hỏi");

            var question = await dbContext.Questions.AsNoTracking()
                                                    .Where(q => q.Id == participantAnswer.QuestionId)
                                                    .FirstOrDefaultAsync(cancellationToken);

            if (question == null)
                throw new QuestionNotFoundException(command.QuestionId);

            if (question.Type != QuestionType.EssayQuestion)
                throw new BadRequestException("Đây không phải câu hỏi tự luận");

            var pointPerCorrect = await dbContext.Exams.AsNoTracking()
                                                         .Where(e => e.Id == esp.es.ExamId)
                                                         .SelectMany(e => e.ExamVersions)
                                                         .SelectMany(ev => ev.Questions)
                                                         .Where(q => q.QuestionId == question.Id)
                                                         .Select(q => q.PointPerCorrect)
                                                         .FirstOrDefaultAsync(cancellationToken);

            if (command.Score > pointPerCorrect)
                throw new BadRequestException($"Giáo viên không thể chấm lớn hơn điểm hiện tại {pointPerCorrect}");

            esp.p.Grade(QuestionId.Of(command.QuestionId), command.Score);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new MarkEssayQuestionResult(true);
        }
    }
}
