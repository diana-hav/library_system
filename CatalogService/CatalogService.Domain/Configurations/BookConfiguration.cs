using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Domain.Configurations
{
    public class BookConfiguration : IEntityTypeConfiguration<Book>
    {
        public void Configure(EntityTypeBuilder<Book> builder)
        {
            builder.ToTable("books");

            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id).HasColumnName("id");

            builder.Property(b => b.Title)
                   .HasColumnName("title")
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(b => b.AuthorId).HasColumnName("authorid");
            builder.Property(b => b.GenreId).HasColumnName("genreid");

            builder.HasOne(b => b.Author)
                   .WithMany(a => a.Books)
                   .HasForeignKey(b => b.AuthorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.Genre)
                   .WithMany(g => g.Books)
                   .HasForeignKey(b => b.GenreId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
