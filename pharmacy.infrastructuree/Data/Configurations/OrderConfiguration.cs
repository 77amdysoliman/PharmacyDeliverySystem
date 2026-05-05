using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pharmacy.domin.Entites;

namespace pharmacy.infrastructuree.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.OrderDate)
                .IsRequired();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasConversion<string>();

            builder.Property(x => x.TotalPrice)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.DeliveryAddress)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(x => x.Notes)
                .HasMaxLength(1000);

            // ✅ UserId string مش Relationship
            builder.Property(x => x.UserId)
                .IsRequired()
                .HasMaxLength(450);

            // Relationship مع Pharmacy بس
            builder.HasOne(x => x.Pharmacy)
                .WithMany(x => x.Orders)
                .HasForeignKey(x => x.PharmacyId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}