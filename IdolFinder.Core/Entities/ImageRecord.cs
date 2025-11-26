namespace IdolFinder.Core.Entities
{
    public class ImageRecord : BaseEntity
    {
        public string SourceUrl { get; set; } = null!;
        public string SourcePage { get; set; } = null!;
        public string StoragePath { get; set; } = null!; // object key in MinIO
        public int Width { get; set; }
        public int Height { get; set; }
        public string? MimeType { get; set; }
        public double? PerceptualHash { get; set; } // placeholder for later
        public string Status { get; set; } = "Downloaded"; // Downloaded, Rejected, Processed
    }
}
