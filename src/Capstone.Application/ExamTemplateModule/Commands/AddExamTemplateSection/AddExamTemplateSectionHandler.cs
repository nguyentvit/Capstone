using Capstone.Domain.ChapterDomain.ValueObjects;
using Capstone.Domain.ExamTemplateModule.Entities;
using Capstone.Domain.ExamTemplateModule.ValueObjects;
using Capstone.Domain.QuestionDomain.Common.Enums;

namespace Capstone.Application.ExamTemplateModule.Commands.AddExamTemplateSection
{
    public class AddExamTemplateSectionHandler(IApplicationDbContext dbContext) : ICommandHandler<AddExamTemplateSectionCommand, AddExamTemplateSectionResult>
    {
        public async Task<AddExamTemplateSectionResult> Handle(AddExamTemplateSectionCommand command, CancellationToken cancellationToken)
        {
            var examTemplateId = ExamTemplateId.Of(command.ExamTemplateId);
            var examTemplate = await dbContext.ExamTemplates
                                              .Where(e => e.Id == examTemplateId)
                                              .FirstOrDefaultAsync(cancellationToken);

            if (examTemplate == null)
                throw new ExamTemplateNotFoundException(examTemplateId.Value);

            var subject = await dbContext.Subjects
                                         .AsNoTracking()
                                         .Where(s => s.Id == examTemplate.SubjectId)
                                         .FirstOrDefaultAsync();

            if (subject == null)
                throw new SubjectNotFoundException(examTemplate.SubjectId.Value);

            var userId = UserId.Of(command.UserId);
            SubjectExtention.CheckRole(subject, userId, command.Role);

            var chapterId = ChapterId.Of(command.ChapterId);
            if (!subject.ChapterIds.Contains(chapterId))
                throw new BadRequestException($"Chương với Id: {chapterId.Value} không thuộc môn học");

            var examTemplateSection = AddExamTemplateSectionCommandToExamTemplateSection(command);
            examTemplate.AddExamTemplateSection(examTemplateSection);

            await dbContext.SaveChangesAsync(cancellationToken);

            return new AddExamTemplateSectionResult(examTemplateSection.Id.Value);
        }
        private static ExamTemplateSection AddExamTemplateSectionCommandToExamTemplateSection(AddExamTemplateSectionCommand command)
        {
            var difficultyConfigs = command.DifficultyConfigs.Select(dc =>
            {
                var questionTypeConfigs = dc.QuestionTypeConfigs
                                            .Select(qt => QuestionTypeConfig.Of((QuestionType)qt.QuestionType, qt.NumberOfQuestions, qt.PointPerCorrect, qt.PointPerInConrrect))
                                            .ToList();
                return DifficultySectionConfig.Of((QuestionDifficulty)dc.QuestionDifficulty, questionTypeConfigs);
            }).ToList();

            return ExamTemplateSection.Of(
                ChapterId.Of(command.ChapterId),
                difficultyConfigs
                );
        }
    }
}
