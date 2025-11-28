using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Domain.Configurations
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> builder)
        {
            builder.ToTable("authors");

            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id).HasColumnName("id");

            builder.Property(a => a.Name)
                   .HasColumnName("name")
                   .IsRequired()
                   .HasMaxLength(100);

            // Навігаційна колекція Books
            builder.HasMany(a => a.Books)
                   .WithOne(b => b.Author)
                   .HasForeignKey(b => b.AuthorId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
