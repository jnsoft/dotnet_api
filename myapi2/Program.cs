using myapi2;
using myapi2.Authentication;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(/*x => x.Filters.Add<ApiKeyAuthFilter>()*/); // method 2 (filter): apply to every controller

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ApiKeyAuthFilter>(); // method 3: allow using filters on controllers/methods

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// app.UseMiddleware<ApiKeyAuthMiddleware>(); // method 1 (middleware): use to apply apikey to every method (works with both controllers and minimal API)



app.UseAuthorization();

app.MapControllers();

// using minimal API
app.MapGet("/miniget", () =>
{
    var summaries = new[]{"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"};

    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        { 
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = summaries[Random.Shared.Next(summaries.Length)]
        })
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecastMini");

app.Run();
