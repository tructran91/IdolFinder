using Azure.Storage.Blobs;
using IdolFinder.Application.Configurations.Options;
using IdolFinder.CrawData.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IdolFinder.Application.Services
{
    public class BlobStorageService : IStorageService
    {
        private readonly StorageOptions _options;
        private readonly BlobContainerClient _container;

        public BlobStorageService(
            IOptions<StorageOptions> options,
            ILogger<BlobStorageService> logger)
        {
            _options = options.Value;
            _container = new BlobContainerClient(_options.Blob.ConnectionString, _options.Blob.Container);
            _container.CreateIfNotExists();
        }

        public async Task<string> SaveFileAsync(string folderName, string fileName, byte[] data)
        {
            var blob = _container.GetBlobClient($"{folderName}/{fileName}");
            using var stream = new MemoryStream(data);
            await blob.UploadAsync(stream, overwrite: true);
            return blob.Uri.ToString();
        }

        public async Task<bool> DeleteFileAsync(string folderName, string fileName)
        {
            var blob = _container.GetBlobClient(fileName);
            var response = await blob.DeleteIfExistsAsync();
            return response.Value;
        }

        public async Task<Stream?> GetFileAsync(string folderName, string fileName)
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
