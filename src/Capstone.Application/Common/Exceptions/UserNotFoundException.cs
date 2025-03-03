namespace Capstone.Application.Common.Exceptions;
public class UserNotFoundException : NotFoundException
{
    public UserNotFoundException(Guid Id) : base("User with Id", Id)
    {

    }
}