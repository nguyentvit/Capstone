using Capstone.Domain.UserAccess.ValueObjects;

namespace Capstone.Domain.UserAccess.Models;

public class User : Aggregate<UserId>
{
    public UserName UserName { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public PhoneNumber? Phone { get; private set; } = default!;
    public Date? DateOfBirth { get; private set; } = default!;
    public Image? Avatar { get; private set; } = default!;
    public Address? Address { get; private set; } = default!;
    public UserGender? Gender { get; private set; } = default!;
    private User() {}
    private User(UserId id, UserName userName, Email email) 
    {
        Id = id;
        UserName = userName;
        Email = email;
    }
    public static User Of(UserId id, UserName userName, Email email)
    {
        var user = new User(id, userName, email);

        return user;
    }
}