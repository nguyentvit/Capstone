namespace Capstone.Application.Common.Exceptions
{
    public class ExamTemplateSectionNotFoundException(Guid Id) : NotFoundException($"Mạch khung đề thi với Id: {Id} không tìm thấy")
    {
    }
}
