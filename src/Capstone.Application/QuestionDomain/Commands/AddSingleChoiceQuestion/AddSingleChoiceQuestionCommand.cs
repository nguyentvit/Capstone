namespace Capstone.Application.QuestionDomain.Commands.AddSingleChoiceQuestion
{
    public record AddSingleChoiceQuestionCommand(Guid UserId, Guid? ChapterId, string Title, string Content, int Difficulty, string Role, List<string> Choices, int CorrectAnswerIndex) : ICommand<AddSingleChoiceQuestionResult>;
    public record AddSingleChoiceQuestionResult(Guid Id);
    public class AddSingleChoiceQuestionCommandValidator : AbstractValidator<AddSingleChoiceQuestionCommand>
    {
        public AddSingleChoiceQuestionCommandValidator()
        {

        }
    }
}
