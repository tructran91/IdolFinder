using IdolFinder.Application.Configurations.Options;
using IdolFinder.CrawData.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IdolFinder.Application.Services
{
    public class LocalStorageService : IStorageService
    {
        private readonly StorageOptions _options;
        private readonly ILogger<LocalStorageService> _logger;

        public LocalStorageService(
            IOptions<StorageOptions> options,
            ILogger<LocalStorageService> logger)
        {
            _options = options.Value;
            _logger = logger;
        }

        public async Task<string> SaveFileAsync(string folderName, string fileName, byte[] data)
        {
            string folderPath = Path.Combine(_options.ImageFolder, folderName);
            Directory.CreateDirectory(folderPath);
            string fullPath = Path.Combine(folderPath, fileName);

            await File.WriteAllBytesAsync(fullPath, data);
            _logger.LogInformation($"Saved file: {fullPath}");

            return fullPath;
        }

        public Task<bool> DeleteFileAsync(string folderName, string fileName)
        {
            string fullPath = Path.Combine(_options.ImageFolder, folderName, fileName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<Stream?> GetFileAsync(string folderName, string fileName)
        {
            string fullPath = Path.Combine(_options.ImageFolder, folderName, fileName);
            if (!File.Exists(fullPath))
                return Task.FromResult<Stream?>(null);

            return Task.FromResult<Stream?>(File.OpenRead(fullPath));
        }
    }
}
