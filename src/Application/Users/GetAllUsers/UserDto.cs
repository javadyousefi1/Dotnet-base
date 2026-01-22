using Domain.Enums;

namespace Application.Users.GetAllUsers;

public sealed record UserDto(
    Guid Id,
    string PhoneNumber,
    string? Email,
    string Name,
    string Family,
    IEnumerable<UserRole> Roles,
    DateTime CreatedAt
);
