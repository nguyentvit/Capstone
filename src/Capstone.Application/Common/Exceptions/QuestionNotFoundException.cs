namespace Capstone.Application.Common.Exceptions
{
    public class QuestionNotFoundException(Guid Id) : NotFoundException($"Câu hỏi với Id: {Id} không tồn tại")
    {
    }
}
