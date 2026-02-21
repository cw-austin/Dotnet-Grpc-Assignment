using WebApi.Repository.Repositories; // For StocksRepository, CityRepository, etc.
using Stocks;
using WebApi.Mappers;
using WebApi.WebApi.Repository.Repositories;
using WebApi.Services;

var builder = WebApplication.CreateBuilder(args);

var grpcUrl = builder.Configuration["GrpcSettings:Url"] 
              ?? throw new InvalidOperationException("gRPC URL 'GrpcSettings:Url' is missing in appsettings.json");

builder.Services.AddGrpcClient<StockService.StockServiceClient>(o => o.Address = new Uri(grpcUrl));
builder.Services.AddGrpcClient<CitiesService.CitiesServiceClient>(o => o.Address = new Uri(grpcUrl));
builder.Services.AddGrpcClient<MakesService.MakesServiceClient>(o => o.Address = new Uri(grpcUrl));

builder.Services.AddSingleton<StockRawDataMapper>();
builder.Services.AddSingleton<StocksDtoMapper>();
builder.Services.AddSingleton<FiltersMapper>();
builder.Services.AddSingleton<MetadataMapper>();
builder.Services.AddSingleton<FiltersRequestMapper>();

builder.Services.AddScoped<IStocksRepository, StocksRepository>();
builder.Services.AddScoped<ICityRepository, CityRepository>(); // Added
builder.Services.AddScoped<IMakeRepository, MakeRepository>(); // Added

builder.Services.AddScoped<IStocksService, StocksService>();
builder.Services.AddScoped<ICityService, CityService>();
builder.Services.AddScoped<IMakeService, MakeService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

var myAllowSpecificOrigins = "_myAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000") 
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});

var app = builder.Build();

// --- 6. Middleware Pipeline ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(myAllowSpecificOrigins);
app.UseAuthorization();
app.MapControllers();

app.Run();