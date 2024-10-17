using Stock_Price_Service.Dto;
using System.Text;
using System.Text.Json;

namespace SimpleTestClient;

public static class HttpClientTesting
{
    private const string ApiUrl = "http://localhost:7100/api/stockprice";

    public static async Task StartClient()
    {
        Console.WriteLine("Welcome to the Stock Price Client!");

        while (true)
        {
            Console.WriteLine("Enter stock tickers (comma-separated) or type 'exit' to quit:");
            var input = Console.ReadLine();

            if (string.Equals(input, "exit", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Exiting the client.");
                break; // Exit the loop if the user types 'exit'
            }

            if (string.IsNullOrEmpty(input))
            {
                Console.WriteLine("No tickers provided.");
                continue; // Continue the loop to prompt again
            }

            // Split input tickers into a list
            var tickers = input.Split(',', StringSplitOptions.RemoveEmptyEntries);

            // Call the function and get stock prices
            var stockPrices = await GetStockPricesAsync(tickers);

            if (stockPrices == null)
            {
                Console.WriteLine("No stock prices received.");
                continue; // Continue the loop to prompt again
            }

            // Display the stock prices
            Console.WriteLine("Stock prices:");
            foreach (var stockPrice in stockPrices)
            {
                Console.WriteLine($"Ticker: {stockPrice.Ticker}, Price: {stockPrice.Price}");
            }
        }
    }

    // Method to send tickers to the Azure Function and receive stock prices
    private static async Task<List<StockPriceDto>?> GetStockPricesAsync(string[] tickers)
    {
        using var client = new HttpClient();
        try
        {
            // Prepare request content
            var requestBody = JsonSerializer.Serialize(tickers);
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            // Send POST request to the Azure Function
            var response = await client.PostAsync(ApiUrl, content);

            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine($"Error: {response.StatusCode}");
                return null;
            }

            // Read the response content
            var responseBody = await response.Content.ReadAsStringAsync();

            // Deserialize JSON response to a list of StockPriceDto
            return JsonSerializer.Deserialize<List<StockPriceDto>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An error occurred: {ex.Message}");
            return null;
        }
    }
}
