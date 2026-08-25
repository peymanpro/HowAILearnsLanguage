using HowAILearnsLanguage.Domain;
using HowAILearnsLanguage.Learning;

namespace HowAILearnsLanguage.Tests;

public class EmbeddingEvaluatorTests
{
    [Fact]
    public void Should_compare_embedding_similarity_before_and_after_training()
    {
        var vocabulary =
            CreateVocabulary();

        var model =
            new LanguageModel(
                vocabulary,
                embeddingDimension: 8,
                seed: 42);

        var before =
            EmbeddingSnapshot.Capture(model);

        var trainingCorpus =
            new TrainingCorpus();

        trainingCorpus.Add(
            "The cat drinks milk.");

        trainingCorpus.Add(
            "The kitten drinks milk.");

        var examples =
            new TrainingExampleGenerator()
                .Generate(
                    trainingCorpus,
                    vocabulary,
                    contextRadius: 1);

        var trainer =
            new Trainer(
                model,
                new CrossEntropyLoss(),
                new Backpropagator(),
                new GradientDescentOptimizer(0.1));

        trainer.Train(
            examples,
            epochs: 20);

        var evaluator =
            new EmbeddingEvaluator(
                vocabulary);

        var result =
            evaluator.Compare(
                before,
                model,
                "cat",
                "kitten");

        Assert.Equal("cat", result.FirstToken);
        Assert.Equal("kitten", result.SecondToken);

        Assert.InRange(
            result.BeforeTraining,
            -1.0,
            1.0);

        Assert.InRange(
            result.AfterTraining,
            -1.0,
            1.0);
    }

    [Fact]
    public void Snapshot_should_remain_unchanged_after_training()
    {
        var vocabulary =
            CreateVocabulary();

        var model =
            new LanguageModel(
                vocabulary,
                embeddingDimension: 8,
                seed: 42);

        var before =
            EmbeddingSnapshot.Capture(model);

        var original =
            before
                .Get(vocabulary.GetId("cat"))
                .ToArray();

        var trainer =
            new Trainer(
                model,
                new CrossEntropyLoss(),
                new Backpropagator(),
                new GradientDescentOptimizer(0.1));

        var corpus =
            new TrainingCorpus();

        corpus.Add(
            "The cat drinks milk.");

        var examples =
            new TrainingExampleGenerator()
                .Generate(
                    corpus,
                    vocabulary,
                    contextRadius: 1);

        trainer.Train(
            examples,
            epochs: 10);

        Assert.Equal(
            original,
            before
                .Get(vocabulary.GetId("cat"))
                .ToArray());
    }

    [Fact]
    public void Should_reject_unknown_token()
    {
        var vocabulary =
            CreateVocabulary();

        var model =
            new LanguageModel(
                vocabulary,
                embeddingDimension: 4);

        var snapshot =
            EmbeddingSnapshot.Capture(model);

        var evaluator =
            new EmbeddingEvaluator(
                vocabulary);

        Assert.Throws<KeyNotFoundException>(
            () =>
                evaluator.Compare(
                    snapshot,
                    model,
                    "cat",
                    "unknown"));
    }

    private static Vocabulary CreateVocabulary()
    {
        var vocabulary =
            new Vocabulary();

        foreach (var token in new[]
        {
            "the",
            "cat",
            "kitten",
            "dog",
            "puppy",
            "drinks",
            "likes",
            "sleeps",
            "milk",
            "water",
            "food",
            "on",
            "mat"
        })
        {
            vocabulary.Add(token);
        }

        return vocabulary;
    }
}
