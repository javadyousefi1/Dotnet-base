using Domain.Enums;
using SharedKernel;

namespace Domain.Entities;

public sealed class User : BaseEntity
{
    public string PhoneNumber { get; private set; }
    public string? Email { get; private set; }
    public string Name { get; private set; }
    public string Family { get; private set; }

    private readonly List<UserRoleEntity> _userRoles = new();
    public IReadOnlyCollection<UserRoleEntity> UserRoles => _userRoles.AsReadOnly();

    public IEnumerable<UserRole> Roles => _userRoles.Select(ur => ur.Role);

    private User(Guid id, string phoneNumber, string name, string family)
        : base(id)
    {
        PhoneNumber = phoneNumber;
        Name = name;
        Family = family;
    }

    private User() : base()
    {
        PhoneNumber = string.Empty;
        Name = string.Empty;
        Family = string.Empty;
    }

    public static User Create(string phoneNumber, string name, string family)
    {
        var user = new User(
            Guid.NewGuid(),
            phoneNumber,
            name,
            family);

        user.AddRole(UserRole.User);
        return user;
    }

    public void UpdateProfile(string name, string family, string? email)
    {
        Name = name;
        Family = family;
        Email = email;
    }

    public void AddRole(UserRole role)
    {
        if (!_userRoles.Any(ur => ur.Role == role))
        {
            _userRoles.Add(UserRoleEntity.Create(Id, role));
        }
    }

    public void RemoveRole(UserRole role)
    {
        var userRole = _userRoles.FirstOrDefault(ur => ur.Role == role);
        if (userRole != null)
        {
            _userRoles.Remove(userRole);
        }
    }
}
