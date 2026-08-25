using HowAILearnsLanguage.Mathematics;

namespace HowAILearnsLanguage.Learning;

public sealed record LanguageGradients(
    int TargetTokenId,
    int ContextTokenId,
    Vector InputEmbeddingGradient,
    IReadOnlyList<Vector> OutputEmbeddingGradients);
