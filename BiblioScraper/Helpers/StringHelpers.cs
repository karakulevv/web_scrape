using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiblioScraper.Helpers
{
    public static class StringHelpers
    {
        public static string GenerateUniqueFileName(HtmlDocument htmlDocument)
        {
            var currentPageElement = htmlDocument.DocumentNode.SelectSingleNode("//li[@class='current']");
            var currentPageText = currentPageElement.InnerText.Trim().Split(" ");
            int.TryParse(currentPageText[1], out int currentPage);

            return String.Concat("Page-", currentPage, ".html");
        }
    }
}
