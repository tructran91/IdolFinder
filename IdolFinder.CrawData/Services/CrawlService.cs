namespace IdolFinder.CrawData.Services
{
    public class CrawlService
    {
        private readonly IStorageService _storage;

        public CrawlService(IStorageService storage)
        {
            _storage = storage;
        }

        public async Task SaveImageAsync(string url, string fileName)
        {
            using var http = new HttpClient();
            var data = await http.GetByteArrayAsync(url);
            await _storage.SaveFileAsync(fileName, data);
        }
    }

}
