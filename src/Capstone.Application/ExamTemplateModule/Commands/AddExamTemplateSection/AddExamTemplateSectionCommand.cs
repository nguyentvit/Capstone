namespace Capstone.Application.ExamTemplateModule.Commands.AddExamTemplateSection
{
    public record AddExamTemplateSectionQuestion(int NumberOfQuestions, double PointPerCorrect, double PointPerInConrrect, int QuestionType);
    public record AddExamTemplateSectionDifficulty(int QuestionDifficulty, List<AddExamTemplateSectionQuestion> QuestionTypeConfigs);
    public record AddExamTemplateSectionCommand(Guid UserId, string Role, Guid ExamTemplateId, Guid ChapterId, List<AddExamTemplateSectionDifficulty> DifficultyConfigs) : ICommand<AddExamTemplateSectionResult>;
    public record AddExamTemplateSectionResult(Guid Id);
    public class AddExamTemplateSectionCommandValidator : AbstractValidator<AddExamTemplateSectionCommand>
    {
        public AddExamTemplateSectionCommandValidator()
        {

        }
    }
}
