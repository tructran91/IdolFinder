using IdolFinder.Core.Enums;

namespace IdolFinder.Core.Entities
{
    public class Image : BaseEntity
    {
        public Guid AlbumId { get; set; }
        public Album Album { get; set; }

        public string Url { get; set; }

        public ImageType ImageType { get; set; }
    }
}
