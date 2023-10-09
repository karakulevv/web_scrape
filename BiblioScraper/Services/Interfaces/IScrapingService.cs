using HtmlAgilityPack;

namespace BiblioScraper.Services.Interfaces
{
    public interface IScrapingService
    {
        Task ReadPagesAsync(string baseUrl, int currentPage, List<HtmlDocument> htmlDocuments, string nextPageUrl = null);
        Task SaveHtmlInParallel(List<HtmlDocument> htmlDocuments, string outputPath);
        Task SaveImagesInParallel(string baseUrl, List<HtmlDocument> htmlDocuments, string outputPath);
    }
}