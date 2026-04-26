using _005_Expense_Tracker_API_01.Models;
using Microsoft.EntityFrameworkCore;

namespace _005_Expense_Tracker_API_01.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Transaction> Transactions { get; set; }
    }
}
