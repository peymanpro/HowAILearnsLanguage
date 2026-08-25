using HowAILearnsLanguage.Domain;
using HowAILearnsLanguage.Learning;

namespace HowAILearnsLanguage.Tests;

public class LanguageLearningExperimentTests
{
    [Fact]
    public void Experiment_should_reduce_loss()
    {
        var vocabulary =
            CreateVocabulary();

        var corpus =
            new TrainingCorpus();

        corpus.Add(
            "The cat drinks milk.");

        corpus.Add(
            "The cat drinks milk.");

        var experiment =
            new LanguageLearningExperiment(
                vocabulary,
                corpus);

        var history =
            experiment.Run(
                embeddingDimension: 8,
                contextRadius: 1,
                epochs: 20,
                learningRate: 0.1,
                seed: 42);

        Assert.True(
            history[^1].AverageLoss <
            history[0].AverageLoss);
    }

    private static Vocabulary CreateVocabulary()
    {
        var vocabulary =
            new Vocabulary();

        foreach (var token in new[]
        {
            "the",
            "cat",
            "drinks",
            "milk"
        })
        {
            vocabulary.Add(token);
        }

        return vocabulary;
    }
}
