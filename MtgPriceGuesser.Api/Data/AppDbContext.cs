using Microsoft.EntityFrameworkCore;
using MtgPriceGuesser.Api.Models;

namespace MtgPriceGuesser.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<HighScore> HighScores { get; set; }
    }
}
