using BiblioScraper.Services;
using BiblioScraper.Services.Interfaces;
using HtmlAgilityPack;
using Microsoft.Extensions.DependencyInjection;

namespace BiblioScraper
{
    public class Program
    {
        private static readonly string baseUrl = "https://books.toscrape.com/";
        public static void Main(string[] args)
        {
            var serviceProvider = new ServiceCollection()
                .AddScoped<IScrapingService, ScrapingService>()
                .BuildServiceProvider();

            using (var scope = serviceProvider.CreateScope())
            {
                var scrapeService = scope.ServiceProvider.GetRequiredService<IScrapingService>();

                var htmlDocuments = new List<HtmlDocument>();
                scrapeService.ReadPagesAsync(baseUrl, 1, htmlDocuments).Wait();

                scrapeService.SaveHtmlInParallel(htmlDocuments).Wait();

                scrapeService.SaveImagesInParallel(htmlDocuments).Wait();
            }
        }
    }

}