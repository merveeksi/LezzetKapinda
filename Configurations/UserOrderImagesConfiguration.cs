using LezzetKapinda.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LezzetKapinda.Configurations;

public class UserOrderImagesConfiguration : IEntityTypeConfiguration<UserOrderImages>
{
    public void Configure(EntityTypeBuilder<UserOrderImages> builder)
    {
        builder.HasKey(ui => ui.UsersImageUrl);
        
        builder.Property(ui => ui.Id)
            .ValueGeneratedOnAdd();
        
        builder.Property(ui => ui.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(ui => ui.Description)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(ui => ui.UsersImageUrl)
            .IsRequired();

        builder.ToTable("user_order_images");
    }
}

public class UserOrderImages
{
}