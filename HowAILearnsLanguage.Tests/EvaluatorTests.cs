using HowAILearnsLanguage.Domain;
using HowAILearnsLanguage.Learning;

namespace HowAILearnsLanguage.Tests;

public class EvaluatorTests
{
    [Fact]
    public void Should_evaluate_loss_and_accuracy()
    {
        var vocabulary = CreateVocabulary();

        var model =
            new LanguageModel(
                vocabulary,
                embeddingDimension: 8,
                seed: 42);

        var examples =
            new[]
            {
                new TrainingExample(
                    vocabulary.GetId("cat"),
                    vocabulary.GetId("drinks")),
                new TrainingExample(
                    vocabulary.GetId("cat"),
                    vocabulary.GetId("milk"))
            };

        var evaluator =
            new Evaluator(
                model,
                new CrossEntropyLoss());

        var result =
            evaluator.Evaluate(examples);

        Assert.True(result.AverageLoss > 0);
        Assert.InRange(result.Accuracy, 0.0, 1.0);
        Assert.Equal(2, result.TotalExamples);
    }

    [Fact]
    public void Correct_predictions_should_be_counted()
    {
        var vocabulary = CreateVocabulary();

        var model =
            new LanguageModel(
                vocabulary,
                embeddingDimension: 8,
                seed: 42);

        var example =
            new TrainingExample(
                vocabulary.GetId("cat"),
                vocabulary.GetId("drinks"));

        var evaluator =
            new Evaluator(
                model,
                new CrossEntropyLoss());

        var result =
            evaluator.Evaluate(
                [example]);

        Assert.InRange(
            result.CorrectPredictions,
            0,
            1);
    }

    [Fact]
    public void Should_reject_empty_evaluation_set()
    {
        var vocabulary = CreateVocabulary();

        var model =
            new LanguageModel(
                vocabulary,
                embeddingDimension: 8);

        var evaluator =
            new Evaluator(
                model,
                new CrossEntropyLoss());

        Assert.Throws<ArgumentException>(
            () => evaluator.Evaluate([]));
    }

    private static Vocabulary CreateVocabulary()
    {
        var vocabulary =
            new Vocabulary();

        foreach (var token in new[]
        {
            "the",
            "cat",
            "dog",
            "kitten",
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
