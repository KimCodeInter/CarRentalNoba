using CarRental.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CarRental.Infrastructure.Persistence.Configurations;

public class PricingRateConfiguration : IEntityTypeConfiguration<PricingRateEntity>
{
    public void Configure(EntityTypeBuilder<PricingRateEntity> builder)
    {
        builder.HasKey(p => p.Id);
        builder.Property(p => p.CarType).IsRequired();
        builder.Property(p => p.BaseDayRental).HasPrecision(18, 2).IsRequired();
        builder.Property(p => p.BaseKmPrice).HasPrecision(18, 2).IsRequired();
        builder.Property(p => p.EffectiveFrom).IsRequired();
        builder.HasIndex(p => new { p.CarType, p.EffectiveFrom });
    }
}
