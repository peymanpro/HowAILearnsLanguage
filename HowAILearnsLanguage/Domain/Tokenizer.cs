using System.Text.RegularExpressions;

namespace HowAILearnsLanguage.Domain;

public sealed class Tokenizer
{
    private static readonly Regex TokenPattern =
        new(
            @"[A-Za-z]+",
            RegexOptions.Compiled);

    public IReadOnlyList<string> Tokenize(
        string sentence)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sentence);

        return TokenPattern
            .Matches(sentence.ToLowerInvariant())
            .Select(match => match.Value)
            .ToArray();
    }
}
