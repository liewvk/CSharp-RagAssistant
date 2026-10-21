using CSharpRagAssistant.Models;

namespace CSharpRagAssistant.Knowledge;

public static class KnowledgeBaseLoader
{
    public static List<DocumentChunk> Load(
        string folderPath,
        int maxWords = 120)
    {
        if (maxWords <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(maxWords));

        var chunks = new List<DocumentChunk>();

        if (!Directory.Exists(folderPath))
            return chunks;

        foreach (string file in Directory.GetFiles(
            folderPath, "*.txt"))
        {
            string text = File.ReadAllText(file);

            string[] words = text.Split(
                (char[]?)null,
                StringSplitOptions.RemoveEmptyEntries);

            string source = Path.GetFileName(file);
            string title =
                Path.GetFileNameWithoutExtension(file);

            int number = 1;

            for (int i = 0; i < words.Length; i += maxWords)
            {
                string content = string.Join(
                    " ",
                    words.Skip(i).Take(maxWords));

                chunks.Add(new DocumentChunk
                {
                    Source = source,
                    Title = $"{title} - Part {number}",
                    Content = content
                });

                number++;
            }
        }

        return chunks;
    }
}

