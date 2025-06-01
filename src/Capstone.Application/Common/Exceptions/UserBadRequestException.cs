namespace Capstone.Application.Common.Exceptions
{
    public class UserBadRequestException(string message) : BadRequestException(message)
    {
    }
}
