namespace HowAILearnsLanguage.Learning;

public sealed class CrossEntropyLoss : ILossFunction
{
    private const double Epsilon = 1e-12;

    public double Calculate(
        Prediction prediction,
        int targetTokenId)
    {
        ArgumentNullException.ThrowIfNull(prediction);

        if ((uint)targetTokenId >=
            (uint)prediction.Probabilities.Count)
        {
            throw new ArgumentOutOfRangeException(
                nameof(targetTokenId));
        }

        var probability =
            prediction.Probabilities[targetTokenId];

        probability = Math.Clamp(
            probability,
            Epsilon,
            1.0);

        return -Math.Log(probability);
    }
}
