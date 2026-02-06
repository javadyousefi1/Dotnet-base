using Domain.Enums;

namespace Application.Users.Update;

public sealed record RoleMangementResponse(
    List<UserRole> roles
);