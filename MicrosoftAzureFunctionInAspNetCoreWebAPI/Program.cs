using MicrosoftAzureFunctionInAspNetCoreWebAPI_BAL.DataFetcherService;
using MicrosoftAzureFunctionInAspNetCoreWebAPI_BAL.IDataService;
using MicrosoftAzureFunctionInAspNetCoreWebAPI_DAL.ApplicationDbContext;
using MicrosoftAzureFunctionInAspNetCoreWebAPI_DAL.IRepository;
using MicrosoftAzureFunctionInAspNetCoreWebAPI_DAL.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// Add services to the container.
builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<IDataFetcherService, DataFetcherService>();
builder.Services.AddHttpClient<IDataFetcher, RepositoryDataFetcher>();
builder.Services.AddScoped<IDataFetcher, RepositoryDataFetcher>();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Microservices Development using Microsoft Azure Functions", Version = "v1" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if(app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
