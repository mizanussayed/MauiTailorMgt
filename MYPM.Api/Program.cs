using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using MYPM.Data.Configurations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

var connectionString = "";
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.MigrationsHistoryTable(HistoryRepository.DefaultTableName, "Public")
           .EnableRetryOnFailure();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/weatherforecast", () =>
{
    return "Hello World!";
})
.WithName("GetWeatherForecast");

app.Run();
