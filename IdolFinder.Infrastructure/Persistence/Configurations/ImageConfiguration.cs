using IdolFinder.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IdolFinder.Infrastructure.Persistence.Configurations
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasOne(i => i.Album)
               .WithMany(a => a.Images)
               .HasForeignKey(i => i.AlbumId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
