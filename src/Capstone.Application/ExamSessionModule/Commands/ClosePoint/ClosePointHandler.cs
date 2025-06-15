namespace Capstone.Application.ExamSessionModule.Commands.ClosePoint
{
    public class ClosePointHandler(IApplicationDbContext dbContext) : ICommandHandler<ClosePointCommand, CLosePointResult>
    {
        public async Task<CLosePointResult> Handle(ClosePointCommand command, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
