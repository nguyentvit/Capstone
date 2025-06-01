namespace Capstone.Application.Common.Exceptions
{
    public class ExamNotFoundException(Guid Id) : NotFoundException($"Gói đề thi với Id: {Id} không tồn tại")
    {
    }
}
