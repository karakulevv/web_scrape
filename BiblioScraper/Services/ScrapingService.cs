using BiblioScraper.Services.Interfaces;
using HtmlAgilityPack;

namespace BiblioScraper.Services
{
    public class ScrapingService : IScrapingService
    {
        private readonly HttpClient _httpClient;
        private readonly string _outputPath;

        public ScrapingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _outputPath = Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "\\OutputFiles\\");
        }

        /// <summary>
        /// Scrape pages
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="currentPage"></param>
        /// <param name="htmlDocuments"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task ReadPagesAsync(string baseUrl, int currentPage, List<HtmlDocument> htmlDocuments, string nextPageUrl = null)
        {
            try
            {
                var fullUrl = GetUrl(baseUrl, nextPageUrl);
                string htmlContent = await _httpClient.GetStringAsync(fullUrl);

                var htmlDocument = new HtmlDocument();
                htmlDocument.LoadHtml(htmlContent);

                htmlDocuments.Add(htmlDocument);
                if (HasNextPage(htmlDocument, out nextPageUrl))
                {
                    currentPage++;
                    await ReadPagesAsync(baseUrl, currentPage, htmlDocuments, nextPageUrl);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Exception occurred during page scraping.", ex);
            }
        }

        /// <summary>
        /// Save html documents in parallel
        /// </summary>
        /// <param name="htmlDocuments"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SaveHtmlInParallel(List<HtmlDocument> htmlDocuments)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Save images to disk in parallel 
        /// </summary>
        /// <param name="htmlDocuments"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SaveImagesInParallel(List<HtmlDocument> htmlDocuments)
        {
            throw new NotImplementedException();
        }


        /// <summary>
        /// Return next page url if next page exists
        /// </summary>
        /// <param name="htmlDocument"></param>
        /// <param name="nextPageUrl"></param>
        /// <returns></returns>
        public bool HasNextPage(HtmlDocument htmlDocument, out string nextPageUrl)
        {
            nextPageUrl = null;

            var nextButton = htmlDocument.DocumentNode.SelectSingleNode("//li[@class='next']");
            if (nextButton != null)
            {
                nextPageUrl = nextButton.FirstChild.GetAttributes("href").FirstOrDefault().Value;
                return true;
            }
            return false;
        }

        /// <summary>
        /// Catalog is not present in the button link after 2nd page hence we need to add it manually
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="nextPageUrl"></param>
        /// <returns></returns>
        private string GetUrl(string baseUrl, string nextPageUrl)
        {
            if (!string.IsNullOrEmpty(nextPageUrl))
            {
                nextPageUrl = nextPageUrl.Contains("catalogue") ? nextPageUrl : "catalogue/" + nextPageUrl;
            }

            return baseUrl + nextPageUrl;
        }
    }
}