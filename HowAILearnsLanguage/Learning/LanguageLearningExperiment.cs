using HowAILearnsLanguage.Domain;

namespace HowAILearnsLanguage.Learning;

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

    public IReadOnlyList<TrainingResult> Run(
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

        var trainer =
            new Trainer(
                model,
                new CrossEntropyLoss(),
                new Backpropagator(),
                new GradientDescentOptimizer(
                    learningRate));

        return trainer.Train(
            examples,
            epochs);
    }
}
