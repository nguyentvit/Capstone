using Capstone.Domain.ExamSessionModule.ValueObjects;

namespace Capstone.Application.ExamSessionModule.Commands.VerifyCode
{
    public class VerifyCodeHandler(IApplicationDbContext dbContext) : ICommandHandler<VerifyCodeCommand, VerifyCodeResult>
    {
        public async Task<VerifyCodeResult> Handle(VerifyCodeCommand command, CancellationToken cancellationToken)
        {
            var examSessionId = ExamSessionId.Of(command.ExamSessionId);
            var code = await dbContext.ExamSessions
                                             .Where(es => es.Id == examSessionId && es.IsCodeBased == IsCodeBased.Of(false))
                                             .Select(es => es.Code)
                                             .FirstOrDefaultAsync(cancellationToken);

            if (code == null)
                throw new ExamSessionNotFoundException(examSessionId.Value);

            if (code.Value != command.Code)
                throw new BadRequestException("Mã vào thi không hợp lệ");

            return new VerifyCodeResult(true);
        }
    }
}
