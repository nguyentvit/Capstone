namespace Capstone.Application.ExamSessionModule.Commands.CreateExamSession
{
    public record CreateExamSessionCommand(Guid UserId, string Name, DateTime StartTime, DateTime EndTime, int Duration, bool IsCodeBased, Guid ExamId, Guid? ClassId) : ICommand<CreateExamSessionResult>;
    public record CreateExamSessionResult(Guid Id);
    public record CreateExamSessionStudentId(string Id);
    public class CreateExamSessionCommandValidator : AbstractValidator<CreateExamSessionCommand>
    {
        public CreateExamSessionCommandValidator()
        {

        }
    }
}
