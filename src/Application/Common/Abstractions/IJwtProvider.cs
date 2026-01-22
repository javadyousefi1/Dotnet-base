using Application.Common.Models;
using Domain.Enums;

namespace Application.Common.Abstractions;

public interface IJwtProvider
{
    JwtToken Generate(Guid userId, IEnumerable<UserRole> roles);
}
