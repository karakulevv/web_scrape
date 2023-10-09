﻿using BiblioScraper.Helpers;
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
        public async Task ReadPagesAsync(string baseUrl, int currentPage, List<HtmlDocument> htmlDocuments, string nextPageUrl = null)
        {
            try
            {
                var fullUrl = StringHelpers.GetUrl(baseUrl, nextPageUrl);
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

                            await SaveContentToFileAsync(page.DocumentNode.OuterHtml, filePath);
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
        /// <param name="baseUrl"></param>
        /// <param name="htmlDocuments"></param>
        /// <param name="outputPath"></param>
        /// <returns></returns>
        public async Task SaveImagesInParallel(string baseUrl, List<HtmlDocument> htmlDocuments, string outputPath)
        {
            EnsureOutputExists(outputPath);

            try
            {
                using var taskPool = new SemaphoreSlim(5);
                var tasks = new List<Task>();

                foreach (var htmlDocument in htmlDocuments)
                {
                    var imageNodes = htmlDocument.DocumentNode.SelectNodes("//img[@src]");
                    if (imageNodes != null)
                    {
                        foreach (var imageNode in imageNodes)
                        {
                            tasks.Add(Task.Run(async () =>
                            {
                                await taskPool.WaitAsync();

                                try
                                {
                                    var imageUrl = imageNode.GetAttributeValue("src", "");
                                    if (!string.IsNullOrEmpty(imageUrl))
                                    {
                                        var fullPath = string.Concat(baseUrl, imageUrl);
                                        var imageFilePath = StringHelpers.GetImageFilePath(baseUrl, outputPath, imageUrl);

                                        var imageBytes = await _httpClient.GetByteArrayAsync(fullPath);
                                        await SaveContentToFileAsync(imageBytes, imageFilePath);
                                    }
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
                    }
                    await Task.WhenAll(tasks);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("\nCRITICAL ERROR!!!!!!!!");
                Console.WriteLine(ex.Message);
            }
        }

        /// <summary>
        /// Return next page url if next page exists
        /// </summary>
        /// <param name="htmlDocument"></param>
        /// <param name="nextPageUrl"></param>
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
        /// Create directory if non-existing
        /// </summary>
        /// <param name="outputPath"></param>
        private void EnsureOutputExists(string outputPath)
        {
            if (!Directory.Exists(outputPath))
            {
                // Directory doesn't exist, so create it
                Directory.CreateDirectory(outputPath);
            }
        }

        /// <summary>
        /// Write document string content to file path
        /// </summary>
        /// <param name="content"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private async Task SaveContentToFileAsync(string content, string filePath)
        {
            using (var streamWriter = new StreamWriter(filePath))
            {
                try
                {
                    await streamWriter.WriteAsync(content);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }

        /// <summary>
        /// Write document byte content to file path
        /// </summary>
        /// <param name="content"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private async Task SaveContentToFileAsync(byte[] content, string filePath)
        {
            using (var streamWriter = new FileStream(filePath, FileMode.Create))
            {
                try
                {
                    await streamWriter.WriteAsync(content, 0, content.Length);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}