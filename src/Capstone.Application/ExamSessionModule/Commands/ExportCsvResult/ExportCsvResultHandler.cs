using Capstone.Application.ExamSessionModule.Queries.GetParticipantsResult;
using QuestPDF.Fluent;
using QuestPDF.Helpers;

namespace Capstone.Application.ExamSessionModule.Commands.ExportCsvResult
{
    public class ExportCsvResultHandler(IApplicationDbContext dbContext) : ICommandHandler<ExportCsvResultCommand, ExportCsvResultResult>
    {
        public async Task<ExportCsvResultResult> Handle(ExportCsvResultCommand command, CancellationToken cancellationToken)
        {
            var query = new GetParticipantsResultQuery(command.UserId, command.ExamSessionId);
            var result = await new GetParticipantsResultHandler(dbContext).Handle(query, cancellationToken);

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Size(PageSizes.A4);

                    page.Header()
                        .Text("KẾT QUẢ KỲ THI")
                        .FontSize(18).Bold().AlignCenter();

                    page.Content().Element(e =>
                    {
                        e.Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(40);
                                columns.RelativeColumn(2);
                                columns.RelativeColumn(3);
                                columns.RelativeColumn();
                                columns.RelativeColumn();
                            });

                            // Header
                            table.Header(header =>
                            {
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("STT").FontSize(11).Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Mã SV").FontSize(11).Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Tên người dùng").FontSize(11).Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Điểm").FontSize(11).Bold();
                                header.Cell().Background(Colors.Grey.Lighten2).Padding(5).Text("Hình thức").FontSize(11).Bold();
                            });

                            // Data rows
                            int index = 1;
                            foreach (var dto in result.Result)
                            {
                                table.Cell().Padding(5).Text(index++.ToString()).FontSize(10);
                                table.Cell().Padding(5).Text(dto.StudentId ?? "—").FontSize(10);
                                table.Cell().Padding(5).Text(dto.UserName).FontSize(10);
                                table.Cell().Padding(5).Text($"{dto.Score:0.##} / {dto.TotalScore:0.##}").FontSize(10);
                                table.Cell().Padding(5).Text(dto.IsFree ? "Tự do" : "Chính quy").FontSize(10);
                            }
                        });
                    });

                    page.Footer().Column(column =>
                    {
                        // Chỉ hiển thị chữ ký ở trang cuối cùng
                        column.Item().ShowOnce().AlignRight().Text(text =>
                        {
                            text.Span("Giáo viên phụ trách").FontSize(12).Bold();
                            text.Line("");
                            text.Line("");
                            text.Line(""); // cách 3 dòng
                            text.Span(result.TeacherName).FontSize(12);
                        });

                        // Đánh số trang luôn ở cuối
                        column.Item().AlignCenter().Text(x =>
                        {
                            x.Span("Trang ");
                            x.CurrentPageNumber();
                            x.Span(" / ");
                            x.TotalPages();
                        });
                    });
                });
            });

            return new ExportCsvResultResult(document.GeneratePdf());
        }
    }
}
