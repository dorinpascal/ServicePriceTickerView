using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.WebJobs.Extensions.OpenApi.Core.Attributes;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Stock_Price_Service.Dto;
using Stock_Price_Service.Helpers;
using Stock_Price_Service.Services;
using System.Net;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

namespace Stock_Price_Service.Functions.Http;

public class StockPriceFunctions(ILogger<StockPriceFunctions> logger, IStockPriceService stockPriceService)
{
    [Function(nameof(GetStockPriceByTickerFunction))]
    [OpenApiOperation(operationId: nameof(GetStockPriceByTickerFunction), tags: ["StockPrice"])]
    [OpenApiRequestBody(contentType: "application/json", bodyType: typeof(List<string>))]
    [OpenApiResponseWithBody(HttpStatusCode.OK, "application/json", typeof(List<StockPriceDto>), Description = "The OK response with list of stock price")]
    [OpenApiResponseWithBody(HttpStatusCode.NotFound, contentType: "application/json", bodyType: typeof(string), Description = "Stock ticker not found.")]
    public async Task<IActionResult> GetStockPriceByTickerFunction([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "stockprice")] HttpRequest req, [FromBody] List<string> tickers)
    {
        try
        {
            //ToDo Implement authorization
            var stockPrice = await stockPriceService.GetByTickersAsync(tickers);
            //Use AutoMapper
            return new OkObjectResult(stockPrice.Select(x => new StockPriceDto(x.Ticker, x.Price)).ToList());
        }
        catch (Exception ex)
        {
            return HttpResponseMessageHelper.HandleException(ex, logger);
        }
    }
}
