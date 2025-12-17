using HtmlAgilityPack;
using IdolFinder.CrawData.Services;

namespace IdolFinder.Application.Images.Handlers
{
    public abstract class BaseEndpointHandler
    {
        protected readonly IStorageService _storageService;
        protected readonly IHttpClientFactory _httpClientFactory;

        protected BaseEndpointHandler(IStorageService storageService, IHttpClientFactory httpClientFactory)
        {
            _storageService = storageService;
            _httpClientFactory = httpClientFactory;
        }

        public abstract Task<int> HandleAsync(string endpoint, CancellationToken cancellationToken);

        protected async Task<List<string>> ExtractImageUrlsAsync(string html, string baseUrl)
        {
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var imgNodes = doc.DocumentNode.SelectNodes("//img[@src]");
            var urls = imgNodes?
                .Select(img => img.GetAttributeValue("src", ""))
                .Where(src => !string.IsNullOrWhiteSpace(src))
                .Select(src => Uri.IsWellFormedUriString(src, UriKind.Absolute) ? src : new Uri(new Uri(baseUrl), src).ToString())
                .ToList();

            return urls ?? new List<string>();
        }
    }
}
