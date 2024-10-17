using AutoFixture;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;
using ServicePriceTickerViewTests.Helpers;
using Stock_Price_Service.Dto;
using Stock_Price_Service.Functions.Http;
using Stock_Price_Service.Models;
using Stock_Price_Service.Services;

namespace ServicePriceTickerViewTests.FunctionTests;

public class GetStockPriceByTickerFunctionTests
{
    private readonly StockPriceFunctions _sut;
    private readonly IStockPriceService _stockPriceService;
    private readonly ILogger<StockPriceFunctions> _logger;
    private readonly HttpRequest _request;
    private readonly Fixture _fixture;

    public GetStockPriceByTickerFunctionTests()
    {
        _stockPriceService = Substitute.For<IStockPriceService>();
        _logger = Substitute.For<ILogger<StockPriceFunctions>>();
        _sut = new StockPriceFunctions(_logger, _stockPriceService);
        var context = new DefaultHttpContext();
        context.Request.ContentType = "application/x-www-form-urlencoded";
        _request = context.Request;
        _fixture = new CustomFixture();
    }

    [Fact]
    public async Task GetStockPriceByTickerFunctionTests_ServiceThrowsError_Returns503()
    {
        // Arrange
        // Act
        _stockPriceService.GetByTickersAsync(Arg.Any<List<string>>()).Throws(new Exception());
        var result = await _sut.GetStockPriceByTickerFunction(_request, ["GOOG"]);

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(StatusCodes.Status503ServiceUnavailable, objectResult.StatusCode);
    }

    [Fact]
    public async Task GetStockPriceByTickerFunctionTests_ServiceIsCalled_ReturnsEmptyList()
    {
        // Arrange
        var tickers = new List<StockPrice>();
        // Act
        _stockPriceService.GetByTickersAsync(Arg.Any<List<string>>()).Returns(tickers);
        var response = await _sut.GetStockPriceByTickerFunction(_request, ["GOOG"]);
        var result = Assert.IsType<OkObjectResult>(response);
        var responseData = result.Value as List<StockPriceDto?>;

        // Assert
        Assert.Empty(responseData!);
    }

    [Fact]
    public async Task GetStockPriceByTickerFunctionTests_ServiceIsCalled_ReturnsListOfObjects()
    {
        // Arrange
        var tickers = _fixture.Create<List<StockPrice>>();
        // Act
        _stockPriceService.GetByTickersAsync(Arg.Any<List<string>>()).Returns(tickers);
        var response = await _sut.GetStockPriceByTickerFunction(_request, ["GOOG"]);
        var result = Assert.IsType<OkObjectResult>(response);
        var responseData = result.Value as List<StockPriceDto>;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(tickers.Count, responseData!.Count);
    }
}
