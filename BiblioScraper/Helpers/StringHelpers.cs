using HtmlAgilityPack;

namespace BiblioScraper.Helpers
{
    public static class StringHelpers
    {
        /// <summary>
        /// Generate unique name for html document
        /// </summary>
        /// <param name="htmlDocument"></param>
        /// <returns></returns>
        public static string GenerateUniqueFileName(string currentPageText)
        {
            var textArray = currentPageText.Trim().Split(" ");
            int.TryParse(textArray[1], out int currentPage);

            return String.Concat("Page-", currentPage, ".html");
        }

        /// <summary>
        /// Catalog is not present in the button link after 2nd page hence we need to add it manually
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="nextPageUrl"></param>
        /// <returns></returns>
        public static string GetUrl(string baseUrl, string nextPageUrl)
        {
            if (!string.IsNullOrEmpty(nextPageUrl))
            {
                nextPageUrl = nextPageUrl.Contains("catalogue") ? nextPageUrl : "catalogue/" + nextPageUrl;
            }

            return baseUrl + nextPageUrl;
        }

        /// <summary>
        /// Return img file path combined
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="outputPath"></param>
        /// <param name="imageUrl"></param>
        /// <returns></returns>
        public static string GetImageFilePath(string baseUrl, string outputPath, string imageUrl)
        {
            var fullPath = string.Concat(baseUrl, imageUrl);
            var uri = new Uri(fullPath);
            var imageName = Path.GetFileName(uri.LocalPath);

            // Save the image to the directory
            return Path.Combine(outputPath, imageName);
        }
    }
}