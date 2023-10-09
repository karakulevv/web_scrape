﻿using BiblioScraper.Services;
using HtmlAgilityPack;

namespace BiblioScraper
{
    public static class Program
    {
        private static readonly string baseUrl = "https://books.toscrape.com/";
        public static async Task Main(string[] args)
        {
            var httpClient = new HttpClient();

            var scrapeService = new ScrapingService(httpClient);

            var htmlDocuments = new List<HtmlDocument>();

            //process 1
            await scrapeService.ReadPagesAsync(baseUrl, 1, htmlDocuments);

            //process 2
            await scrapeService.SaveHtmlInParallel(htmlDocuments);

            //process 3
            await scrapeService.SaveImagesInParallel(htmlDocuments);            
        }
    }
}