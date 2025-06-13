using Capstone.Domain.ExamSessionModule.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;

namespace Capstone.Application.FreeParticipant.Commands.AnswerMultiChoiceQuestion
{
    public class AnswerMultiChoiceQuestionHandler(IApplicationDbContext dbContext) : ICommandHandler<AnswerMultiChoiceQuestionCommand, AnswerMultiChoiceQuestionResult>
    {
        public async Task<AnswerMultiChoiceQuestionResult> Handle(AnswerMultiChoiceQuestionCommand command, CancellationToken cancellationToken)
        {
            var examSession = await dbContext.ExamSessions
                                             .SelectMany(es => es.Participants, (es, p) => new
                                             {
                                                 es,
                                                 p
                                             })
                                             .Where(t => t.p.Id == ParticipantId.Of(command.ParticipantId))
                                             .FirstOrDefaultAsync(cancellationToken);

            if (examSession == null)
                throw new AccessNotAllowException();

            if (examSession.p.IsDone.Value)
                throw new BadRequestException("Bài thi đã hoàn thành, không thể trả lời câu hỏi");

            var dateNow = DateTime.UtcNow;
            if (examSession.es.StartTime.Value > dateNow || examSession.es.EndTime.Value < dateNow)
                throw new BadRequestException("Không nằm trong thời gian thi");

            if (examSession.p.ExamVersionId == null)
                throw new BadRequestException("Bài thi chưa được bắt đầu");

            var examVersion = await dbContext.Exams
                                             .AsNoTracking()
                                             .SelectMany(e => e.ExamVersions)
                                             .Where(ev => ev.Id == examSession.p.ExamVersionId)
                                             .FirstOrDefaultAsync(cancellationToken);

            if (examVersion == null)
                throw new ExamVersionNotFoundException(examSession.p.ExamVersionId.Value);

            var question = examVersion.Questions.FirstOrDefault(q => q.QuestionId.Value == command.QuestionId);
            if (question == null)
                throw new BadRequestException("Không tìm thấy câu hỏi này");

            var isMultiChoiceQuestion = await dbContext.Questions
                                                       .AsNoTracking()
                                                       .Where(q => q.Id == question.QuestionId)
                                                       .Select(q => q.Type)
                                                       .FirstOrDefaultAsync(cancellationToken);

            if (isMultiChoiceQuestion != QuestionType.MultiChoiceQuestion)
                throw new BadRequestException("Đây không phải câu hỏi chọn nhiều");

            var answer = new MultiChoiceAnswer(command.Answer);

            examSession.p.SubmitAnswer(question.QuestionId, answer);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new AnswerMultiChoiceQuestionResult(true);
        }
    }
}
