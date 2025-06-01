namespace Capstone.Application.ExamDomain.Commands.DownloadExamVersionPdf
{
    public record DownloadExamVersionPdfCommand(Guid UserId, Guid ExamVersionId) : ICommand<DownloadExamVersionPdfResult>;
    public record DownloadExamVersionPdfResult(byte[] Pdf);
}
