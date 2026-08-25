using HowAILearnsLanguage.Domain;
using HowAILearnsLanguage.Learning;

namespace HowAILearnsLanguage.Tests;

public class TrainerTests
{
    [Fact]
    public void Training_should_reduce_average_loss()
    {
        var vocabulary = CreateVocabulary();

        var corpus = new TrainingCorpus();
        corpus.Add("The cat drinks milk.");
        corpus.Add("The cat drinks milk.");

        var examples =
            new TrainingExampleGenerator()
                .Generate(
                    corpus,
                    vocabulary,
                    contextRadius: 1);

        var model =
            new LanguageModel(
                vocabulary,
                embeddingDimension: 8,
                seed: 42);

        var trainer =
            new Trainer(
                model,
                new CrossEntropyLoss(),
                new Backpropagator(),
                new GradientDescentOptimizer(0.1));

        var history =
            trainer.Train(
                examples,
                epochs: 30);

        Assert.Equal(30, history.Count);

        Assert.True(
            history[^1].AverageLoss <
            history[0].AverageLoss);
    }

    [Fact]
    public void Training_results_should_be_ordered_by_epoch()
    {
        var vocabulary = CreateVocabulary();

        var corpus = new TrainingCorpus();
        corpus.Add("The cat drinks milk.");

        var examples =
            new TrainingExampleGenerator()
                .Generate(
                    corpus,
                    vocabulary,
                    contextRadius: 1);

        var trainer =
            new Trainer(
                new LanguageModel(
                    vocabulary,
                    embeddingDimension: 4),
                new CrossEntropyLoss(),
                new Backpropagator(),
                new GradientDescentOptimizer(0.1));

        var history =
            trainer.Train(
                examples,
                epochs: 5);

        Assert.Equal(
            [1, 2, 3, 4, 5],
            history.Select(x => x.Epoch));
    }

    private static Vocabulary CreateVocabulary()
    {
        var vocabulary = new Vocabulary();

        vocabulary.Add("the");
        vocabulary.Add("cat");
        vocabulary.Add("drinks");
        vocabulary.Add("milk");

        return vocabulary;
    }
}
