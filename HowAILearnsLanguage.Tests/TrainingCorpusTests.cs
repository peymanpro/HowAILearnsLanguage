using HowAILearnsLanguage.Domain;

namespace HowAILearnsLanguage.Tests;

public class TrainingCorpusTests
{
    [Fact]
    public void Should_store_training_sentences()
    {
        var corpus = new TrainingCorpus();

        corpus.Add("The cat drinks milk.");
        corpus.Add("The dog drinks water.");

        Assert.Equal(2, corpus.Count);
        Assert.Equal(
            "The cat drinks milk.",
            corpus.Sentences[0]);
    }

    [Fact]
    public void Should_reject_empty_sentence()
    {
        var corpus = new TrainingCorpus();

        Assert.Throws<ArgumentException>(
            () => corpus.Add("   "));
    }
}
