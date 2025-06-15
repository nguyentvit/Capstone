namespace Capstone.Application.ExamSessionModule.Commands.ExportCsvResult
{
    public record ExportCsvResultCommand(Guid UserId, Guid ExamSessionId) : ICommand<ExportCsvResultResult>;
    public record ExportCsvResultResult(byte[] Pdf);
}
