using Moq;
using Xunit;
using Grpc.Core;
using System.Collections.Generic;
using System.Threading.Tasks;

// --- ALIAS SECTION: This solves CS0118 ---
// 'GrpcModels' points to the gRPC generated namespace
using GrpcModels = Stocks; 
// 'StockEntity' points to your database class
//using StockEntity = Contracts.Entities.Stocks; 
// 'FilterEntity' points to your filter class
using FilterEntity = Contracts.Entities.Filters;

using GrpcService.Services;
using GrpcService.Repositories;
using GrpcService.Mappers;

namespace GrpcService.Tests;

public class GrpcStocksServiceTests
{
    private readonly Mock<IGrpcStocksRepository> _mockRepo;
    private readonly Mock<IStocksProtoMapper> _mockMapper;
    private readonly GrpcStocksService _service;

    public GrpcStocksServiceTests()
    {
        _mockRepo = new Mock<IGrpcStocksRepository>();
        _mockMapper = new Mock<IStocksProtoMapper>();
        _service = new GrpcStocksService(_mockRepo.Object, _mockMapper.Object);
    }

    [Fact]
    public async Task GetStocks_Success_ReturnsMappedResponse()
    {
        // 1. Arrange
        var request = new GrpcModels.FiltersRequest { Page = 1 };
        
        // Use the alias 'StockEntity' instead of 'Stocks'
        var fakeEntities = new List<StockEntity> { new StockEntity() };
        int fakeCount = 1;
        var expectedResponse = new GrpcModels.StocksResponse { TotalCount = 1 };

        // Fixes the ReturnsAsync error by ensuring the Tuple matches the Repo interface exactly
        _mockRepo.Setup(r => r.GetStocksRepository(It.IsAny<FilterEntity>()))
                 .ReturnsAsync((fakeEntities, fakeCount));

        _mockMapper.Setup(m => m.MapToResponse(fakeEntities, fakeCount))
                   .Returns(expectedResponse);

        // 2. Act
        var result = await _service.GetStocks(request, null!);

        // 3. Assert
        Assert.NotNull(result);
        Assert.Equal(fakeCount, result.TotalCount);
        _mockRepo.Verify(r => r.GetStocksRepository(It.IsAny<FilterEntity>()), Times.Once);
    }
}