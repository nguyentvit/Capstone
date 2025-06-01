namespace Capstone.Application.QuestionDomain.Commands.AddMultiChoiceQuestion
{
    public record AddMultiChoiceQuestionCommand(Guid UserId, Guid? ChapterId, string Title, string Content, int Difficulty, string Role, Dictionary<string, bool> Choices) : ICommand<AddMultiChoiceQuestionResult>;
    public record AddMultiChoiceQuestionResult(Guid Id);
    public class AddMultiChoiceQuestionCommandValidator : AbstractValidator<AddMultiChoiceQuestionCommand>
    {
        public AddMultiChoiceQuestionCommandValidator()
        {

        }
    }
}
