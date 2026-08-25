namespace HowAILearnsLanguage.Domain;

public sealed record TrainingExample
{
    public TrainingExample(
        int targetTokenId,
        int contextTokenId)
    {
        if (targetTokenId < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(targetTokenId),
                "Target token ID must be non-negative.");
        }

        if (contextTokenId < 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(contextTokenId),
                "Context token ID must be non-negative.");
        }

        TargetTokenId = targetTokenId;
        ContextTokenId = contextTokenId;
    }

    public int TargetTokenId { get; }

    public int ContextTokenId { get; }
}
