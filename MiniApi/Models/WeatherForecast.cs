namespace MiniApi.Models;

public record WeatherForecast(DateTime Date, int TemperatureC, string? Summary)
{
    private static readonly string[] summaries = new[]{"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"};
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

    public static IEnumerable<WeatherForecast> GenerateWeatherReport()
    {
        return Enumerable.Range(1, 5).Select(index =>
            new WeatherForecast
            (
                DateTime.Now.AddDays(index),
                Random.Shared.Next(-20, 55),
                summaries[Random.Shared.Next(summaries.Length)]
            )
        ).ToArray();
    }
}