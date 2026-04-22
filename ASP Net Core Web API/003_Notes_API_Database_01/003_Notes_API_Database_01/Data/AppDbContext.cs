using _003_Notes_API_Database_01.Models;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace _003_Notes_API_Database_01.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Note> Notes { get; set; }
    }
}
