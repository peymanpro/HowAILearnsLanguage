namespace HowAILearnsLanguage.Domain;

public sealed class TrainingCorpus
{
    private readonly List<string> _sentences = [];

    public IReadOnlyList<string> Sentences =>
        _sentences.AsReadOnly();

    public int Count => _sentences.Count;

    public void Add(string sentence)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(sentence);

        _sentences.Add(sentence.Trim());
    }
}
