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

        var result =
            experiment.Run(
                embeddingDimension: 8,
                contextRadius: 1,
                epochs: 20,
                learningRate: 0.1,
                seed: 42);

        Assert.Equal(
            20,
            result.History.Count);

        Assert.True(
            result.History[^1].AverageLoss <
            result.History[0].AverageLoss);
    }

    [Fact]
    public void Experiment_should_return_trained_model_and_initial_snapshot()
    {
        var vocabulary =
            CreateVocabulary();

        var corpus =
            new TrainingCorpus();

        corpus.Add(
            "The cat drinks milk.");

        var experiment =
            new LanguageLearningExperiment(
                vocabulary,
                corpus);

        var result =
            experiment.Run(
                embeddingDimension: 8,
                contextRadius: 1,
                epochs: 5,
                learningRate: 0.1,
                seed: 42);

        Assert.NotNull(result.Model);
        Assert.NotNull(result.InitialEmbeddings);

        Assert.Equal(
            vocabulary.Count,
            result.Model.VocabularySize);
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
