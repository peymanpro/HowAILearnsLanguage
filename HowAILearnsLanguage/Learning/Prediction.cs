namespace HowAILearnsLanguage.Learning;

public sealed record Prediction(
    IReadOnlyList<double> Probabilities)
{
    public int PredictedTokenId
    {
        get
        {
            var bestIndex = 0;
            var bestValue = Probabilities[0];

            for (var i = 1; i < Probabilities.Count; i++)
            {
                if (Probabilities[i] > bestValue)
                {
                    bestValue = Probabilities[i];
                    bestIndex = i;
                }
            }

            return bestIndex;
        }
    }
}
