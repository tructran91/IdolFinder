using HtmlAgilityPack;
using IdolFinder.Application.Constants;
using IdolFinder.Application.Images.Handlers;
using IdolFinder.CrawData.Services;

namespace IdolFinder.Application.Images.Endpoints
{
    public class AbcEndpointHandler : BaseEndpointHandler
    {
        public AbcEndpointHandler(IStorageService storageService, IHttpClientFactory httpClientFactory)
            : base(storageService, httpClientFactory) { }

        public override async Task<int> HandleAsync(string endpoint, CancellationToken cancellationToken)
        {
            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/117.0.0.0 Safari/537.36");

            var html = await client.GetStringAsync(endpoint, cancellationToken);

            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var divNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'product-card-default-image')]");
            var imgNodes = divNodes?
                .SelectMany(div => div.SelectNodes(".//img[@src]") ?? Enumerable.Empty<HtmlNode>())
                .ToList();
            int count = 0;

            if (imgNodes == null)
                return count;

            foreach (var img in imgNodes)
            {
                var imgUrl = img.GetAttributeValue("src", "");
                if (string.IsNullOrWhiteSpace(imgUrl)) continue;

                if (!Uri.IsWellFormedUriString(imgUrl, UriKind.Absolute))
                {
                    imgUrl = new Uri(new Uri(endpoint), imgUrl).ToString();
                }

                try
                {
                    var imageBytes = await client.GetByteArrayAsync(imgUrl, cancellationToken);
                    var fileName = Path.GetFileName(new Uri(imgUrl).LocalPath);
                    await _storageService.SaveFileAsync(EndpointName.Abc, fileName, imageBytes);
                    count++;
                }
                catch
                {
                    // Có thể log lỗi ở đây nếu cần
                }
            }

            return count;
        }
    }
}