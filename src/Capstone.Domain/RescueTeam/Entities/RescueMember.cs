using Capstone.Domain.RescueTeam.Enums;
using Capstone.Domain.RescueTeam.ValueObjects;

namespace Capstone.Domain.RescueTeam.Entities;
public class RescueMember : Entity<RescueMemberId>
{
    public RescueMemberName RescueMemberName { get; private set; } = default!;
    public PhoneNumber Phone { get; private set; } = default!;
    public Address Address { get; private set; } = default!;
    public Content Introduction { get; private set; } = default!;
    public Date DateOfBirth { get; private set; } = default!;
    public Email Email { get; private set; } = default!;
    public Passport Passport { get; private set; } = default!;
    public UserGender Gender { get; private set; } = default!;
    public RescueMemberStatus Status { get; private set; } = default!;
    public RescueMemberAvailableStatus AvailableStatus { get; private set; } = default!;
    public RescueMemberRole Role { get; private set; } = default!;
    private RescueMember() { }
    private RescueMember(RescueMemberId id, RescueMemberName rescueMemberName, PhoneNumber phone, Address address, Content introduction, Date dateOfBirth, Email email, Passport passport, UserGender gender, RescueMemberStatus status, RescueMemberAvailableStatus availableStatus, RescueMemberRole role)
    {
        Id = id;
        RescueMemberName = rescueMemberName;
        Phone = phone;
        Address = address;
        Introduction = introduction;
        DateOfBirth = dateOfBirth;
        Email = email;
        Passport = passport;
        Gender = gender;
        Status = status;
        AvailableStatus = availableStatus;
        Role = role;
    }
    public static RescueMember Of(RescueMemberName rescueMemberName, PhoneNumber phone, Address address, Content introduction, Date dateOfBirth, Email email, Passport passport, UserGender gender, RescueMemberRole role)
    {
        var rescueMember = new RescueMember(
            RescueMemberId.Of(Guid.NewGuid()),
            rescueMemberName,
            phone,
            address,
            introduction,
            dateOfBirth,
            email,
            passport,
            gender,
            RescueMemberStatus.Pending,
            RescueMemberAvailableStatus.Unavailable,
            role
        );

        return rescueMember;
    }

}