using Capstone.Domain.Common.Enums;
using Capstone.Domain.Common.ValueObjects;
using Capstone.Domain.RescueTeam.Entities;
using Capstone.Domain.RescueTeam.Enums;
using Capstone.Domain.RescueTeam.Models;
using Capstone.Domain.RescueTeam.ValueObjects;
using Capstone.Domain.UserAccess.ValueObjects;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Capstone.Infrastructure.Data.Configurations.RescueTeam;

public class RescueConfiguration : IEntityTypeConfiguration<Rescue>
{
    public void Configure(EntityTypeBuilder<Rescue> builder)
    {
        ConfigureRescue(builder);
        ConfigureMemberIds(builder);
    }
    private void ConfigureRescue(EntityTypeBuilder<Rescue> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(
                RescueId => RescueId.Value,
                dbId => RescueId.Of(dbId)
            );

        builder.ComplexProperty(
            s => s.RescueName, nameBuilder =>
            {
                nameBuilder.Property(n => n.Value)
                    .HasColumnName(nameof(Rescue.RescueName))
                    .HasMaxLength(100);
            }
        );

        builder.ComplexProperty(
            s => s.Phone, phoneBuilder =>
            {
                phoneBuilder.Property(p => p.Value)
                    .HasColumnName(nameof(Rescue.Phone))
                    .HasMaxLength(20);
            }
        );

        builder.OwnsOne(
            s => s.Avatar, avatarBuilder =>
            {
                avatarBuilder.Property(a => a.Url)
                    .HasColumnName(nameof(Image.Url));

                avatarBuilder.Property(a => a.Format)
                    .HasColumnName(nameof(Image.Format))
                    .HasConversion(f => f.ToString(), dbFormat => (ImageFormat)Enum.Parse(typeof(ImageFormat), dbFormat));
            }
        );

        builder.ComplexProperty(
            s => s.Address, addressBuilder =>
            {
                addressBuilder.Property(a => a.Province)
                    .HasColumnName(nameof(Address.Province))
                    .HasMaxLength(50);

                addressBuilder.Property(a => a.Ward)
                    .HasColumnName(nameof(Address.Ward))
                    .HasMaxLength(50);

                addressBuilder.Property(a => a.District)
                    .HasColumnName(nameof(Address.District))
                    .HasMaxLength(50); ;

                addressBuilder.Property(a => a.Country)
                    .HasColumnName(nameof(Address.Country))
                    .HasDefaultValue("Việt Nam")
                    .HasMaxLength(50);
            }
        );

        builder.Property(s => s.ManagerId)
            .HasConversion(
                id => id.Value,
                dbId => UserId.Of(dbId)
            );

        builder.ComplexProperty(s => s.Coordinates, coordinatesBuilder =>
        {
            coordinatesBuilder.Property(c => c.Latitude)
                .HasColumnName(nameof(Coordinates.Latitude));

            coordinatesBuilder.Property(c => c.Longitude)
                .HasColumnName(nameof(Coordinates.Longitude));
        });
    }
    private void ConfigureMemberIds(EntityTypeBuilder<Rescue> builder)
    {
        builder.OwnsMany(u => u.RescueMembers, rms =>
        {
            rms.ToTable("RescueMembers");
            rms.WithOwner().HasForeignKey("RescueId");
            rms.HasKey("Id", "RescueId");

            rms.Property(rm => rm.Id)
                .HasColumnName("RescueMemberId")
                .ValueGeneratedNever()
                .HasConversion(
                    id => id.Value,
                    dbId => RescueMemberId.Of(dbId)
                );

            rms.Property(rm => rm.RescueMemberName)
                .HasColumnName(nameof(RescueMember.RescueMemberName))
                .HasConversion(
                    name => name.Value,
                    dbName => RescueMemberName.Of(dbName)
                );

            rms.Property(rm => rm.Phone)
                .HasColumnName(nameof(RescueMember.Phone))
                .HasConversion(
                    phone => phone.Value,
                    dbPhone => PhoneNumber.Of(dbPhone)
                );

            rms.OwnsOne(rm => rm.Address, addressBuilder =>
            {
                addressBuilder.Property(a => a.Province)
                    .HasColumnName(nameof(Address.Province))
                    .HasMaxLength(50);

                addressBuilder.Property(a => a.Ward)
                    .HasColumnName(nameof(Address.Ward))
                    .HasMaxLength(50);

                addressBuilder.Property(a => a.District)
                    .HasColumnName(nameof(Address.District))
                    .HasMaxLength(50); ;

                addressBuilder.Property(a => a.Country)
                    .HasColumnName(nameof(Address.Country))
                    .HasDefaultValue("Việt Nam")
                    .HasMaxLength(50);
            });

            rms.Property(rm => rm.Introduction)
                .HasColumnName(nameof(RescueMember.Introduction))
                .HasConversion(
                    content => content.Value,
                    dbContent => Content.Of(dbContent)
                );

            rms.OwnsOne(rm => rm.DateOfBirth, dobBuilder => 
            {
                dobBuilder.Property(dob => dob.Value)
                    .HasColumnName(nameof(RescueMember.DateOfBirth));
            });

            rms.Property(rm => rm.Email)
                .HasColumnName(nameof(RescueMember.Email))
                .HasConversion(
                    email => email.Value,
                    dbEmail => Email.Of(dbEmail)
                );

            rms.Property(rm => rm.Passport)
                .HasColumnName(nameof(RescueMember.Passport))
                .HasConversion(
                    passport => passport.Value,
                    dbPassport => Passport.Of(dbPassport)
                );

            rms.Property(rm => rm.Gender)
                .HasConversion(
                    gender => gender.ToString(),
                    dbGender => (UserGender)Enum.Parse(typeof(UserGender), dbGender)
                );

            rms.Property(rm => rm.Status)
                .HasConversion(
                    status => status.ToString(),
                    dbStatus => (RescueMemberStatus)Enum.Parse(typeof(RescueMemberStatus), dbStatus)
                );

            rms.Property(rm => rm.AvailableStatus)
                .HasConversion(
                    availableStatus => availableStatus.ToString(),
                    dbAvailableStatus => (RescueMemberAvailableStatus)Enum.Parse(typeof(RescueMemberAvailableStatus), dbAvailableStatus)
                );

            rms.Property(rm => rm.Role)
                .HasConversion(
                    role => role.ToString(),
                    dbRole => (RescueMemberRole)Enum.Parse(typeof(RescueMemberRole), dbRole)
                );


        });

        builder.Metadata.FindNavigation(nameof(Rescue.RescueMembers))!.SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}
