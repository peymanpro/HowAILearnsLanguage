using HowAILearnsLanguage.Mathematics;

namespace HowAILearnsLanguage.Learning;

public sealed class EmbeddingSnapshot
{
    private readonly IReadOnlyDictionary<int, Vector> _embeddings;

    private EmbeddingSnapshot(
        IReadOnlyDictionary<int, Vector> embeddings)
    {
        _embeddings = embeddings;
    }

    public static EmbeddingSnapshot Capture(
        LanguageModel model)
    {
        ArgumentNullException.ThrowIfNull(model);

        var embeddings =
            new Dictionary<int, Vector>();

        for (var tokenId = 0;
             tokenId < model.VocabularySize;
             tokenId++)
        {
            embeddings[tokenId] =
                new Vector(
                    model.GetInputEmbedding(tokenId)
                        .ToArray());
        }

        return new EmbeddingSnapshot(
            embeddings);
    }

    public Vector Get(int tokenId)
    {
        if (!_embeddings.TryGetValue(
                tokenId,
                out var embedding))
        {
            throw new ArgumentOutOfRangeException(
                nameof(tokenId),
                tokenId,
                "Embedding does not exist in the snapshot.");
        }

        return embedding;
    }
}
