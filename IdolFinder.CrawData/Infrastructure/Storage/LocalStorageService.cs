using IdolFinder.CrawData.Services;

namespace IdolFinder.CrawData.Infrastructure.Storage
{
    public class LocalStorageService : IStorageService
    {
        private readonly string _baseFolder;
        private readonly ILogger<LocalStorageService> _logger;

        public LocalStorageService(IConfiguration config, ILogger<LocalStorageService> logger)
        {
            _logger = logger;
            _baseFolder = config["Storage:ImageFolder"] ?? "D:\\IdolFinderData\\Images";
            Directory.CreateDirectory(_baseFolder);
        }

        public async Task<string> SaveFileAsync(string fileName, byte[] data)
        {
            string fullPath = Path.Combine(_baseFolder, fileName);
            await File.WriteAllBytesAsync(fullPath, data);
            _logger.LogInformation("Saved file: {path}", fullPath);
            return fullPath;
        }

        public Task<bool> DeleteFileAsync(string fileName)
        {
            string fullPath = Path.Combine(_baseFolder, fileName);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return Task.FromResult(true);
            }
            return Task.FromResult(false);
        }

        public Task<Stream?> GetFileAsync(string fileName)
        {
            string fullPath = Path.Combine(_baseFolder, fileName);
            if (!File.Exists(fullPath))
                return Task.FromResult<Stream?>(null);

            return Task.FromResult<Stream?>(File.OpenRead(fullPath));
        }
    }
}
