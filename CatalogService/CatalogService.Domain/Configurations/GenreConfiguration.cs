using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CatalogService.Domain.Configurations
{
    public class GenreConfiguration : IEntityTypeConfiguration<Genre>
    {
        public void Configure(EntityTypeBuilder<Genre> builder)
        {
            builder.ToTable("genres");

            builder.HasKey(g => g.Id);
            builder.Property(g => g.Id).HasColumnName("id");

            builder.Property(g => g.Name)
                   .HasColumnName("name")
                   .IsRequired()
                   .HasMaxLength(100);

            builder.HasMany(g => g.Books)
                   .WithOne(b => b.Genre)
                   .HasForeignKey(b => b.GenreId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
