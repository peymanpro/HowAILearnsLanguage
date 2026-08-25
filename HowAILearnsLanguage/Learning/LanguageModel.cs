using HowAILearnsLanguage.Domain;
using HowAILearnsLanguage.Mathematics;


namespace HowAILearnsLanguage.Learning;

public sealed class LanguageModel
{
    private readonly Vocabulary _vocabulary;
    private readonly Vector[] _inputEmbeddings;
    private readonly Vector[] _outputEmbeddings;

    public LanguageModel(
        Vocabulary vocabulary,
        int embeddingDimension,
        int seed = 42)
    {
        ArgumentNullException.ThrowIfNull(vocabulary);

        if (vocabulary.Count == 0)
        {
            throw new ArgumentException(
                "Vocabulary cannot be empty.",
                nameof(vocabulary));
        }

        if (embeddingDimension <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(embeddingDimension));
        }

        _vocabulary = vocabulary;
        _inputEmbeddings = CreateEmbeddings(
            vocabulary.Count,
            embeddingDimension,
            seed);

        _outputEmbeddings = CreateEmbeddings(
            vocabulary.Count,
            embeddingDimension,
            seed + 1);
    }

    public int VocabularySize => _vocabulary.Count;

    public int EmbeddingDimension =>
        _inputEmbeddings[0].Dimension;

    public Prediction Predict(
        TrainingExample example)
    {
        ArgumentNullException.ThrowIfNull(example);

        ValidateTokenId(example.TargetTokenId);
        ValidateTokenId(example.ContextTokenId);

        var inputVector =
            _inputEmbeddings[example.TargetTokenId];

        var scores = new double[_vocabulary.Count];

        for (var tokenId = 0;
             tokenId < _vocabulary.Count;
             tokenId++)
        {
            scores[tokenId] =
                inputVector.Dot(
                    _outputEmbeddings[tokenId]);
        }

        return new Prediction(Softmax(scores));
    }

    private void ValidateTokenId(int tokenId)
    {
        if ((uint)tokenId >= (uint)_vocabulary.Count)
        {
            throw new ArgumentOutOfRangeException(
                nameof(tokenId));
        }
    }

    private static Vector[] CreateEmbeddings(
        int count,
        int dimension,
        int seed)
    {
        var random = new Random(seed);
        var embeddings = new Vector[count];

        var scale =
            1.0 / Math.Sqrt(dimension);

        for (var i = 0; i < count; i++)
        {
            var values = new double[dimension];

            for (var j = 0; j < dimension; j++)
            {
                values[j] =
                    (random.NextDouble() * 2.0 - 1.0)
                    * scale;
            }

            embeddings[i] = new Vector(values);
        }

        return embeddings;
    }

    private static double[] Softmax(
        IReadOnlyList<double> scores)
    {
        var max = scores.Max();

        var exponentials = new double[scores.Count];
        var sum = 0.0;

        for (var i = 0; i < scores.Count; i++)
        {
            var value =
                Math.Exp(scores[i] - max);

            exponentials[i] = value;
            sum += value;
        }

        for (var i = 0; i < exponentials.Length; i++)
        {
            exponentials[i] /= sum;
        }

        return exponentials;
    }
}
