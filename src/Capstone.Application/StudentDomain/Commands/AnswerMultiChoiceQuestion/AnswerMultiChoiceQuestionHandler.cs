using Capstone.Domain.ExamSessionModule.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;

namespace Capstone.Application.StudentDomain.Commands.AnswerMultiChoiceQuestion
{
    public class AnswerMultiChoiceQuestionHandler(IApplicationDbContext dbContext) : ICommandHandler<AnswerMultiChoiceQuestionCommand, AnswerMultiChoiceQuestionResult>
    {
        public async Task<AnswerMultiChoiceQuestionResult> Handle(AnswerMultiChoiceQuestionCommand command, CancellationToken cancellationToken)
        {
            var studentId = await dbContext.Students
                                           .AsNoTracking()
                                           .Where(s => s.Id == UserId.Of(command.UserId))
                                           .Select(s => s.StudentId)
                                           .FirstOrDefaultAsync(cancellationToken);

            if (studentId == null)
                throw new StudentNotFoundException(command.UserId);

            var examSession = await dbContext.ExamSessions
                                             .SelectMany(e => e.Participants, (es, participants) => new
                                             {
                                                 ExamSession = es,
                                                 Participants = participants
                                             })
                                             .Where(t => t.Participants.StudentId != null && t.Participants.StudentId == studentId && t.ExamSession.Id == ExamSessionId.Of(command.ExamSessionId))
                                             .FirstOrDefaultAsync(cancellationToken);

            if (examSession == null)
                throw new BadRequestException("Thí sinh không có quyền tham gia vào bài thi này");

            if (examSession.Participants.IsDone.Value)
                throw new BadRequestException("Bài thi đã hoàn thành, không thể trả lời câu hỏi");

            var dateNow = DateTime.UtcNow;
            if (examSession.ExamSession.StartTime.Value > dateNow || examSession.ExamSession.EndTime.Value < dateNow)
                throw new BadRequestException("Không nằm trong thời gian thi");

            if (examSession.Participants.ExamVersionId == null)
                throw new BadRequestException("Bài thi chưa được bắt đầu");

            var examVersion = await dbContext.Exams
                                             .AsNoTracking()
                                             .SelectMany(e => e.ExamVersions)
                                             .Where(ev => ev.Id == examSession.Participants.ExamVersionId)
                                             .FirstOrDefaultAsync(cancellationToken);

            if (examVersion == null)
                throw new ExamVersionNotFoundException(examSession.Participants.ExamVersionId.Value);

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

            examSession.Participants.SubmitAnswer(question.QuestionId, answer);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new AnswerMultiChoiceQuestionResult(true);
        }
    }
}
