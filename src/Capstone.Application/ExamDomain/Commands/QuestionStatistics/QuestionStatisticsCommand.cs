namespace Capstone.Application.ExamDomain.Commands.QuestionStatistics
{
    public record QuestionStatisticsCommand() : ICommand<QuestionStatisticsResult>;
    public record QuestionStatisticsResult(bool IsSuccess);
}
