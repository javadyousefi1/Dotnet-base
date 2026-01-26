using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

public sealed class UserRoleConfiguration : IEntityTypeConfiguration<UserRoleEntity>
{
    public void Configure(EntityTypeBuilder<UserRoleEntity> builder)
    {
        builder.ToTable("UserRoles");

        // Composite primary key
        builder.HasKey(ur => new { ur.UserId, ur.Role });

        builder.Property(ur => ur.UserId)
            .IsRequired();

        builder.Property(ur => ur.Role)
            .IsRequired()
            .HasConversion<int>();

        // Index for better query performance
        builder.HasIndex(ur => ur.UserId);
    }
}
