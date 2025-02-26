namespace Capstone.Application.Identity.Exceptions;
public class IdentityBadRequestException : BadRequestException
{
    public IdentityBadRequestException(string message) : base(message)
    {
        
    }
}