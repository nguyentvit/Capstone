using Capstone.Domain.Common.Enums;
using Capstone.Domain.Common.ValueObjects;
using Capstone.Domain.UserAccess.Models;
using Capstone.Domain.UserAccess.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone.Infrastructure.Data.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUserTable(builder);
    }
    private static void ConfigureUserTable(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.Property(u => u.Id)
            .HasConversion(
                userId => userId.Value,
                dbId => UserId.Of(dbId)
            );

        builder.ComplexProperty(
                u => u.UserName, nameBuilder =>
                {
                    nameBuilder.Property(n => n.Value)
                        .HasColumnName(nameof(User.UserName))
                        .HasMaxLength(100);
                });

        builder.OwnsOne(u => u.Email, emailBuilder =>
        {
            emailBuilder.Property(e => e.Value)
                .HasColumnName(nameof(User.Email))
                .HasMaxLength(50);
        });

        builder.OwnsOne(
        u => u.Phone, phoneBuilder =>
        {
            phoneBuilder.Property(p => p.Value)
                .HasColumnName(nameof(User.Phone))
                .HasMaxLength(20);
        });

        builder.OwnsOne(
        u => u.Avatar, avatarBuilder =>
        {
            avatarBuilder.Property(a => a.Url)
                .HasColumnName(nameof(Image.Url));
        });

        builder.Property(u => u.IsActive)
            .HasConversion(
                isActive => isActive.Value,
                dbIsActive => IsActive.Of(dbIsActive)
            );

    }
}