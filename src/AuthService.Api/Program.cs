using AuthService.Persistence;  // Importa el espacio de nombres de ApplicationDbContext
using AuthService.Application;  // Si DataSeeder está aquí, importa este espacio de nombres

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[] {
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
.WithOpenApi();

app.Run();

// INICIALIZACIÓN DE LA BASE DE DATOS
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    try
    {
        logger.LogInformation("Iniciando migración de la base de datos ...");

        await context.Database.EnsureCreatedAsync();

        logger.LogInformation("Migración completada exitosamente");
        await DataSeeder.SeedAsync(context);
        logger.LogInformation("Datos iniciales cargados exitosamente");
    }
    catch (Exception es)
    {
        logger.LogError(es, "Error al inicializar la base de datos");
        throw; // Detener la aplicación si hay un error al inicializar la base de datos
    }
}

// Definición del record WeatherForecast
record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}