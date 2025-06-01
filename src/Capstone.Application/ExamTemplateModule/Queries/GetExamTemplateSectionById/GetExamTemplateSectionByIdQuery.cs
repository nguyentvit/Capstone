using Capstone.Domain.QuestionDomain.Common.Enums;

namespace Capstone.Application.ExamTemplateModule.Queries.GetExamTemplateSectionById
{
    public record GetExamTemplateSectionByIdQuery(Guid UserId, Guid SectionId) : IQuery<GetExamTemplateSectionByIdResult>;
    public record GetExamTemplateSectionByIdResult(Guid ChapterId, string ChapterName, List<GetExamTemplateSectionByIdDifficultyDto> DifficultyConfigs);
    public record GetExamTemplateSectionByIdDifficultyDto(QuestionDifficulty Difficulty, List<GetExamTemplateSectionByIdQuestionDto> QuestionTypeConfigs);
    public record GetExamTemplateSectionByIdQuestionDto(QuestionType Type, int NumberOfQuestions, double PointPerCorrect, double PointPerInCorrect);
}
