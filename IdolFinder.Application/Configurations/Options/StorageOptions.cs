namespace IdolFinder.Application.Configurations.Options
{
    public class StorageOptions
    {
        public string Type { get; set; }

        public string ImageFolder { get; set; }

        public BlobOptions Blob { get; set; }
    }

    public class BlobOptions
    {
        public string ConnectionString { get; set; }

        public string Container { get; set; }
    }
}
