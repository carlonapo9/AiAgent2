using AiAgent2.LLM;
using AiAgent2.Memory;
using AiAgent2.Tools;

string? apiKey = Environment.GetEnvironmentVariable("GROQ_API_KEY");

if (string.IsNullOrWhiteSpace(apiKey))
{
    Console.WriteLine("ERROR: GROQ_API_KEY environment variable is not set.");
    return;
}

const string MODEL = "llama-3.1-8b-instant";

var llm = new GroqAi(apiKey, MODEL);
var memory = new ChatMemory();
var serper = new SerperWebSearch();

Console.WriteLine("AI Chat started. Type 'exit' to quit.");

while (true)
{
    Console.Write("\nYou: ");
    string userInput = Console.ReadLine() ?? "";

    if (userInput.ToLower() == "exit")
        break;

    // STEP 1 — Ask Groq if search is needed
    string checkPrompt =
        $"User asked: \"{userInput}\".\n" +
        "Does this require a websearch? Answer only YES or NO.";

    string check = await llm.SendAsync(new()
    {
        new("system", "You are a classifier that answers only YES or NO."),
        new("user", checkPrompt)
    });

    bool requiresSearch = check.Trim().StartsWith("Y", StringComparison.OrdinalIgnoreCase);

    if (requiresSearch)
    {
        Console.WriteLine("\n[Groq decided: Web search needed…]");

        // STEP 2 — Call Serper
        string searchJson = await serper.SearchAsync(userInput);

        // STEP 3 — Ask Groq to summarise the JSON (NOT stored in memory)
        string summaryPrompt =
            $"Summarise the following Serper JSON into short bullet points. " +
            $"Extract titles, snippets, and key facts only. Avoid hallucinating.\n\n{searchJson}";

        string summary = await llm.SendAsync(new()
        {
            new("system", "You summarise search results into clean bullet points."),
            new("user", summaryPrompt)
        });

        // STEP 4 — Now ask Groq to answer using the summary
        string finalPrompt =
            $"Current date/time: {DateTime.Now:dddd, dd MMMM yyyy HH:mm}\n" +
            $"User question: {userInput}\n\n" +
            $"Search summary:\n{summary}\n\n" +
            $"Answer the user's question using ONLY the summary above.";

        memory.AddUser(finalPrompt);

        string reply = await llm.SendAsync(memory.GetMessages());
        memory.AddAssistant(reply);

        Console.WriteLine($"\nAI: {reply}");
        continue;
    }

    // STEP 5 — Normal conversation
    memory.AddUser(userInput);

    string normalReply = await llm.SendAsync(memory.GetMessages());
    memory.AddAssistant(normalReply);

    Console.WriteLine($"\nAI: {normalReply}");
}
