using SampleClient;
using System.Text.Json;
using System.Collections.Generic;

public class Program
{
    public static async Task Main(string[] args)
    {
        var _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri("https://localhost:7155/");
        
        var _options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

        using (var response = await _httpClient.GetAsync("WeatherForecast", HttpCompletionOption.ResponseHeadersRead).ConfigureAwait(false))
        {
            response.EnsureSuccessStatusCode();
            var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false); ;

            IAsyncEnumerable<WeatherForecast> weatherForecasts = JsonSerializer.DeserializeAsyncEnumerable<WeatherForecast>(
            stream,
            new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultBufferSize = 128
            });

            await foreach (WeatherForecast weatherForecast in weatherForecasts)
            {
                System.Console.WriteLine(weatherForecast?.Summary);
            }

        }

    }
}


