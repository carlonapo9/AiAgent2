using AiAgent2.ChatFormat;

namespace AiAgent2.Memory;

public class ChatMemory
{
    private readonly List<ChatMessage> _messages = new();

    private const int MaxMessages = 10; // keep memory small

    public ChatMemory()
    {
        _messages.Add(new ChatMessage("system",
            "You are a helpful AI assistant. Keep answers concise and avoid hallucinations."));
    }

    public void AddUser(string text)
    {
        _messages.Add(new ChatMessage("user", text));
        Trim();
    }

    public void AddAssistant(string text)
    {
        _messages.Add(new ChatMessage("assistant", text));
        Trim();
    }

    public List<ChatMessage> GetMessages()
    {
        return _messages;
    }

    private void Trim()
    {
        // Keep system message + last N messages
        while (_messages.Count > MaxMessages + 1)
            _messages.RemoveAt(1);
    }
}
