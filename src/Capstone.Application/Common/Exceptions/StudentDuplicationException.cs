namespace Capstone.Application.Common.Exceptions
{
    public class StudentDuplicationException(string StudentId) : BadRequestException($"Sinh viên với MSSV: {StudentId} đã tồn tại")
    {
    }
}
