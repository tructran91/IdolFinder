using Azure.Storage.Blobs;
using IdolFinder.CrawData.Services;

namespace IdolFinder.CrawData.Infrastructure.Storage
{
    public class BlobStorageService : IStorageService
    {
        private readonly BlobContainerClient _container;

        public BlobStorageService(IConfiguration config)
        {
            var connectionString = config["Storage:Blob:ConnectionString"];
            var containerName = config["Storage:Blob:Container"] ?? "images";
            _container = new BlobContainerClient(connectionString, containerName);
            _container.CreateIfNotExists();
        }

        public async Task<string> SaveFileAsync(string fileName, byte[] data)
        {
            var blob = _container.GetBlobClient(fileName);
            using var stream = new MemoryStream(data);
            await blob.UploadAsync(stream, overwrite: true);
            return blob.Uri.ToString();
        }

        public async Task<bool> DeleteFileAsync(string fileName)
        {
            var blob = _container.GetBlobClient(fileName);
            var response = await blob.DeleteIfExistsAsync();
            return response.Value;
        }

        public async Task<Stream?> GetFileAsync(string fileName)
        {
            var blob = _container.GetBlobClient(fileName);
            if (!await blob.ExistsAsync()) return null;
            var stream = new MemoryStream();
            await blob.DownloadToAsync(stream);
            stream.Position = 0;
            return stream;
        }
    }
}
