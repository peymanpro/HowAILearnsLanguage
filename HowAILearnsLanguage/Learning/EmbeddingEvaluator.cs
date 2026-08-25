using HowAILearnsLanguage.Domain;

namespace HowAILearnsLanguage.Learning;

public sealed class EmbeddingEvaluator
{
    private readonly Vocabulary _vocabulary;

    public EmbeddingEvaluator(
        Vocabulary vocabulary)
    {
        _vocabulary = vocabulary ??
            throw new ArgumentNullException(nameof(vocabulary));
    }

    public EmbeddingSimilarityComparison Compare(
        EmbeddingSnapshot beforeTraining,
        LanguageModel afterTraining,
        string firstToken,
        string secondToken)
    {
        ArgumentNullException.ThrowIfNull(beforeTraining);
        ArgumentNullException.ThrowIfNull(afterTraining);
        ArgumentException.ThrowIfNullOrWhiteSpace(firstToken);
        ArgumentException.ThrowIfNullOrWhiteSpace(secondToken);

        var firstId =
            _vocabulary.GetId(firstToken);

        var secondId =
            _vocabulary.GetId(secondToken);

        var beforeSimilarity =
            beforeTraining
                .Get(firstId)
                .CosineSimilarity(
                    beforeTraining.Get(secondId));

        var afterSimilarity =
            afterTraining
                .GetInputEmbedding(firstId)
                .CosineSimilarity(
                    afterTraining.GetInputEmbedding(secondId));

        return new EmbeddingSimilarityComparison(
            firstToken.Trim().ToLowerInvariant(),
            secondToken.Trim().ToLowerInvariant(),
            beforeSimilarity,
            afterSimilarity);
    }
}
