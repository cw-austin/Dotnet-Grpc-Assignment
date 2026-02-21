using GrpcService.Mappers;
using GrpcService.Repositories;
using GrpcService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddGrpc();

builder.Services.AddScoped<StocksProtoMapper>();
builder.Services.AddScoped<RepoMapper>();

builder.Services.AddScoped<IGrpcStocksRepository, GrpcStocksRepository>();
builder.Services.AddScoped<IGrpcCityRepository, GrpcCityRepository>();
builder.Services.AddScoped<IGrpcMakeRepository, GrpcMakeRepository>(); 

var app = builder.Build();


app.MapGrpcService<GrpcStocksService>();
app.MapGrpcService<GrpcCityService>();
app.MapGrpcService<GrpcMakeService>();

app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");

app.Run();