using Capstone.Domain.Common.Exceptions;
using Capstone.Domain.ExamSessionModule.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.ValueObjects;

namespace Capstone.Application.FreeParticipant.Commands.SubmitExamSession
{
    public class SubmitExamSessionHandler(IApplicationDbContext dbContext) : ICommandHandler<SubmitExamSessionCommand, SubmitExamSessionResult>
    {
        public async Task<SubmitExamSessionResult> Handle(SubmitExamSessionCommand command, CancellationToken cancellationToken)
        {
            var examSession = await dbContext.ExamSessions
                                             .SelectMany(e => e.Participants, (es, participants) => new
                                             {
                                                 ExamSession = es,
                                                 Participants = participants
                                             })
                                             .Where(t => t.Participants.Id == ParticipantId.Of(command.ParticipantId))
                                             .FirstOrDefaultAsync(cancellationToken);

            if (examSession == null)
                throw new BadRequestException("Thí sinh không có quyền tham gia vào bài thi này");

            if (examSession.Participants.IsDone.Value)
                throw new BadRequestException("Bài thi đã hoàn thành, không thể lưu bài");

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
                        Key = new { x.ChapterId, x.difficultyConfig.Difficulty, qtc.Type },
                        Value = new { qtc.PointPerCorrect, qtc.PointPerInCorrect }
                    })
                    .ToDictionary(x => x.Key, x => x.Value);

            double fullScore = examTemplate.ExamTemplateSection
                        .SelectMany(section => section.DifficultyConfigs)
                        .SelectMany(config => config.QuestionTypeConfigs)
                        .Sum(qtc => qtc.NumberOfQuestions * qtc.PointPerCorrect);

            double score = 0;

            var scores = new Dictionary<QuestionId, double>();

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

                scores[question.Id] = percentage * pointConfig.PointPerCorrect;
            }

            examSession.Participants.SubmitExam(scores);

            await dbContext.SaveChangesAsync();

            return new SubmitExamSessionResult(true);
        }
    }
}
