using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

using Questinator.Models;

namespace Questinator.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        public DbSet<Quest> Quests { get; set; }
        public DbSet<Achievement> Achievements { get; set; }
        public DbSet<StoreItem> StoreItems { get; set; }
        public DbSet<CoinTransaction> CoinTransactions { get; set; }

    }
}