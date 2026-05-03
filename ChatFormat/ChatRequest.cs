using System.Collections.Generic;

namespace AiAgent2.ChatFormat;

public class ChatRequest
{
    public string model { get; set; }
    public List<ChatMessage> messages { get; set; }

    public ChatRequest(string model, List<ChatMessage> messages)
    {
        this.model = model;
        this.messages = messages;
    }
}
