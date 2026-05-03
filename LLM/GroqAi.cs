using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using AiAgent2.ChatFormat;

namespace AiAgent2.LLM;

public class GroqAi
{
    private readonly HttpClient _http;
    private readonly string _model;

    public GroqAi(string apiKey, string model)
    {
        _model = model;

        _http = new HttpClient();
        _http.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", apiKey);
    }

    public async Task<string> SendAsync(List<ChatMessage> messages)
    {
        var request = new ChatRequest(_model, messages);
        string json = JsonSerializer.Serialize(request);

        var response = await _http.PostAsync(
            "https://api.groq.com/openai/v1/chat/completions",
            new StringContent(json, Encoding.UTF8, "application/json")
        );

        string responseJson = await response.Content.ReadAsStringAsync();

        using var doc = JsonDocument.Parse(responseJson);
        var root = doc.RootElement;

        if (root.TryGetProperty("error", out var errorObj))
        {
            return "[Groq API Error] " + errorObj.GetProperty("message").GetString();
        }

        var choices = root.GetProperty("choices");
        return choices[0].GetProperty("message").GetProperty("content").GetString() ?? "";
    }
}
