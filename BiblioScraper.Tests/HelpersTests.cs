using NUnit.Framework;
using BiblioScraper.Helpers;

namespace BiblioScraper.Tests
{
    [TestFixture]
    public class HelpersTests
    {
        private string _outputPath;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            _outputPath = Directory.GetCurrentDirectory().Replace("\\bin\\Debug\\net6.0", "\\OutputFiles\\"); 
        }

        [TestCase("https://books.toscrape.com/", "catalogue/page-2.html", ExpectedResult = "https://books.toscrape.com/catalogue/page-2.html")]
        [TestCase("https://books.toscrape.com/", "page-3.html", ExpectedResult = "https://books.toscrape.com/catalogue/page-3.html")]
        [TestCase("https://books.toscrape.com/", "", ExpectedResult = "https://books.toscrape.com/")]
        [TestCase("https://books.toscrape.com/", null, ExpectedResult = "https://books.toscrape.com/")]
        public string GetUrl_ValidInput_ReturnsCorrectUrl(string baseUrl, string nextPageUrl)
        {
            return StringHelpers.GetUrl(baseUrl, nextPageUrl);
        }

        [TestCase("    Page 12 of 51", ExpectedResult = "Page-12.html")]
        [TestCase("    Page 2 of 100     ", ExpectedResult = "Page-2.html")]
        [TestCase("Page 8 of 19         ", ExpectedResult = "Page-8.html")]
        [TestCase("Page 1 of 3", ExpectedResult = "Page-1.html")]
        public string GenerateUniqueFileName_ValidInput_ReturnsCorrectName(string name)
        {
            return StringHelpers.GenerateUniqueFileName(name);
        }

        [TestCase("https://books.toscrape.com/", $"C:\\GIT\\Other\\Web Scraping\\BiblioScraper\\BiblioScraper\\OutputFiles\\", "media/cache/68/33/68339b4c9bc034267e1da611ab3b34f8.jpg", ExpectedResult = "C:\\GIT\\Other\\Web Scraping\\BiblioScraper\\BiblioScraper\\OutputFiles\\68339b4c9bc034267e1da611ab3b34f8.jpg")]
        [TestCase("https://books.toscrape.com/", $"C:\\GIT\\Other\\Web Scraping\\BiblioScraper\\BiblioScraper\\OutputFiles\\", "cache/68/33/68339b4c9bc034267e1da611ab3b34f8.jpg", ExpectedResult = "C:\\GIT\\Other\\Web Scraping\\BiblioScraper\\BiblioScraper\\OutputFiles\\68339b4c9bc034267e1da611ab3b34f8.jpg")]
        [TestCase("https://books.toscrape.com/", $"C:\\GIT\\Other\\Web Scraping\\BiblioScraper\\BiblioScraper\\OutputFiles\\", "68/33/68339b4c9bc034267e1da611ab3b34f8.jpg", ExpectedResult = "C:\\GIT\\Other\\Web Scraping\\BiblioScraper\\BiblioScraper\\OutputFiles\\68339b4c9bc034267e1da611ab3b34f8.jpg")]
        [TestCase("https://books.toscrape.com/", $"C:\\GIT\\Other\\Web Scraping\\BiblioScraper\\BiblioScraper\\OutputFiles\\", "33/68339b4c9bc034267e1da611ab3b34f8.jpg", ExpectedResult = "C:\\GIT\\Other\\Web Scraping\\BiblioScraper\\BiblioScraper\\OutputFiles\\68339b4c9bc034267e1da611ab3b34f8.jpg")]
        [TestCase("https://books.toscrape.com/", $"C:\\GIT\\Other\\Web Scraping\\BiblioScraper\\BiblioScraper\\OutputFiles\\", "media/68339b4c9bc034267e1da611ab3b34f8.jpg", ExpectedResult = "C:\\GIT\\Other\\Web Scraping\\BiblioScraper\\BiblioScraper\\OutputFiles\\68339b4c9bc034267e1da611ab3b34f8.jpg")]
        public string GetImageFilePath_ValidInput_ReturnsCorrectImagePath(string baseUrl, string outputPath, string imageUrl)
        {
            return StringHelpers.GetImageFilePath(baseUrl, outputPath, imageUrl);
        }
    }
}
