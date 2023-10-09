using BiblioScraper.Helpers;
using BiblioScraper.Services.Interfaces;
using HtmlAgilityPack;

namespace BiblioScraper.Services
{
    public class ScrapingService : IScrapingService
    {
        private readonly HttpClient _httpClient;

        public ScrapingService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
                Console.WriteLine("Exception occurred during page scraping.\n", ex);
            }
        }

        /// <summary>
        /// Save html documents in parallel
        /// </summary>
        /// <param name="htmlDocuments"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task SaveHtmlInParallel(List<HtmlDocument> htmlDocuments, string outputPath)
        {
            EnsureOutputExists(outputPath);

            try
            {
                using var taskPool = new SemaphoreSlim(5); //5 parallel tasks
                var tasks = new List<Task>();

                foreach (var page in htmlDocuments)
                {
                    tasks.Add(Task.Run(async () =>
                    {
                        await taskPool.WaitAsync();

                        try
                        {
                            string fileName = StringHelpers.GenerateUniqueFileName(page);
                            string filePath = Path.Combine(outputPath, fileName);

                            // Save HTML content to file
                            await SaveHtmlDocumentToFile(page, filePath);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        finally
                        {
                            taskPool.Release();
                        }
                    }));
                }

                await Task.WhenAll(tasks);
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nCRITICAL ERROR!!!!!!!!");
                Console.WriteLine(ex.Message);
            }            
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
        private bool HasNextPage(HtmlDocument htmlDocument, out string nextPageUrl)
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

        private void EnsureOutputExists(string outputPath)
        {
            if (!Directory.Exists(outputPath))
            {
                // Directory doesn't exist, so create it
                Directory.CreateDirectory(outputPath);
            }
        }

        private async Task SaveHtmlDocumentToFile(HtmlDocument htmlDocument, string filePath)
        {
            using (var streamWriter = new StreamWriter(filePath))
            {
                try
                {
                    await streamWriter.WriteAsync(htmlDocument.DocumentNode.OuterHtml);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString());
                }
            }
        }
    }
}