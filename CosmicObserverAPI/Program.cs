using Microsoft.EntityFrameworkCore;
using CosmicObserverAPI.Data;
using CosmicObserverAPI.Configuration;

var builder = WebApplication.CreateBuilder(args);

// Get the connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string" + "\"DefaultConnection\" not found.");

// Register data context and configure it to SQLite
builder.Services.AddDbContext<CosmicDbContext>(options => options.UseSqlite(connectionString));

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Bind NASA API options to the dependency injection service container
builder.Services.Configure<NasaApiOptions>(
    builder.Configuration.GetSection(NasaApiOptions.SectionName));

var app = builder.Build();

/*-----------------------------*/

//Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
