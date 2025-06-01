using Capstone.Domain.UserAccess.Events;
using Capstone.Domain.UserAccess.ValueObjects;

namespace Capstone.Domain.UserAccess.Models;

public class User : Aggregate<UserId>
{
    public UserName UserName { get; private set; } = default!;
    public Email? Email { get; private set; } = default!;
    public PhoneNumber? Phone { get; private set; } = default!;
    public Image? Avatar { get; private set; } = default!;
    public IsActive IsActive { get; private set; } = IsActive.Of(true);
    protected User() {}
    protected User(UserId id, UserName userName) 
    {
        Id = id;
        UserName = userName;
    }
    protected User(UserId id, UserName userName, Email? email, PhoneNumber? phone, FileVO? avatar)
    {
        Id = id;

        UserName = userName;

        if (email != null) Email = email;

        if (phone != null) Phone = phone;

        if (avatar != null)
            AddDomainEvent(new UploadAvatarUserEvent(this, avatar));
    }
    public static User Of(UserId id, UserName userName)
    {
        var user = new User(id, userName);

        return user;
    }

    public void UpdateAvatar(Image avatar)
    {
        Avatar = avatar;
    }
    
    public void UpdateUser(UserName? userName, Email? email, PhoneNumber? phone, FileVO? avatar)
    {
        if (userName != null)
            UserName = userName;

        if (email != null)
            Email = email;

        if (phone != null)
            Phone = phone;

        if (avatar != null)
            AddDomainEvent(new UploadAvatarUserEvent(this, avatar));
    }

    public void ActiveUser()
    {
        IsActive = IsActive.Of(true);
    }

    public void DeactiveUser()
    {
        IsActive = IsActive.Of(false);
    }
}