using HowAILearnsLanguage.Domain;
using HowAILearnsLanguage.Learning;

namespace HowAILearnsLanguage.Tests;

public class LearningStepTests
{
    [Fact]
    public void One_gradient_descent_step_should_reduce_loss()
    {
        var vocabulary = CreateVocabulary();

        var model = new LanguageModel(
            vocabulary,
            embeddingDimension: 8,
            seed: 42);

        var example = new TrainingExample(
            targetTokenId: vocabulary.GetId("cat"),
            contextTokenId: vocabulary.GetId("drinks"));

        var lossFunction =
            new CrossEntropyLoss();

        var backpropagator =
            new Backpropagator();

        var optimizer =
            new GradientDescentOptimizer(0.5);

        var before =
            lossFunction.Calculate(
                model.Predict(example),
                example.ContextTokenId);

        var gradients =
            backpropagator.Calculate(
                model,
                example);

        model.ApplyGradients(
            gradients,
            optimizer);

        var after =
            lossFunction.Calculate(
                model.Predict(example),
                example.ContextTokenId);

        Assert.True(
            after < before,
            $"Expected loss to decrease. Before={before}, After={after}");
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
