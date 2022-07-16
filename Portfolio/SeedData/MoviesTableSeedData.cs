using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Portfolio.Data;
using Portfolio.Models;
using System;
using System.Linq;

namespace Portfolio.SeedData
{
    public static class MoviesTableSeedData
    {
        /// <summary>
        /// 如果DB中，Movies的資料表沒有任何資料，則寫入FirstSeedDataToDB()中預設的資料
        /// </summary>
        public static void FirstSeedDataToMoviesTable(IServiceProvider serviceProvider)
        {
            using (var context = new PortfolioContext(serviceProvider.GetRequiredService<DbContextOptions<PortfolioContext>>()))
            {
                // 如果DB中，已經存有資料，則不要寫入預設資料到DB中
                if (context.Movies.Any())
                {
                    return;
                }

                context.Movies.AddRange(
                    new Movies {
                        Title = "When Harry Met Sally",
                        ReleaseDate = DateTime.Parse("1989-2-12"),
                        Genre = "Romantic Comedy",
                        Price = 7.99M
                    },

                    new Movies
                    {
                        Title = "Ghostbusters ",
                        ReleaseDate = DateTime.Parse("1984-3-13"),
                        Genre = "Comedy",
                        Price = 8.99M
                    },

                    new Movies
                    {
                        Title = "Ghostbusters 2",
                        ReleaseDate = DateTime.Parse("1986-2-23"),
                        Genre = "Comedy",
                        Price = 9.99M
                    },

                    new Movies
                    {
                        Title = "Rio Bravo",
                        ReleaseDate = DateTime.Parse("1959-4-15"),
                        Genre = "Western",
                        Price = 3.99M
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
