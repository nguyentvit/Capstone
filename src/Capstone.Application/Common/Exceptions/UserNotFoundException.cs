namespace Capstone.Application.Common.Exceptions;
public class UserNotFoundException(Guid Id) : NotFoundException("Người dùng với Id: ", Id)
{
}