using Domain.Enums;

namespace Domain.Entities;

public sealed class UserRoleEntity
{
    public Guid UserId { get; private set; }
    public User User { get; private set; } = null!;

    public UserRole Role { get; private set; }

    private UserRoleEntity(Guid userId, UserRole role)
    {
        UserId = userId;
        Role = role;
    }

    private UserRoleEntity()
    {
    }

    public static UserRoleEntity Create(Guid userId, UserRole role)
    {
        if (!Enum.IsDefined(typeof(UserRole), role))
        {
            throw new ArgumentException($"Invalid role value: {(int)role}. Role must be a valid UserRole enum member.", nameof(role));
        }

        return new UserRoleEntity(userId, role);
    }
}
