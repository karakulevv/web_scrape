using BiblioScraper.Services;
using HtmlAgilityPack;

namespace BiblioScraper
{
    public static class Program
    {
        private static readonly string baseUrl = "https://books.toscrape.com/";
        private static readonly string outputPath = Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "\\OutputFiles\\");

        public static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();

            var scrapeService = new ScrapingService(httpClient);

            var htmlDocuments = new List<HtmlDocument>();

            Console.WriteLine("=============Web Scraping Console App=============");
            Console.WriteLine("==============Powered by karakulevv===============");
            Console.WriteLine("==================================================");

            //process 1
            Console.WriteLine("\nProcess 1/3 starting...");
            await scrapeService.ReadPagesAsync(baseUrl, 1, htmlDocuments);

            //process 2
            Console.WriteLine("\nProcess 2/3 starting...");
            await scrapeService.SaveHtmlInParallel(htmlDocuments, outputPath);

            //process 3
            Console.WriteLine("\nProcess 3/3 starting...");
            await scrapeService.SaveImagesInParallel(baseUrl, htmlDocuments, outputPath);
        }
    }
}