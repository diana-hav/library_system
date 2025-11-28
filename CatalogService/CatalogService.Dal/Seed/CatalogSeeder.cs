using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatalogService.Dal.Seed;

public static class CatalogSeeder
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>().HasData(
            new Author { Id = 1, Name = "Тарас Шевченко" },
            new Author { Id = 2, Name = "Ліна Костенко" },
            new Author { Id = 3, Name = "Іван Франко" }
        );

        modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = 1, Name = "Поезія" },
            new Genre { Id = 2, Name = "Проза" },
            new Genre { Id = 3, Name = "Фантастика" }
        );

        modelBuilder.Entity<Book>().HasData(
            new Book { Id = 1, Title = "Кобзар", AuthorId = 1, GenreId = 1 },
            new Book { Id = 2, Title = "Маруся Чурай", AuthorId = 2, GenreId = 1 },
            new Book { Id = 3, Title = "Захар Беркут", AuthorId = 3, GenreId = 2 }
        );
    }
}
