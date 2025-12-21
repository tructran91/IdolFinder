namespace IdolFinder.Core.Entities
{
    public class Idol : BaseEntity
    {
        public string Name { get; set; }

        public Guid? CoverImageId { get; set; }
        public Image CoverImage { get; set; }

        public ICollection<Album> Albums { get; set; }
    }
}
