namespace Capstone.Application.QuestionDomain.Commands.UpdateMatchingQuestion
{
    public record UpdateMatchingQuestionCommand(Guid UserId, Guid QuestionId, string Title, string Content, int Difficulty, Dictionary<string, string> MatchingPairs) : ICommand<UpdateMatchingQuestionResult>;
    public record UpdateMatchingQuestionResult(Guid Id);
    public class UpdateMatchingQuestionCommandValidator : AbstractValidator<UpdateMatchingQuestionCommand>
    {
        public UpdateMatchingQuestionCommandValidator()
        {

        }
    }
}
