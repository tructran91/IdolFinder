using IdolFinder.CrawData.Models;
using Microsoft.EntityFrameworkCore;

namespace IdolFinder.CrawData.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> opts) : base(opts) { }

        public DbSet<ImageRecord> Images => Set<ImageRecord>();
    }
}
