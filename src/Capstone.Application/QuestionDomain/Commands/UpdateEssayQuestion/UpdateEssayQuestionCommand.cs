namespace Capstone.Application.QuestionDomain.Commands.UpdateEssayQuestion
{
    public record UpdateEssayQuestionCommand(Guid UserId, Guid QuestionId, string Title, string Content, int Difficulty) : ICommand<UpdateEssayQuestionResult>;
    public record UpdateEssayQuestionResult(Guid Id);
}
