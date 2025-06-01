namespace Capstone.Application.Common.Exceptions
{
    public class AccessNotAllowException() : BadRequestException("Bạn không có quyền truy cập vào tài nguyen này")
    {
    }
}
