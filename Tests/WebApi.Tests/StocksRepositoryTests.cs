// using Moq;
// using Xunit;
// using Grpc.Core;
// using Grpc.Core.Testing;
// using WebApi.Repository.Repositories;
// using WebApi.Mappers;
// using Stocks; 
// using Contracts.Entities;
// using WebApi.Domain.Entities.StocksRawData;
// using System;
// using System.Collections.Generic;
// using System.Threading;
// using System.Threading.Tasks;

// namespace WebApi.Tests.Repositories;

// public class StocksRepositoryTests
// {
//     private readonly Mock<StockService.StockServiceClient> _mockClient;
//     private readonly Mock<FiltersRequestMapper> _mockFiltersMapper;
//     private readonly Mock<StockRawDataMapper> _mockStockMapper;
//     private readonly StocksRepository _repository;

//     public StocksRepositoryTests()
//     {
//         _mockClient = new Mock<StockService.StockServiceClient>();
//         _mockFiltersMapper = new Mock<FiltersRequestMapper>();
//         _mockStockMapper = new Mock<StockRawDataMapper>();
        
//         _repository = new StocksRepository(
//             _mockClient.Object, 
//             _mockFiltersMapper.Object, 
//             _mockStockMapper.Object);
//     }

//     [Fact]
//     public async Task GetStocksAsync_Success_ReturnsMappedData()
//     {
//         // Arrange
//         var domainFilters = new Filters();
//         var grpcRequest = new FiltersRequest(); 
//         var grpcResponse = new StocksResponse { TotalCount = 50 }; 
//         grpcResponse.Stocks.Add(new Stock { ProfileId = "ABC-123", Price = 10000 }); 

//         var expectedEntities = new List<StockRawData> { new StockRawData { ProfileId = "ABC-123" } };

//         _mockFiltersMapper.Setup(m => m.ToGrpc(domainFilters)).Returns(grpcRequest);
//         _mockStockMapper.Setup(m => m.Map(It.IsAny<List<Stock>>())).Returns(expectedEntities);

//         // FIX: Explicitly specify <StocksResponse>
//         var call = TestCalls.AsyncUnaryCall<StocksResponse>(
//             Task.FromResult(grpcResponse), 
//             Task.FromResult(new Metadata()), 
//             () => Status.DefaultSuccess, 
//             () => new Metadata(), 
//             () => { });

//         _mockClient.Setup(c => c.GetStocksAsync(grpcRequest, null, null, default))
//                    .Returns(call);

//         // Act
//         var (stocks, totalCount) = await _repository.GetStocksAsync(domainFilters);

//         // Assert
//         Assert.Equal(50, totalCount); 
//         Assert.Single(stocks);
//         _mockClient.Verify(c => c.GetStocksAsync(grpcRequest, null, null, default), Times.Once);
//     }

//     [Fact]
//     public async Task GetStocksAsync_EmptyResponse_ReturnsEmptyList()
//     {
//         // Arrange
//         var domainFilters = new Filters();
//         var grpcResponse = new StocksResponse { TotalCount = 0 };

//         _mockFiltersMapper.Setup(m => m.ToGrpc(domainFilters)).Returns(new FiltersRequest());
//         _mockStockMapper.Setup(m => m.Map(It.IsAny<List<Stock>>())).Returns(new List<StockRawData>());

//         // FIX: Explicitly specify <StocksResponse>
//         var call = TestCalls.AsyncUnaryCall<StocksResponse>(
//             Task.FromResult(grpcResponse), 
//             Task.FromResult(new Metadata()), 
//             () => Status.DefaultSuccess, 
//             () => new Metadata(), 
//             () => { });

//         _mockClient.Setup(c => c.GetStocksAsync(It.IsAny<FiltersRequest>(), null, null, default))
//                    .Returns(call);

//         // Act
//         var (stocks, totalCount) = await _repository.GetStocksAsync(domainFilters);

//         // Assert
//         Assert.Empty(stocks);
//         Assert.Equal(0, totalCount);
//     }

//     [Fact]
//     public async Task GetStocksAsync_OnRpcException_Rethrows()
//     {
//         // Arrange
//         var domainFilters = new Filters();
//         var rpcException = new RpcException(new Status(StatusCode.Unavailable, "Service Unavailable"));
        
//         _mockFiltersMapper.Setup(m => m.ToGrpc(domainFilters)).Returns(new FiltersRequest());
//         _mockClient.Setup(c => c.GetStocksAsync(It.IsAny<FiltersRequest>(), null, null, It.IsAny<CancellationToken>()))
//                    .Throws(rpcException);

//         // Act & Assert
//         var ex = await Assert.ThrowsAsync<RpcException>(() => _repository.GetStocksAsync(domainFilters));
//         Assert.Equal(StatusCode.Unavailable, ex.StatusCode);
//     }

//     [Fact]
//     public async Task GetStocksAsync_OnGeneralException_Rethrows()
//     {
//         // Arrange
//         var domainFilters = new Filters();
//         _mockFiltersMapper.Setup(m => m.ToGrpc(domainFilters)).Throws(new Exception("Unknown Error"));

//         // Act & Assert
//         await Assert.ThrowsAsync<Exception>(() => _repository.GetStocksAsync(domainFilters));
//     }

//     [Fact]
//     public async Task GetStocksAsync_NullFilters_ThrowsArgumentNullException()
//     {
//         // Act & Assert
//         await Assert.ThrowsAsync<ArgumentNullException>(() => _repository.GetStocksAsync(null!));
//     }
// }