using Microsoft.EntityFrameworkCore;
using Questinator.AI.Models;

namespace Questinator.AI.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Quest> Quests { get; set; }
    }
}