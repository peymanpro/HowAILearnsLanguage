using HowAILearnsLanguage.Domain;
using HowAILearnsLanguage.Learning;

namespace HowAILearnsLanguage.Tests;

public class BackpropagatorTests
{
    [Fact]
    public void Should_return_gradient_for_input_embedding()
    {
        var vocabulary = CreateVocabulary();

        var model = new LanguageModel(
            vocabulary,
            embeddingDimension: 4,
            seed: 42);

        var example = new TrainingExample(
            targetTokenId: vocabulary.GetId("cat"),
            contextTokenId: vocabulary.GetId("drinks"));

        var backpropagator = new Backpropagator();

        var gradients =
            backpropagator.Calculate(
                model,
                example);

        Assert.Equal(
            model.EmbeddingDimension,
            gradients.InputEmbeddingGradient.Dimension);
    }

    [Fact]
    public void Should_return_one_output_gradient_per_vocabulary_token()
    {
        var vocabulary = CreateVocabulary();

        var model = new LanguageModel(
            vocabulary,
            embeddingDimension: 4);

        var example = new TrainingExample(
            targetTokenId: vocabulary.GetId("cat"),
            contextTokenId: vocabulary.GetId("drinks"));

        var gradients =
            new Backpropagator().Calculate(
                model,
                example);

        Assert.Equal(
            vocabulary.Count,
            gradients.OutputEmbeddingGradients.Count);
    }

    [Fact]
    public void Target_output_gradient_should_be_different_from_non_target_gradient()
    {
        var vocabulary = CreateVocabulary();

        var model = new LanguageModel(
            vocabulary,
            embeddingDimension: 4,
            seed: 42);

        var targetId =
            vocabulary.GetId("drinks");

        var example = new TrainingExample(
            targetTokenId: vocabulary.GetId("cat"),
            contextTokenId: targetId);

        var gradients =
            new Backpropagator().Calculate(
                model,
                example);

        var targetGradient =
            gradients.OutputEmbeddingGradients[targetId];

        var otherId =
            vocabulary.GetId("milk");

        var otherGradient =
            gradients.OutputEmbeddingGradients[otherId];

        Assert.False(
            targetGradient.ToArray()
                .SequenceEqual(
                    otherGradient.ToArray()));
    }

    [Fact]
    public void Backpropagation_should_be_deterministic_for_same_model()
    {
        var vocabulary = CreateVocabulary();

        var firstModel = new LanguageModel(
            vocabulary,
            embeddingDimension: 4,
            seed: 42);

        var secondModel = new LanguageModel(
            vocabulary,
            embeddingDimension: 4,
            seed: 42);

        var example = new TrainingExample(
            targetTokenId: vocabulary.GetId("cat"),
            contextTokenId: vocabulary.GetId("drinks"));

        var calculator = new Backpropagator();

        var first =
            calculator.Calculate(
                firstModel,
                example);

        var second =
            calculator.Calculate(
                secondModel,
                example);

        Assert.Equal(
            first.InputEmbeddingGradient.ToArray(),
            second.InputEmbeddingGradient.ToArray());

        for (var i = 0;
             i < first.OutputEmbeddingGradients.Count;
             i++)
        {
            Assert.Equal(
                first.OutputEmbeddingGradients[i].ToArray(),
                second.OutputEmbeddingGradients[i].ToArray());
        }
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
