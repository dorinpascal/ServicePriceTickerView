using Stock_Price_Service.Helpers;
using Stock_Price_Service.Models;
using Stock_Price_Service.Repositories.Cache;
using Stock_Price_Service.Repositories.Persistence;

namespace Stock_Price_Service.Services;

public class StockPriceService(IStockPriceCacheRepository _stockPriceCacheRepository, IStockPriceRepository _stockPriceRepository) : IStockPriceService
{
    public async Task GenerateAndStoreStockPricesAsync()
    {
        var stockPrices = await _stockPriceCacheRepository.GetAll();
        foreach (var stockPrice in stockPrices)
        {
            double newPrice = GenerateStockPrice.GenerateRandom();
            await _stockPriceCacheRepository.SetStockPriceAsync(new StockPrice { Price = newPrice, Ticker = stockPrice.Ticker });
        }
    }

    public async Task UpdateAsync(List<StockPrice> stockPrices)
    {
        foreach (var stockPrice in stockPrices)
        {
            await _stockPriceCacheRepository.SetStockPriceAsync(stockPrice);
        }
    }

    public async Task<IEnumerable<StockPrice>> GetByTickersAsync(List<string> tickers)
    {
        // Check if stock price is cached
        var cachedStockPrice = await _stockPriceCacheRepository.GetAllByTickers(tickers);
        if (cachedStockPrice.Any() && cachedStockPrice.Count() == tickers.Count)
        {
            return cachedStockPrice;
        }
        var stockPrices = await _stockPriceRepository.GetAllByTickers(tickers);
        // Cache for future
        await UpdateAsync([.. stockPrices]);
        return stockPrices;
    }

    public async Task<IEnumerable<StockPrice>> GetAllAsync()
    {
        var stockPrices = await _stockPriceCacheRepository.GetAll();
        return stockPrices;
    }
}
