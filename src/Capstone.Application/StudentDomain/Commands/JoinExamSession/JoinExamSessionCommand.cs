namespace Capstone.Application.StudentDomain.Commands.JoinExamSession
{
    public record JoinExamSessionCommand(Guid UserId, Guid ExamSessionId) : ICommand<JoinExamSessionResult>;
    public record JoinExamSessionResult(int Duration, string Code, string SubjectName, List<JoinExamSessionDto> Questions);
    public record JoinExamSessionDto(QuestionBaseDto Data, int Order, double PointPerCorrect, double PointPerInCorrect);
}
