using IdolFinder.API.Extensions;
using IdolFinder.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.RegisterApplicationLayers();
builder.Services.ConfigureSwagger();
builder.Services.ConfigureCorsAllowAny();

builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
