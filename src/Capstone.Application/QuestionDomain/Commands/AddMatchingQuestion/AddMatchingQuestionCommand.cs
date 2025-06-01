namespace Capstone.Application.QuestionDomain.Commands.AddMatchingQuestion
{
    public record AddMatchingQuestionCommand(Guid UserId, Guid? ChapterId, string Title, string Content, int Difficulty, string Role, Dictionary<string, string> MatchingPairs) : ICommand<AddMatchingQuestionResult>;
    public record AddMatchingQuestionResult(Guid Id);
    public class AddMatchingQuestionCommandValidator : AbstractValidator<AddMatchingQuestionCommand>
    {
        public AddMatchingQuestionCommandValidator()
        {

        }
    }
}
