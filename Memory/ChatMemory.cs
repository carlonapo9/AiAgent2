using AiAgent2.ChatFormat;

namespace AiAgent2.Memory;

public class ChatMemory
{
    private readonly List<ChatMessage> _messages = new();

    public ChatMemory()
    {
        _messages.Add(new ChatMessage("system", "You are a helpful AI assistant."));
    }

    public void AddUser(string text)
    {
        _messages.Add(new ChatMessage("user", text));
    }

    public void AddAssistant(string text)
    {
        _messages.Add(new ChatMessage("assistant", text));
    }

    public List<ChatMessage> GetMessages()
    {
        return _messages;
    }
}
