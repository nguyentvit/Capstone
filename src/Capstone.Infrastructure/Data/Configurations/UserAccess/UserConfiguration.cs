using Capstone.Domain.Common.Enums;
using Capstone.Domain.Common.ValueObjects;
using Capstone.Domain.UserAccess.Models;
using Capstone.Domain.UserAccess.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone.Infrastructure.Data.Configurations.UserAccess;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        ConfigureUserTable(builder);
    }
    private void ConfigureUserTable(EntityTypeBuilder<User> builder)
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

        builder.ComplexProperty(
                u => u.Email, emailBuilder =>
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
        u => u.DateOfBirth, dobBuilder =>
        {
            dobBuilder.Property(dob => dob.Value)
                .HasColumnName(nameof(User.DateOfBirth));

        });

        builder.OwnsOne(
        u => u.Avatar, avatarBuilder =>
        {
            avatarBuilder.Property(a => a.Url)
                .HasColumnName(nameof(Image.Url));

            avatarBuilder.Property(a => a.Format)
                .HasColumnName(nameof(Image.Format))
                .HasConversion(f => f.ToString(), dbFormat => (ImageFormat)Enum.Parse(typeof(ImageFormat), dbFormat));
        });


        builder.OwnsOne(
        u => u.Address, addressBuilder =>
        {
            addressBuilder.Property(a => a.Province)
                .HasColumnName(nameof(Address.Province))
                .HasMaxLength(50);

            addressBuilder.Property(a => a.Ward)
                .HasColumnName(nameof(Address.Ward))
                .HasMaxLength(50);

            addressBuilder.Property(a => a.District)
                .HasColumnName(nameof(Address.District))
                .HasMaxLength(50);

            addressBuilder.Property(a => a.Country)
                .HasColumnName(nameof(Address.Country))
                .HasDefaultValue("Việt Nam")
                .HasMaxLength(50);
        });


        builder.Property(u => u.Gender)
        .HasConversion(
            g => g.ToString(),
            dbGender => (UserGender)Enum.Parse(typeof(UserGender), dbGender!)
        );

    }
}