using okta_core.Models;
using System.Text.Json;

namespace okta_blazor_server_example.Services
{
    public class WeatherForecastApiService
    {
        private readonly HttpClient _http;
        private readonly TokenProvider _tokenProvider;
        private readonly IConfiguration _configuration;

        public WeatherForecastApiService(IHttpClientFactory clientFactory,
            TokenProvider tokenProvider, IConfiguration configuration)
        {
            _http = clientFactory.CreateClient();
            _tokenProvider = tokenProvider;
            _configuration = configuration;
        }

        public async Task<WeatherForecast[]> GetForecastAsync()
        {
            var forecasts = Array.Empty<WeatherForecast>();

            var token = _tokenProvider.AccessToken;
            var request = new HttpRequestMessage(HttpMethod.Get, $"{_configuration.GetValue<string>("WebApi:Domain")}/WeatherForecast");
            request.Headers.Add("Authorization", $"Bearer {token}");
            var response = await _http.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            if (!string.IsNullOrEmpty(json)) 
                forecasts = JsonSerializer.Deserialize<WeatherForecast[]>(json, options);

            return forecasts;
        }
    }
}
