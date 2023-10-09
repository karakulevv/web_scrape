using BiblioScraper.Services.Interfaces;
using HtmlAgilityPack;

namespace BiblioScraper.Services
{
    public class ScrapingService : IScrapingService
    {
        /// <summary>
        /// Scrape pages
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="currentPage"></param>
        /// <param name="htmlDocuments"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task ReadPagesAsync(string baseUrl, int currentPage, List<HtmlDocument> htmlDocuments)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save html documents in parallel
        /// </summary>
        /// <param name="htmlDocuments"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task SaveHtmlInParallel(List<HtmlDocument> htmlDocuments)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save images to disk in parallel 
        /// </summary>
        /// <param name="htmlDocuments"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Task SaveImagesInParallel(List<HtmlDocument> htmlDocuments)
        {
            throw new NotImplementedException ();
        }
    }
}