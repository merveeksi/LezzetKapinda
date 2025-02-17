using LezzetKapinda.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LezzetKapinda.Configurations;

public class TestimonialConfiguration : IEntityTypeConfiguration<Testimonial>
{
    public void Configure(EntityTypeBuilder<Testimonial> builder)
    {
        builder.HasKey(t => t.Id);
        builder.Property(t => t.Id).ValueGeneratedOnAdd();

        builder.Property(t => t.ClientName)
            .IsRequired();

        builder.Property(t => t.Profession)
            .IsRequired();

        builder.Property(t => t.Comment)
            .IsRequired();

        builder.Property(t => t.ImageUrl)
            .IsRequired();

        builder.Property(t => t.Rating)
            .IsRequired();

        builder.ToTable("testimonials");
    }
} 