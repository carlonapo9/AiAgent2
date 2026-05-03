using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace AiAgent2.Tools;

public class SerperWebSearch
{
    private readonly HttpClient _http;
    private readonly string _apiKey;

    public SerperWebSearch()
    {
        _apiKey = Environment.GetEnvironmentVariable("Serper_Api_Key")
                  ?? throw new Exception("Serper_Api_Key environment variable not set.");

        _http = new HttpClient();
        _http.DefaultRequestHeaders.Add("X-API-KEY", _apiKey);
    }

    public async Task<string> SearchAsync(string query)
    {
        var body = new
        {
            q = query,
            gl = "uk",
            hl = "en"
        };

        string json = JsonSerializer.Serialize(body);

        var response = await _http.PostAsync(
            "https://google.serper.dev/search",
            new StringContent(json, Encoding.UTF8, "application/json")
        );

        return await response.Content.ReadAsStringAsync();
    }
}
