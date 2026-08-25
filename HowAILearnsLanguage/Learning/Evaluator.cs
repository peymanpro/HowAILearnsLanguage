using HowAILearnsLanguage.Domain;

namespace HowAILearnsLanguage.Learning;

public sealed class Evaluator
{
    private readonly LanguageModel _model;
    private readonly ILossFunction _lossFunction;

    public Evaluator(
        LanguageModel model,
        ILossFunction lossFunction)
    {
        _model = model ??
            throw new ArgumentNullException(nameof(model));

        _lossFunction = lossFunction ??
            throw new ArgumentNullException(nameof(lossFunction));
    }

    public EvaluationResult Evaluate(
        IReadOnlyList<TrainingExample> examples)
    {
        ArgumentNullException.ThrowIfNull(examples);

        if (examples.Count == 0)
        {
            throw new ArgumentException(
                "Evaluation examples cannot be empty.",
                nameof(examples));
        }

        var totalLoss = 0.0;
        var correct = 0;

        foreach (var example in examples)
        {
            var prediction =
                _model.Predict(example);

            totalLoss +=
                _lossFunction.Calculate(
                    prediction,
                    example.ContextTokenId);

            if (prediction.PredictedTokenId ==
                example.ContextTokenId)
            {
                correct++;
            }
        }

        return new EvaluationResult(
            AverageLoss: totalLoss / examples.Count,
            Accuracy: (double)correct / examples.Count,
            CorrectPredictions: correct,
            TotalExamples: examples.Count);
    }
}
