using HtmlAgilityPack;
using IdolFinder.Application.Configurations.Options;
using IdolFinder.Application.Images.Queries;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace IdolFinder.Application.Images.Handlers
{
    public class DownloadImagesHandler : IRequestHandler<DownloadImagesQuery, int>
    {
        private readonly ImageSourceOptions _options;

        public DownloadImagesHandler(
            IOptions<ImageSourceOptions> options)
        {
            _options = options.Value;
        }

        public async Task<int> Handle(DownloadImagesQuery request, CancellationToken cancellationToken)
        {
            var endpoint = "https://javpics.com/porn/yui-hatano";

            var result = new List<PinItem>();

            using var playwright = await Playwright.CreateAsync();
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = false
            });

            var context = await browser.NewContextAsync(new()
            {
                UserAgent =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) " +
                "AppleWebKit/537.36 (KHTML, like Gecko) " +
                "Chrome/120.0.0.0 Safari/537.36",
                ViewportSize = new() { Width = 1280, Height = 800 }
            });

            var page = await browser.NewPageAsync();

            await page.GotoAsync(endpoint, new PageGotoOptions
            {
                WaitUntil = WaitUntilState.NetworkIdle
            });

            // Nếu site lazy-load, có thể scroll thêm
            await page.EvaluateAsync(@"
                window.scrollTo(0, document.body.scrollHeight);
            ");
            await page.WaitForTimeoutAsync(3000);

            var html = await page.ContentAsync();

            // Parse bằng HtmlAgilityPack
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var nodes = doc.DocumentNode.SelectNodes(
                "//div[contains(concat(' ',normalize-space(@class),' '),' pincontainer ')]" +
                "//div[contains(concat(' ',normalize-space(@class),' '),' pincolumn ')]" +
                "//div[contains(concat(' ',normalize-space(@class),' '),' pinbox ') " +
                "and not(contains(concat(' ',normalize-space(@class),' '),' pinimg '))]" +
                "//a"
            );

            if (nodes == null)
                return 0;

            foreach (var a in nodes)
            {
                var href = a.GetAttributeValue("href", "");
                var imgNode = a.SelectSingleNode(".//img");
                var src = imgNode?.GetAttributeValue("src", "");

                if (!string.IsNullOrWhiteSpace(href) && !string.IsNullOrWhiteSpace(src))
                {
                    result.Add(new PinItem
                    {
                        Href = href,
                        ImgSrc = src
                    });
                }
            }

            return 0;



            //foreach (var img in imgNodes)
            //{
            //    var imgUrl = img.GetAttributeValue("src", "");
            //    if (string.IsNullOrWhiteSpace(imgUrl)) continue;

            //    if (!Uri.IsWellFormedUriString(imgUrl, UriKind.Absolute))
            //    {
            //        imgUrl = new Uri(new Uri(endpoint), imgUrl).ToString();
            //    }

            //    try
            //    {
            //        var imageBytes = await client.GetByteArrayAsync(imgUrl, cancellationToken);
            //        var fileName = Path.GetFileName(new Uri(imgUrl).LocalPath);
            //        await _storageService.SaveFileAsync(EndpointName.JAVpics, fileName, imageBytes);
            //        count++;
            //    }
            //    catch
            //    {
            //        // Có thể log lỗi ở đây nếu cần
            //    }
            //}
        }
    }

    public class PinItem
    {
        public string Href { get; set; }
        public string ImgSrc { get; set; }
        public string Text { get; set; }
    }
}
