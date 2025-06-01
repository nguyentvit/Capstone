using Capstone.Application.ExamDomain.Queries.GetExamVersionById;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using Capstone.Domain.QuestionDomain.Common.Enums;

namespace Capstone.Application.ExamDomain.Commands.DownloadExamVersionPdf
{
    public class DownloadExamVersionPdfHandler(IApplicationDbContext dbContext) : ICommandHandler<DownloadExamVersionPdfCommand, DownloadExamVersionPdfResult>
    {
        public async Task<DownloadExamVersionPdfResult> Handle(DownloadExamVersionPdfCommand command, CancellationToken cancellationToken)
        {
            var query = new GetExamVersionByIdQuery(command.UserId, command.ExamVersionId);
            var examVersion = await new GetExamVersionByIdHandler(dbContext).Handle(query, cancellationToken);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);

                    page.Header()
                        .Text($"ĐỀ THI - {examVersion.SubjectName.ToUpper()}")
                        .FontSize(18).Bold().AlignCenter();

                    page.Content().Element(c =>
                    {
                        c.Column(col =>
                        {
                            col.Item().Text($"Mã đề: {examVersion.Code}").FontSize(12).Italic();
                            col.Item().Text($"Thời gian: {examVersion.Duration} phút").FontSize(12);
                            col.Item().PaddingVertical(10);

                            foreach (var (q, index) in examVersion.Questions.Select((q, i) => (q, i + 1)))
                            {
                                col.Item().Text($"{index}. {q.Data.Title}").FontSize(11).Bold();

                                if (!string.IsNullOrWhiteSpace(q.Data.Content))
                                    col.Item().Text(q.Data.Content).FontSize(11);

                                col.Item().Text($"(Điểm đúng: {q.PointPerCorrect} | Sai: {q.PointPerInCorrect})")
                                       .FontSize(9).Italic();

                                // Single Choice
                                if (q.Data.Type == QuestionType.SingleChoiceQuestion.ToString() && q.Data.SingleChoiceQuestionDto is not null)
                                {
                                    foreach (var choice in q.Data.SingleChoiceQuestionDto.Choices)
                                    {
                                        col.Item().Text($"☐ {choice.Content}").FontSize(10);
                                    }
                                }

                                // True/False
                                else if (q.Data.Type == QuestionType.TrueFalseQuestion.ToString() && q.Data.TrueFalseQuestionDto is not null)
                                {
                                    col.Item().Text("☐ Đúng").FontSize(10);
                                    col.Item().Text("☐ Sai").FontSize(10);
                                }

                                // Multiple Choice
                                else if (q.Data.Type == QuestionType.MultiChoiceQuestion.ToString() && q.Data.MultiChoiceQuestionDto is not null)
                                {
                                    col.Item().Text("(*) Có thể chọn nhiều đáp án.").FontSize(9).Italic();
                                    foreach (var choice in q.Data.MultiChoiceQuestionDto.Choices)
                                    {
                                        col.Item().Text($"☐ {choice.Content}").FontSize(10);
                                    }
                                }

                                // Matching Pair
                                else if (q.Data.Type == QuestionType.MatchingPairQuestion.ToString() && q.Data.MatchingQuestionDto is not null)
                                {
                                    var pairs = q.Data.MatchingQuestionDto.MatchingPairs;
                                    col.Item().Text("Nối cột A với cột B:").FontSize(9).Italic();

                                    col.Item().Table(table =>
                                    {
                                        table.ColumnsDefinition(columns =>
                                        {
                                            columns.ConstantColumn(200);
                                            columns.ConstantColumn(30);
                                            columns.RelativeColumn();
                                        });

                                        foreach (var pair in pairs)
                                        {
                                            table.Cell().Text(pair.Left).FontSize(10);
                                            table.Cell().Text("→").AlignCenter().FontSize(10);
                                            table.Cell().Text(".................").FontSize(10); // Không in sẵn đáp án
                                        }
                                    });
                                }

                                col.Item().PaddingBottom(10);
                            }

                        });
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.Span("Trang ");
                            x.CurrentPageNumber();
                            x.Span(" / ");
                            x.TotalPages();
                        });
                });
            });

            return new DownloadExamVersionPdfResult(document.GeneratePdf());
        }
    }
}
