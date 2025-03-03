using Capstone.Domain.RescueTeam.Entities;
using Capstone.Domain.RescueTeam.ValueObjects;
using Capstone.Domain.UserAccess.ValueObjects;

namespace Capstone.Domain.RescueTeam.Models;
public class Rescue : Aggregate<RescueId>
{
    private readonly List<RescueMember> _rescueMembers = new();
    public IReadOnlyList<RescueMember> RescueMembers => _rescueMembers.AsReadOnly();
    public RescueName RescueName { get; private set; } = default!;
    public PhoneNumber Phone { get; private set; } = default!;
    public Image? Avatar { get; private set; } = default!;
    public Address Address { get; private set; } = default!;
    public Coordinates Coordinates { get; private set; } = default!;
    public UserId ManagerId { get; private set; } = default!; 
    private Rescue() {}
    private Rescue(RescueId id, RescueName rescueName, PhoneNumber phone, Image? avatar, Address address, Coordinates coordinates, UserId managerId, List<RescueMember> rescueMembers)
    {
        Id = id;
        RescueName = rescueName;
        Phone = phone;
        Address = address;
        Coordinates = coordinates;
        ManagerId = managerId;
        Avatar = avatar;
        _rescueMembers = rescueMembers ?? new List<RescueMember>();
    }
    private Rescue(RescueId id, RescueName rescueName, PhoneNumber phone, Address address, Coordinates coordinates, UserId managerId)
    {
        Id = id;
        RescueName = rescueName;
        Phone = phone;
        Address = address;
        Coordinates = coordinates;
        ManagerId = managerId;
    }
    public static Rescue Of(RescueName rescueName, PhoneNumber phone, Address address, Coordinates coordinates, UserId managerId)
    {
        var Rescue = new Rescue(RescueId.Of(Guid.NewGuid()), rescueName, phone, address, coordinates, managerId);

        return Rescue;
    }
    
}