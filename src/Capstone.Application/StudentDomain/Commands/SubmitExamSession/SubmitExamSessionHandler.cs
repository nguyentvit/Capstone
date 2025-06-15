using Capstone.Domain.Common.Exceptions;
using Capstone.Domain.ExamSessionModule.ValueObjects;
using Capstone.Domain.ExamTemplateModule.Models;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;

namespace Capstone.Application.StudentDomain.Commands.SubmitExamSession
{
    public class SubmitExamSessionHandler(IApplicationDbContext dbContext) : ICommandHandler<SubmitExamSessionCommand, SubmitExamSessionResult>
    {
        public async Task<SubmitExamSessionResult> Handle(SubmitExamSessionCommand command, CancellationToken cancellationToken)
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
                throw new BadRequestException("Bài thi đã hoàn thành, không thể lưu bài");

            var dateNow = DateTime.UtcNow;
            //if (examSession.ExamSession.StartTime.Value > dateNow || examSession.ExamSession.EndTime.Value < dateNow)
            //    throw new BadRequestException("Không nằm trong thời gian thi");

            if (examSession.Participants.ExamVersionId == null)
                throw new BadRequestException("Bài thi chưa được bắt đầu");

            var questionIds = examSession.Participants.Answers.Select(a => a.QuestionId).ToList();
            var questions = await dbContext.Questions
                                           .Where(q => questionIds.Any(id => id == q.Id))
                                           .ToListAsync(cancellationToken);

            var examTemplateId = await dbContext.Exams
                                        .AsNoTracking()
                                        .Where(e => e.Id == examSession.ExamSession.ExamId)
                                        .Select(e => e.ExamTemplateId)
                                        .FirstOrDefaultAsync(cancellationToken);

            if (examTemplateId == null)
                throw new ExamNotFoundException(examSession.ExamSession.ExamId.Value);

            var examTemplate = await dbContext.ExamTemplates
                                              .AsNoTracking()
                                              .Where(e => e.Id == examTemplateId)
                                              .FirstOrDefaultAsync(cancellationToken);

            if (examTemplate == null)
                throw new ExamTemplateNotFoundException(examTemplateId.Value);

            var pointDictionary = examTemplate.ExamTemplateSection
                    .SelectMany(section => section.DifficultyConfigs, (section, difficultyConfig) => new
                    {
                        section.ChapterId,
                        difficultyConfig
                    })
                    .SelectMany(x => x.difficultyConfig.QuestionTypeConfigs, (x, qtc) => new
                    {
                        Key = new {  x.ChapterId,  x.difficultyConfig.Difficulty, qtc.Type },
                        Value = new { qtc.PointPerCorrect, qtc.PointPerInCorrect }
                    })
                    .ToDictionary(x => x.Key, x => x.Value);

            double fullScore = examTemplate.ExamTemplateSection
                        .SelectMany(section => section.DifficultyConfigs)
                        .SelectMany(config => config.QuestionTypeConfigs)
                        .Sum(qtc => qtc.NumberOfQuestions * qtc.PointPerCorrect);

            double score = 0;

            foreach (var answer in examSession.Participants.Answers)
            {
                var question = questions.FirstOrDefault(q => q.Id == answer.QuestionId);
                if (question == null)
                    throw new QuestionNotFoundException(answer.QuestionId.Value);

                if (question.ChapterId == null)
                    throw new BadRequestException("ChapterId không thể null");

                var percentage = QuestionExtension.CalculateCorrectnessPercentage(question, answer.AnswerRaw.Value);

                var key = new
                {
                    question.ChapterId,
                    question.Difficulty,
                    question.Type
                };

                if (!pointDictionary.TryGetValue(key, out var pointConfig))
                    throw new DomainException($"Không tìm thấy cấu hình điểm cho câu hỏi {question.Id}");

                score += percentage * pointConfig.PointPerCorrect;
            }
            var scores = new Dictionary<QuestionId, double>();
            examSession.Participants.SubmitExam(scores);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new SubmitExamSessionResult(score, fullScore);
        }
    }
}
