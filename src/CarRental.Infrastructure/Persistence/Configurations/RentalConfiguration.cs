using CarRental.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.Infrastructure.Persistence.Configurations;

public class RentalConfiguration : IEntityTypeConfiguration<RentalEntity>
{
    private const int BookingNumberMaxLength = 50;
    private const int RegistrationNumberMaxLength = 20;
    private const int SocialSecurityNumberMaxLength = 20;
    private const int PricePrecision = 18;
    private const int PriceScale = 2;

    public void Configure(EntityTypeBuilder<RentalEntity> builder)
    {
        builder.HasKey(r => r.Id);

        builder.Property(r => r.BookingNumber)
            .IsRequired()
            .HasMaxLength(BookingNumberMaxLength);

        builder.HasIndex(r => r.BookingNumber)
            .IsUnique();

        builder.Property(r => r.VehicleId)
            .IsRequired();

        builder.Property(r => r.RegistrationNumber)
            .IsRequired()
            .HasMaxLength(RegistrationNumberMaxLength);

        builder.Property(r => r.CarType)
            .IsRequired();

        builder.Property(r => r.CustomerSocialSecurityNumber)
            .IsRequired()
            .HasMaxLength(SocialSecurityNumberMaxLength);

        builder.Property(r => r.BaseDayRental)
            .IsRequired()
            .HasPrecision(PricePrecision, PriceScale);

        builder.Property(r => r.BaseKmPrice)
            .IsRequired()
            .HasPrecision(PricePrecision, PriceScale);

        builder.Property(r => r.TotalPrice)
            .HasPrecision(PricePrecision, PriceScale);

        builder.HasOne<VehicleEntity>()
            .WithMany()
            .HasForeignKey(r => r.VehicleId);
    }
}
