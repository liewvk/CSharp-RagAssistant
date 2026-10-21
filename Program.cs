using CSharpRagAssistant.Knowledge;
using CSharpRagAssistant.Retrieval;
using CSharpRagAssistant.Services;

Console.Title = "C# RAG Knowledge Assistant";

// Locate the copied Documents folder.
string documentsFolder = Path.Combine(
    AppContext.BaseDirectory,
    "Documents");

// Load the knowledge base.
var chunks = KnowledgeBaseLoader.Load(documentsFolder);

var retriever = new KeywordRetriever(chunks);

using var aiService = new OllamaRagService();

Console.WriteLine("==================================");
Console.WriteLine("       C# RAG KNOWLEDGE ASSISTANT");
Console.WriteLine("       Visual Studio 2026");
Console.WriteLine("       Ollama + Qwen 3.5");
Console.WriteLine("==================================");

Console.WriteLine(
    $"Loaded {chunks.Count} document chunks.");

Console.WriteLine(
    "Ask about refund, shipping, warranty, or support.");

Console.WriteLine("Commands: reload, exit");

while (true)
{
    Console.WriteLine();
    Console.Write("Question: ");

    string? question = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(question))
        continue;

    if (question.Equals(
        "exit", StringComparison.OrdinalIgnoreCase))
        break;

    if (question.Equals(
        "reload", StringComparison.OrdinalIgnoreCase))
    {
        try
        {
            chunks = KnowledgeBaseLoader.Load(documentsFolder);
            retriever = new KeywordRetriever(chunks);

            Console.WriteLine(
                $"Reloaded {chunks.Count} document chunks.");
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                "Could not reload documents: " + ex.Message);
        }

        continue;
    }

    try
    {
        // Retrieve relevant document chunks.
        var relevantChunks = retriever.Search(
            question,
            maxResults: 3);

        if (relevantChunks.Count == 0)
        {
            Console.WriteLine(
                "No matching information was found.");
            continue;
        }

        Console.WriteLine();
        Console.WriteLine("Retrieved Sources:");

        foreach (var chunk in relevantChunks)
        {
            Console.WriteLine(
                $"- {chunk.Source}: {chunk.Title}");
        }

        Console.WriteLine();
        Console.WriteLine("Qwen is preparing an answer...");

        string answer = await aiService.AnswerAsync(
            question, relevantChunks);

        Console.WriteLine();
        Console.WriteLine("Assistant:");
        Console.WriteLine(answer);
    }
    catch (HttpRequestException ex)
    {
        Console.WriteLine(
            "Ollama API error: " + ex.Message);
    }
    catch (OperationCanceledException)
    {
        Console.WriteLine(
            "The AI request timed out.");
    }
    catch (Exception ex)
    {
        Console.WriteLine(
            "Error: " + ex.Message);
    }
}

Console.WriteLine("Goodbye!");

