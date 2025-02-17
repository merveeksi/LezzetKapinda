using LezzetKapinda.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LezzetKapinda.Configurations;

public class AddressConfiguration : IEntityTypeConfiguration<Address>
{
    public void Configure(EntityTypeBuilder<Address> builder)
    {
        builder.HasKey(a => a.Id);

        builder.Property(a => a.FullAddress)
            .IsRequired();

        builder.Property(a => a.City)
            .IsRequired();

        builder.Property(a => a.PostalCode)
            .IsRequired();

        builder.Property(a => a.PhoneNumber)
            .IsRequired();
        
        builder.ToTable("addresses");
    }
} 