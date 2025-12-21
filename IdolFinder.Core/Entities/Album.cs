namespace IdolFinder.Core.Entities
{
    public class Album : BaseEntity
    {
        public string Title { get; set; }

        public Guid IdolId { get; set; }
        public Idol Idol { get; set; }

        public Guid? CoverImageId { get; set; }
        public Image CoverImage { get; set; }

        public ICollection<Image> Images { get; set; }
    }
}
