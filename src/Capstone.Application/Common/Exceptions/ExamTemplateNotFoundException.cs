namespace Capstone.Application.Common.Exceptions
{
    public class ExamTemplateNotFoundException(Guid Id) : NotFoundException($"Khung đề thi với Id: {Id} không tồn tại")
    {
    }
}
