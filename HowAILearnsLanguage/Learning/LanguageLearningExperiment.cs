using HowAILearnsLanguage.Domain;

namespace HowAILearnsLanguage.Learning;

public sealed record LanguageLearningResult(
    LanguageModel Model,
    EmbeddingSnapshot InitialEmbeddings,
    IReadOnlyList<TrainingResult> History);

public sealed class LanguageLearningExperiment
{
    private readonly Vocabulary _vocabulary;
    private readonly TrainingCorpus _corpus;

    public LanguageLearningExperiment(
        Vocabulary vocabulary,
        TrainingCorpus corpus)
    {
        _vocabulary = vocabulary ??
            throw new ArgumentNullException(nameof(vocabulary));

        _corpus = corpus ??
            throw new ArgumentNullException(nameof(corpus));
    }

    public LanguageLearningResult Run(
        int embeddingDimension,
        int contextRadius,
        int epochs,
        double learningRate,
        int seed = 42)
    {
        var examples =
            new TrainingExampleGenerator()
                .Generate(
                    _corpus,
                    _vocabulary,
                    contextRadius);

        var model =
            new LanguageModel(
                _vocabulary,
                embeddingDimension,
                seed);

        var initialEmbeddings =
            EmbeddingSnapshot.Capture(model);

        var trainer =
            new Trainer(
                model,
                new CrossEntropyLoss(),
                new Backpropagator(),
                new GradientDescentOptimizer(
                    learningRate));

        var history =
            trainer.Train(
                examples,
                epochs);

        return new LanguageLearningResult(
            model,
            initialEmbeddings,
            history);
    }
}
