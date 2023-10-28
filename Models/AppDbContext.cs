using Microsoft.EntityFrameworkCore;
using ExpenseTrackerMVC.Models;

namespace ExpenseTrackerMVC.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options): base(options)
        {

        }

        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Category> Category { get; set; }
        

    }
}
