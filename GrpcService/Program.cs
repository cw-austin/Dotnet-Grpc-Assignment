using GrpcService.Services;
using GrpcService.Repositories;
using GrpcService.Mappers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddGrpc();
builder.Services.AddScoped<IGrpcStocksRepository, GrpcStocksRepository>();
builder.Services.AddScoped<IGrpcStocksService, GrpcStocksService>();
builder.Services.AddScoped<StocksProtoMapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.MapGrpcService<GrpcStocksService>();
app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client. To learn how to create a client, visit: https://go.microsoft.com/fwlink/?linkid=2086909");

app.Run();
