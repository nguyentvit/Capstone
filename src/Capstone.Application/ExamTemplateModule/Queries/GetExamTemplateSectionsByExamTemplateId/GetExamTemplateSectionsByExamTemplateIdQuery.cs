using Capstone.Domain.QuestionDomain.Common.Enums;

namespace Capstone.Application.ExamTemplateModule.Queries.GetExamTemplateSectionsByExamTemplateId
{
    public record GetExamTemplateSectionsByExamTemplateIdQuery(Guid UserId, Guid ExamTemplateId) : IQuery<GetExamTemplateSectionsByExamTemplateIdResult>;
    public record GetExamTemplateSectionsByExamTemplateIdResult(List<GetExamTemplateSectionsByExamTemplateIdSectionDto> ExamTemplateSections);
    public record GetExamTemplateSectionsByExamTemplateIdSectionDto(Guid Id, Guid ChapterId, string ChapterName, double PercentageOfQuestion, double PointRatio, List<GetExamTemplateSectionsByExamTemplateIdDifficultyDto> DifficultyConfigs);
    public record GetExamTemplateSectionsByExamTemplateIdDifficultyDto(QuestionDifficulty Difficulty, List<GetExamTemplateSectionsByExamTemplateIdQuestion> QuestionTypeConfigs);
    public record GetExamTemplateSectionsByExamTemplateIdQuestion(QuestionType Type, int NumberOfQuestions, double PointPerCorrect, double PointPerInCorrect, int TotalOfQuestions);
}
