using CarRental.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.Infrastructure.Persistence.Configurations;

public class VehicleConfiguration : IEntityTypeConfiguration<VehicleEntity>
{
    private const int RegistrationNumberMaxLength = 20;

    public void Configure(EntityTypeBuilder<VehicleEntity> builder)
    {
        builder.HasKey(v => v.Id);

        builder.Property(v => v.RegistrationNumber)
            .IsRequired()
            .HasMaxLength(RegistrationNumberMaxLength);

        builder.HasIndex(v => v.RegistrationNumber)
            .IsUnique();

        builder.Property(v => v.CarType)
            .IsRequired();
    }
}
