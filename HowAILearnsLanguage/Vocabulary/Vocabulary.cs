namespace HowAILearnsLanguage.Domain;

public sealed class Vocabulary
{
    private readonly Dictionary<string, int> _tokenToId =
        new(StringComparer.OrdinalIgnoreCase);

    private readonly List<string> _tokens = [];

    public int Count => _tokens.Count;

    public int Add(string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        var normalized = token.Trim().ToLowerInvariant();

        if (_tokenToId.TryGetValue(
                normalized,
                out var existingId))
        {
            return existingId;
        }

        var id = _tokens.Count;

        _tokens.Add(normalized);
        _tokenToId.Add(normalized, id);

        return id;
    }

    public bool Contains(string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        return _tokenToId.ContainsKey(
            token.Trim().ToLowerInvariant());
    }

    public int GetId(string token)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(token);

        var normalized = token.Trim().ToLowerInvariant();

        if (!_tokenToId.TryGetValue(
                normalized,
                out var id))
        {
            throw new KeyNotFoundException(
                $"Token '{normalized}' does not exist in the vocabulary.");
        }

        return id;
    }

    public string GetToken(int id)
    {
        if ((uint)id >= (uint)_tokens.Count)
        {
            throw new ArgumentOutOfRangeException(nameof(id));
        }

        return _tokens[id];
    }

    public IReadOnlyList<string> Tokens =>
        _tokens.AsReadOnly();
}
