namespace Capstone.Application.ExamSessionModule.Commands.VerifyCode
{
    public record VerifyCodeCommand(Guid ExamSessionId, string Code) : ICommand<VerifyCodeResult>;
    public record VerifyCodeResult(bool IsSuccess);
}
