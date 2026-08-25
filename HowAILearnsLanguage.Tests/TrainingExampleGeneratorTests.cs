using HowAILearnsLanguage.Domain;
using HowAILearnsLanguage.Learning;

namespace HowAILearnsLanguage.Tests;

public class TrainingExampleGeneratorTests
{
    [Fact]
    public void Should_generate_target_context_pairs()
    {
        var vocabulary = CreateVocabulary();

        var corpus = new TrainingCorpus();
        corpus.Add("The cat drinks milk.");

        var generator =
            new TrainingExampleGenerator();

        var examples =
            generator.Generate(
                corpus,
                vocabulary,
                contextRadius: 1);

        Assert.Equal(6, examples.Count);
    }

    [Fact]
    public void Target_should_never_equal_context()
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

        Assert.All(
            examples,
            example =>
                Assert.NotEqual(
                    example.TargetTokenId,
                    example.ContextTokenId));
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
