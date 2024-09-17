using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Prolance.Domain.Entities;

namespace Prolance.Infrastructure.Persistence.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public DbSet<Bid> Bids { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Payroll> Payrolls { get; set; }

        public ApplicationDbContext(DbContextOptions options)
             : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

        }

    }
}
