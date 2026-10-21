using System.Text.RegularExpressions;
using CSharpRagAssistant.Models;

namespace CSharpRagAssistant.Retrieval;

public class KeywordRetriever
{
    private readonly List<DocumentChunk> _chunks;

    private static readonly HashSet<string> StopWords = new()
    {
        "the", "is", "a", "an", "and", "or",
        "to", "of", "in", "for", "on", "with",
        "what", "how", "can", "i", "my", "me",
        "does", "do", "about", "are", "our",
        "we", "your", "you", "it", "by", "from"
    };

    public KeywordRetriever(List<DocumentChunk> chunks)
    {
        _chunks = chunks;
    }

    private static HashSet<string> GetWords(string text)
    {
        return Regex.Matches(text.ToLowerInvariant(), @"[a-z0-9]+")
            .Select(m => m.Value)
            .Where(w => w.Length > 2 && !StopWords.Contains(w))
            .ToHashSet();
    }

    public List<DocumentChunk> Search(
        string question,
        int maxResults = 3)
    {
        var questionWords = GetWords(question);

        if (questionWords.Count == 0)
            return new List<DocumentChunk>();

        return _chunks
            .Select(chunk => new
            {
                Chunk = chunk,
                Score = CalculateScore(chunk, questionWords)
            })
            .Where(x => x.Score > 0)
            .OrderByDescending(x => x.Score)
            .Take(maxResults)
            .Select(x => x.Chunk)
            .ToList();
    }

    private static int CalculateScore(
        DocumentChunk chunk,
        HashSet<string> questionWords)
    {
        var titleWords = GetWords(chunk.Title);
        var contentWords = GetWords(chunk.Content);

        int score = 0;

        foreach (string word in questionWords)
        {
            if (titleWords.Contains(word))
                score += 3;

            if (contentWords.Contains(word))
                score += 1;
        }

        return score;
    }
}

