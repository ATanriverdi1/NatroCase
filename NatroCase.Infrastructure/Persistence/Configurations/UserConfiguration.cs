using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NatroCase.Domain.User;

namespace NatroCase.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<UserAggregate>
{
    public void Configure(EntityTypeBuilder<UserAggregate> builder)
    {
        builder.ToTable("users");
        builder.Property(p => p.Email).HasColumnName("email");
        builder.Property(p => p.Name).HasColumnName("name");
        builder.Property(p => p.Password).HasColumnName("password");
        builder.Property(p => p.Favorites).HasColumnName("favorites").HasColumnType("jsonb");
        
        builder.HasIndex(p => p.Email).IsUnique();
    }
}