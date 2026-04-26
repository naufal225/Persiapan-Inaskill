using _004_Notes_API_Database_Async_01.Models;
using Microsoft.EntityFrameworkCore;

namespace _004_Notes_API_Database_Async_01.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Note> Notes { get; set; }
    }
}
