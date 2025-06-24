namespace Capstone.Application.ExamDomain.Commands.QuestionStatistics
{
    public record QuestionStatisticsCommand(Guid UserId, Guid ExamId, int Count, int OrderQuestion, bool IsAnswerShuffled) : ICommand<QuestionStatisticsResult>;
    public record QuestionStatisticsResult(bool IsSuccess);
}
