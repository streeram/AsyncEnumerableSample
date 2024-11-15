using SampleClient;
using System.Text.Json;

public static class Program
{
    public static async Task Main(string[] args)
    {
        using var httpClient = new HttpClient();
        httpClient.BaseAddress = new Uri("https://localhost:7155/");

        using var response = await httpClient.GetAsync("WeatherForecast", HttpCompletionOption.ResponseHeadersRead)
            .ConfigureAwait(false);
        response.EnsureSuccessStatusCode();
        var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
        ;

        var weatherForecasts =
            JsonSerializer.DeserializeAsyncEnumerable<WeatherForecast>(
                stream,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                    DefaultBufferSize = 128
                });

        await foreach (WeatherForecast? weatherForecast in weatherForecasts)
        {
            Console.WriteLine(weatherForecast?.Summary);
        }
    }
}