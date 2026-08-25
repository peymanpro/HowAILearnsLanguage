using HowAILearnsLanguage.Domain;
using HowAILearnsLanguage.Learning;

namespace HowAILearnsLanguage.Tests;

public class LanguageModelTests
{
    [Fact]
    public void Prediction_should_return_probability_for_each_token()
    {
        var vocabulary = CreateVocabulary();

        var model = new LanguageModel(
            vocabulary,
            embeddingDimension: 4);

        var example = new TrainingExample(
            targetTokenId: vocabulary.GetId("cat"),
            contextTokenId: vocabulary.GetId("drinks"));

        var prediction = model.Predict(example);

        Assert.Equal(
            vocabulary.Count,
            prediction.Probabilities.Count);
    }

    [Fact]
    public void Probabilities_should_sum_to_one()
    {
        var vocabulary = CreateVocabulary();

        var model = new LanguageModel(
            vocabulary,
            embeddingDimension: 4);

        var example = new TrainingExample(
            targetTokenId: vocabulary.GetId("cat"),
            contextTokenId: vocabulary.GetId("drinks"));

        var prediction = model.Predict(example);

        Assert.Equal(
            1.0,
            prediction.Probabilities.Sum(),
            precision: 10);
    }

    [Fact]
    public void Probabilities_should_be_between_zero_and_one()
    {
        var vocabulary = CreateVocabulary();

        var model = new LanguageModel(
            vocabulary,
            embeddingDimension: 4);

        var example = new TrainingExample(
            targetTokenId: vocabulary.GetId("cat"),
            contextTokenId: vocabulary.GetId("drinks"));

        var prediction = model.Predict(example);

        Assert.All(
            prediction.Probabilities,
            probability =>
            {
                Assert.InRange(
                    probability,
                    0.0,
                    1.0);
            });
    }

    [Fact]
    public void Prediction_should_be_deterministic_for_same_seed()
    {
        var vocabulary = CreateVocabulary();

        var firstModel = new LanguageModel(
            vocabulary,
            embeddingDimension: 4,
            seed: 123);

        var secondModel = new LanguageModel(
            vocabulary,
            embeddingDimension: 4,
            seed: 123);

        var example = new TrainingExample(
            targetTokenId: vocabulary.GetId("cat"),
            contextTokenId: vocabulary.GetId("drinks"));

        var first = firstModel.Predict(example);
        var second = secondModel.Predict(example);

        Assert.Equal(
            first.Probabilities,
            second.Probabilities);
    }

    private static Vocabulary CreateVocabulary()
    {
        var vocabulary = new Vocabulary();

        vocabulary.Add("the");
        vocabulary.Add("cat");
        vocabulary.Add("dog");
        vocabulary.Add("drinks");
        vocabulary.Add("milk");

        return vocabulary;
    }
}
