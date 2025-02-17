using LezzetKapinda.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LezzetKapinda.Configurations;

public class ContactMessageConfiguration : IEntityTypeConfiguration<ContactMessage>
{
    public void Configure(EntityTypeBuilder<ContactMessage> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(c => c.Name)
            .IsRequired();

        builder.Property(c => c.Email)
            .IsRequired();

        builder.Property(c => c.Subject)
            .IsRequired();

        builder.Property(c => c.Message)
            .IsRequired();

        builder.ToTable("contact_messages");
    }
}