namespace Capstone.Application.ExamSessionModule.Commands.JoinExamSessionFree
{
    public record JoinExamSessionFreeCommand(Guid ExamSessionId, string Code, string FullName, string Email) : ICommand<JoinExamSessionFreeResult>;
    public record JoinExamSessionFreeResult(Guid ParticipantId, int Duration, string Code, string SubjectName, List<JoinExamSessionFreeDto> Questions);
    public record JoinExamSessionFreeDto(QuestionBaseDto Data, int Order, double PointPerCorrect, double PointPerInCorrect);
}
