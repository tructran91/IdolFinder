using IdolFinder.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace IdolFinder.Infrastructure.Data
{
    public class IdolFinderContext : DbContext
    {
        public IdolFinderContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<ImageRecord> Images { get; set; }
    }
}
