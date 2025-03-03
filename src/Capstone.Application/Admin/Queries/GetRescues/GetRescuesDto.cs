namespace Capstone.Application.Admin.Queries.GetRescues;
public record GetRescuesDto(Guid Id, string RescueName, string Phone, GetRescuesDtoImage? Avatar, GetRescuesDtoAddress Address, GetRescuesDtoCoordinates Coordinates, GetRescuesDtoManager Manager);
public record GetRescuesDtoImage(string Url, string Format);
public record GetRescuesDtoAddress(string District, string Ward, string Province, string Country);
public record GetRescuesDtoCoordinates(double Latitude, double Longitude);
public record GetRescuesDtoManager(Guid Id, string Name, GetRescuesDtoImage? Avatar);