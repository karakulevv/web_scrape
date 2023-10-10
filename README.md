# BiblioScraper - Web Scraping Console Application

This console application is designed for web scraping tasks, specifically tailored to perform the following three steps:
1. **Recursively Scrape All Pages**: BiblioScraper recursively navigates through web pages to extract data.
2. **Parallel HTML Document Saving**: It saves HTML documents from the web pages in parallel, with a setup for up to 5 concurrent requests. The HTML files are stored in the OutputFiles directory within the BiblioScraper cloned repository.
3. **Parallel Image Saving**: BiblioScraper also concurrently downloads and saves images, with a setup for up to 5 concurrent requests.

## Installation
To install and run BiblioScraper on your local machine, follow these steps:

1. **Clone the Repository**:
- Clone the BiblioScraper repository to your local machine using Git.
2. **Navigate to the Project Directory**:
- Open a command prompt or PowerShell and navigate to the BiblioScraper/BiblioScraper directory within the cloned repository.
3. **Run the Application**:
- If you already have the .NET SDK installed on your local machine, simply execute the following command:
**dotnet run**
If you don't have the .NET SDK installed, you'll need to download and install it from the official .NET website (https://dotnet.microsoft.com/download). Once installed, execute the dotnet run command as mentioned above.
## Libraries Used
BiblioScraper utilizes the HtmlAgilityPack library for web scraping of HTML documents. This powerful library simplifies the parsing and manipulation of HTML content.

## Unit Tests
Unit tests for BiblioScraper can be found in the **BiblioScraper/BiblioScraper.Tests** directory. These tests focus on string manipulation, ensuring the accuracy and robustness of functions related to unique names, URLs, and other string operations.