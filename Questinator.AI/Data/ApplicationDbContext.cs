using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Questinator.AI.Models;

namespace Questinator.AI.Data
{
    public class ApplicationDbContext
        : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(
            DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Quest> Quests { get; set; }
        public DbSet<CoinTransaction> CoinTransactions { get; set; }
        public DbSet<SessionToken> SessionTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<SessionToken>(entity =>
            {
                entity.Property(x => x.UserId).HasMaxLength(450).IsRequired();
                entity.Property(x => x.TokenHash).HasMaxLength(200).IsRequired();
                entity.Property(x => x.CreatedAt).HasDefaultValueSql("SYSUTCDATETIME()");
            });
        }
    }
}