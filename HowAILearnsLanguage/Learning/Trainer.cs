using HowAILearnsLanguage.Domain;

namespace HowAILearnsLanguage.Learning;

public sealed class Trainer
{
    private readonly LanguageModel _model;
    private readonly ILossFunction _lossFunction;
    private readonly Backpropagator _backpropagator;
    private readonly IOptimizer _optimizer;

    public Trainer(
        LanguageModel model,
        ILossFunction lossFunction,
        Backpropagator backpropagator,
        IOptimizer optimizer)
    {
        _model = model ??
            throw new ArgumentNullException(nameof(model));

        _lossFunction = lossFunction ??
            throw new ArgumentNullException(nameof(lossFunction));

        _backpropagator = backpropagator ??
            throw new ArgumentNullException(nameof(backpropagator));

        _optimizer = optimizer ??
            throw new ArgumentNullException(nameof(optimizer));
    }

    public IReadOnlyList<TrainingResult> Train(
        IReadOnlyList<TrainingExample> examples,
        int epochs)
    {
        ArgumentNullException.ThrowIfNull(examples);

        if (examples.Count == 0)
        {
            throw new ArgumentException(
                "Training examples cannot be empty.",
                nameof(examples));
        }

        if (epochs <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(epochs));
        }

        var history =
            new List<TrainingResult>(epochs);

        for (var epoch = 1;
             epoch <= epochs;
             epoch++)
        {
            var totalLoss = 0.0;

            foreach (var example in examples)
            {
                var prediction =
                    _model.Predict(example);

                totalLoss +=
                    _lossFunction.Calculate(
                        prediction,
                        example.ContextTokenId);

                var gradients =
                    _backpropagator.Calculate(
                        _model,
                        example);

                _model.ApplyGradients(
                    gradients,
                    _optimizer);
            }

            history.Add(
                new TrainingResult(
                    epoch,
                    totalLoss / examples.Count));
        }

        return history;
    }
}
