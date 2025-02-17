using LezzetKapinda.Models;
using LezzetKapinda.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace LezzetKapinda.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(u => u.Id);

        builder.OwnsOne(u => u.FullName, fullName =>
        {
            fullName.Property(u => u.FirstName)
                .HasMaxLength(50)
                .IsRequired();
            fullName.Property(u => u.LastName)
                .HasMaxLength(50)
                .IsRequired();
        });

        builder.OwnsOne(u => u.UserName, userName =>
        {
            userName.Property(u => u.Value)
                .HasColumnName("UserName")
                .HasMaxLength(50)
                .IsRequired();
        });
            
        builder.Property(u => u.Comment)
            .HasMaxLength(500);
        
        builder.Property(u => u.ImageUrl)
            .HasMaxLength(500);

        builder.OwnsOne(u => u.Email, email =>
        {
            email.Property(u => u.Value)
                .HasConversion(v => v.ToString(), v => new Email(v))
                .HasMaxLength(100)
                .IsRequired();
        });

        builder.Property(u => u.PasswordHash)
            .IsRequired();
        
        builder.Property(u => u.Role)
            .IsRequired();
        
        builder.Property(u => u.Rating)
            .HasDefaultValue(0);
        
        builder.Property(u => u.CreatedAt)
            .HasDefaultValueSql("NOW()")
            .ValueGeneratedOnAdd();
        
        builder.Property(u => u.UpdatedAt)
            .ValueGeneratedOnUpdate();

        builder.HasMany(u => u.Addresses)
            .WithOne(a => a.User)
            .HasForeignKey(a => a.UserId);

        builder.HasMany(u => u.Orders)
            .WithOne(o => o.User)
            .HasForeignKey(o => o.UserId);

        builder.ToTable("users");
    }
} 