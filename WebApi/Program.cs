using Stocks;
using WebApi.Mappers;
using WebApi.Repositories;
using WebApi.Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpcClient<StockService.StockServiceClient>(options =>
{
    options.Address = new Uri(
        builder.Configuration["GrpcServer:Address"]!);
});
builder.Services.AddSingleton<StockRawDataMapper>();
builder.Services.AddSingleton<StocksDtoMapper>();
builder.Services.AddSingleton<FiltersMapper>();
builder.Services.AddSingleton<MetadataMapper>();
builder.Services.AddSingleton<FiltersRequestMapper>();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<IStocksService, StocksService>();
builder.Services.AddScoped<IStocksRepository, StocksRepository>();
builder.Services.AddSwaggerGen();
var myAllowSpecificOrigins = "_myAllowSpecificOrigins";

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowSpecificOrigins,
        policy =>
        {
            policy.WithOrigins("http://localhost:3000" // Your Frontend URL
                                ) //"https://yourdomain.com" add more allowed domains if needed
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
builder.Services.AddControllers()
    .ConfigureApiBehaviorOptions(options =>
    {
        // This ensures that model state errors return a 400 automatically
        options.SuppressModelStateInvalidFilter = false;
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("_myAllowSpecificOrigins");
app.UseAuthorization();
app.MapControllers();

app.Run();