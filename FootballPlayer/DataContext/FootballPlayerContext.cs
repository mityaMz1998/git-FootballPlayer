using Microsoft.EntityFrameworkCore;
using FootballPlayer.Models;

namespace FootballPlayer.DataContext
{
    /// <summary>
    /// Class for reading data from a database
    /// </summary>
    public class FootballPlayerContext : DbContext
    {
        public DbSet<Club> Clubs { get; set; } = null!;
        public DbSet<Player> Players { get; set; } = null!;
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-GJ5HJ6I;Database=dbFootballPlayer;Trusted_Connection=True;");
        }
    }
}
