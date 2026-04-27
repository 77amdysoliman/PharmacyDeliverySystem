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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.FullName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(200);
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Phone)
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(x => x.Address)
                .HasMaxLength(500);

            builder.Property(x => x.Latitude)
                .HasColumnType("decimal(9,6)");

            builder.Property(x => x.Longitude)
                .HasColumnType("decimal(9,6)");

            builder.Property(x => x.CreatedAt)
                .IsRequired();

            // Unique Email
            builder.HasIndex(x => x.Email)
                .IsUnique();
        }
    }
}
