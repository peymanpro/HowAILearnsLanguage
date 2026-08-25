using System.Text.Json;

namespace HowAILearnsLanguage.Domain;

public sealed class TrainingData
{
    public TrainingData(
        IReadOnlyList<string> vocabularyTokens,
        IReadOnlyList<string> sentences)
    {
        ArgumentNullException.ThrowIfNull(vocabularyTokens);
        ArgumentNullException.ThrowIfNull(sentences);

        VocabularyTokens = vocabularyTokens;
        Sentences = sentences;
    }

    public IReadOnlyList<string> VocabularyTokens { get; }

    public IReadOnlyList<string> Sentences { get; }

    public static TrainingData Load(
        string vocabularyPath,
        string corpusPath)
    {
        var vocabularyJson =
            File.ReadAllText(vocabularyPath);

        var corpusJson =
            File.ReadAllText(corpusPath);

        var vocabulary =
            JsonSerializer.Deserialize<List<string>>(
                vocabularyJson)
            ?? throw new InvalidOperationException(
                "Vocabulary data could not be loaded.");

        var sentences =
            JsonSerializer.Deserialize<List<string>>(
                corpusJson)
            ?? throw new InvalidOperationException(
                "Corpus data could not be loaded.");

        return new TrainingData(
            vocabulary,
            sentences);
    }
}
