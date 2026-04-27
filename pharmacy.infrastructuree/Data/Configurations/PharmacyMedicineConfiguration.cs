using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using pharmacy.domin.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pharmacy.infrastructuree.Data.Configurations
{
    public class PharmacyMedicineConfiguration : IEntityTypeConfiguration<PharmacyMedicine>
    {
        public void Configure(EntityTypeBuilder<PharmacyMedicine> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Price)
                .HasColumnType("decimal(18,2)");

            builder.Property(x => x.Stock)
                .IsRequired();

            builder.Property(x => x.IsAvailable)
                .IsRequired();

            // Relationship مع Pharmacy
            builder.HasOne(x => x.Pharmacy)
                .WithMany(x => x.PharmacyMedicines)
                .HasForeignKey(x => x.PharmacyId)
                .OnDelete(DeleteBehavior.Cascade);

            // Relationship مع Medicine
            builder.HasOne(x => x.Medicine)
                .WithMany(x => x.PharmacyMedicines)
                .HasForeignKey(x => x.MedicineId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

