using AiAgent2.LLM;
using AiAgent2.Memory;

string? apiKey = Environment.GetEnvironmentVariable("GROQ_API_KEY");

if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.WriteLine("ERROR: GROQ_API_KEY environment variable is not set.");
    return;
}

const string MODEL = "llama-3.1-8b-instant";

var llm = new GroqAi(apiKey, MODEL);
var memory = new ChatMemory();

Console.WriteLine("AI Chat started. Type 'exit' to quit.");

while (true)
{
    Console.Write("\nYou: ");
    string userInput = Console.ReadLine() ?? "";

    if (userInput.ToLower() == "exit")
        break;

    memory.AddUser(userInput);

    string reply = await llm.SendAsync(memory.GetMessages());

    memory.AddAssistant(reply);

    Console.WriteLine($"\nAI: {reply}");
}
