using Stock_Price_Service.Models;

namespace Stock_Price_Service.Services;


/// <summary>
/// Interface for managing stock prices, including generating, updating, caching, and retrieving stock price data.
/// </summary>
public interface IStockPriceService
{
    /// <summary>
    /// Generates stock prices and stores them in the cache.
    /// This method simulates price generation for hardcoded tickers.
    /// </summary>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task GenerateAndStoreStockPricesAsync();

    /// <summary>
    /// Updates the stock price for a specific ticker.
    /// </summary>
    /// <param name="stockPrices">List of StockPrice object containing the updated price information.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task UpdateAsync(List<StockPrice> stockPrices);

    /// <summary>
    /// Retrieves stock prices for a list of tickers .
    /// </summary>
    /// <param name="tickers">A list of stock ticker symbols.</param>
    /// <returns>
    /// A task representing the asynchronous operation, containing a collection of StockPrice objects 
    /// for the provided tickers.
    /// </returns>
    Task<IEnumerable<StockPrice>> GetByTickersAsync(List<string> tickers);

    /// <summary>
    /// Retrieves all stock prices.
    /// </summary>
    /// <returns>
    /// A task representing the asynchronous operation, containing a collection of StockPrice objects 
    /// for the provided tickers.
    /// </returns>
    Task<IEnumerable<StockPrice>> GetAllAsync();
}