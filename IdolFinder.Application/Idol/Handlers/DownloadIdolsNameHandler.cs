using HtmlAgilityPack;
using IdolFinder.Application.Configurations.Options;
using IdolFinder.Application.Idol.Queries;
using MediatR;
using Microsoft.Extensions.Options;
using Microsoft.Playwright;

namespace IdolFinder.Application.Images.Handlers
{
    public class DownloadIdolsNameHandler : IRequestHandler<DownloadIdolsNameQuery, int>
    {
        private readonly ImageSourceOptions _options;

        public DownloadIdolsNameHandler(
            IOptions<ImageSourceOptions> options)
        {
            _options = options.Value;
        }

        public async Task<int> Handle(DownloadIdolsNameQuery request, CancellationToken cancellationToken)
        {
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

            var page = await context.NewPageAsync();

            const int MaxPages = 1;

            for (int pageIndex = 1; pageIndex <= MaxPages; pageIndex++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var url = BuildUrl(request.Category, pageIndex);

                await page.GotoAsync(url, new PageGotoOptions
                {
                    WaitUntil = WaitUntilState.NetworkIdle
                });

                await page.EvaluateAsync("window.scrollTo(0, document.body.scrollHeight);");
                await page.WaitForTimeoutAsync(3000);

                var html = await page.ContentAsync();

                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                var nodes = doc.DocumentNode.SelectNodes(
                    "//div[contains(concat(' ',normalize-space(@class),' '),' pincontainer ')]" +
                    "//div[contains(concat(' ',normalize-space(@class),' '),' pincolumn ')]" +
                    "//div[contains(concat(' ',normalize-space(@class),' '),' pinbox ') " +
                    "and not(contains(concat(' ',normalize-space(@class),' '),' pinimg '))]" +
                    "//a"
                );

                if (nodes == null || nodes.Count == 0)
                    break;

                foreach (var node in nodes)
                {
                    var pNode = node.SelectSingleNode("./p");
                    var text = pNode?.InnerText?.Trim();

                    result.Add(new PinItem
                    {
                        Text = text
                    });
                }
            }
            
            return 0;
        }

        private static string BuildUrl(string category, int page)
        {
            return $"https://javpics.com/porn/{category}/{page}";
        }
    }
}
