namespace Capstone.Application.Common.Exceptions;
public class RescueNotFoundException : NotFoundException
{
    public RescueNotFoundException(Guid Id) : base("Rescue with Id", Id)
    {

    }
}