using Microsoft.EntityFrameworkCore;
using MigrationHelper.Models;

namespace MigrationHelper.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Currency>().HasData(
                new Currency
                {
                    ID = 1,
                    name = "GBP"
                },
                new Currency
                {
                    ID = 2,
                    name = "Euro"
                },
                new Currency
                {
                    ID = 3,
                    name = "USD"
                });


            builder.Entity<Rate>().HasData(
                new Rate
                {
                    ID = 1,
                    EUR = 1,
                    GBP = 0.835,
                    USD = 1.088                
                });
        }

        public DbSet<Register> Registers { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<UserAccount> UserAccounts { get; set; }
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
    }
}
