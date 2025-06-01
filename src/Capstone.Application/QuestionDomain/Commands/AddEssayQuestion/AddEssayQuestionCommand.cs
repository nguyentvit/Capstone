namespace Capstone.Application.QuestionDomain.Commands.AddEssayQuestion
{
    public record AddEssayQuestionCommand(Guid UserId, Guid? ChapterId, string Title, string Content, int Difficulty, string Role) : ICommand<AddEssayQuestionResult>;
    public record AddEssayQuestionResult(Guid Id);
    public class AddEssayQuestionCommandValidator : AbstractValidator<AddEssayQuestionCommand>
    {
        public AddEssayQuestionCommandValidator() { }
    }
}
