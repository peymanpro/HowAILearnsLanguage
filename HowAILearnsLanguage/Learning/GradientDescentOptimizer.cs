using HowAILearnsLanguage.Mathematics;

namespace HowAILearnsLanguage.Learning;

public sealed class GradientDescentOptimizer : IOptimizer
{
    public GradientDescentOptimizer(
        double learningRate)
    {
        if (!double.IsFinite(learningRate) ||
            learningRate <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(learningRate),
                "Learning rate must be a positive finite number.");
        }

        LearningRate = learningRate;
    }

    public double LearningRate { get; }

    public Vector Update(
        Vector parameters,
        Vector gradients)
    {
        ArgumentNullException.ThrowIfNull(parameters);
        ArgumentNullException.ThrowIfNull(gradients);

        if (parameters.Dimension != gradients.Dimension)
        {
            throw new ArgumentException(
                "Parameters and gradients must have the same dimension.");
        }

        return parameters.Subtract(
            gradients.Multiply(LearningRate));
    }
}
