using Moq;
using Xunit;
using Microsoft.AspNetCore.Mvc;
using WebApi.Controllers;
using WebApi.Services;
using WebApi.Domain.DTOs;
using WebApi.Domain.Entities;

namespace WebApi.Tests.Controllers;

public class StocksControllerTests
{
    private readonly Mock<IStocksService> _mockStocksService;
    private readonly Mock<ICityService> _mockCityService;
    private readonly Mock<IMakeService> _mockMakeService;
    private readonly StocksController _controller;

    public StocksControllerTests()
    {
        _mockStocksService = new Mock<IStocksService>();
        _mockCityService = new Mock<ICityService>();
        _mockMakeService = new Mock<IMakeService>();

        _controller = new StocksController(
            _mockStocksService.Object, 
            _mockCityService.Object, 
            _mockMakeService.Object);
    }

    #region GetCities Tests

    [Fact]
    public async Task GetCities_ReturnsOk_WithData()
    {
        // Arrange
        var fakeCities = new List<CityDto> { new CityDto(1, "Mumbai", true) };
        _mockCityService.Setup(s => s.GetCitiesAsync()).ReturnsAsync(fakeCities);

        // Act
        var result = await _controller.GetCities();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(fakeCities, okResult.Value);
    }

    [Fact]
    public async Task GetCities_Returns500_OnException()
    {
        // Arrange
        _mockCityService.Setup(s => s.GetCitiesAsync()).ThrowsAsync(new Exception("Database Down"));

        // Act
        var result = await _controller.GetCities();

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, objectResult.StatusCode);
        Assert.Equal("An error occurred while processing your request.", objectResult.Value);
    }

    #endregion

    #region GetMakes Tests

    [Fact]
    public async Task GetMakes_ReturnsOk_WithData()
    {
        // Arrange
        var fakeMakes = new List<MakeDto> { new MakeDto(1, "Tesla") };
        _mockMakeService.Setup(s => s.GetMakesAsync()).ReturnsAsync(fakeMakes);

        // Act
        var result = await _controller.GetMakes();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
    }

    #endregion

    #region GetStocks Tests

    [Fact]
    public async Task GetStocks_ReturnsOk_WithPagedData()
    {
        // Arrange
        var filters = new FiltersDto { Page = 2 };
        var stocks = new List<StocksDto> { new StocksDto { /* set props */ } };
        int totalCount = 100;

        // Mocking the tuple return value (List<StocksDto>, int)
        _mockStocksService.Setup(s => s.GetStocksAsync(filters))
            .ReturnsAsync((stocks, totalCount));

        // Act
        var result = await _controller.GetStocks(filters);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var response = Assert.IsType<PagedResponse<StocksDto>>(okResult.Value);
        
        Assert.Equal(2, response.Page);
        Assert.Equal(totalCount, response.TotalCount);
        Assert.Equal(stocks, response.Stocks);
    }

    [Fact]
    public async Task GetStocks_Returns500_OnException()
    {
        // Arrange
        _mockStocksService.Setup(s => s.GetStocksAsync(It.IsAny<FiltersDto>()))
            .ThrowsAsync(new Exception("Service Error"));

        // Act
        var result = await _controller.GetStocks(new FiltersDto());

        // Assert
        var objectResult = Assert.IsType<ObjectResult>(result.Result);
        Assert.Equal(500, objectResult.StatusCode);
    }

    #endregion
}