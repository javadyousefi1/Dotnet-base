using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Abstractions;

public interface IAppDbContext
{
    DbSet<User> Users { get; }
    DbSet<UserRoleEntity> UserRoles { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
