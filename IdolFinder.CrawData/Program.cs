using IdolFinder.CrawData.Infrastructure.Data;
using IdolFinder.CrawData.Infrastructure.Storage;
using IdolFinder.CrawData.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IStorageService>(provider =>
{
    var config = provider.GetRequiredService<IConfiguration>();
    var storageType = config["Storage:Type"] ?? "Local";

    return storageType.ToLower() switch
    {
        "blob" => new BlobStorageService(config),
        _ => new LocalStorageService(config, provider.GetRequiredService<ILogger<LocalStorageService>>())
    };
});

builder.Services.AddHttpClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
