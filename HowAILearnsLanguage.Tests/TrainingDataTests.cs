using HowAILearnsLanguage.Domain;

namespace HowAILearnsLanguage.Tests;

public class TrainingDataTests
{
    [Fact]
    public void Should_store_vocabulary_and_sentences()
    {
        var data = new TrainingData(
            ["cat", "dog"],
            [
                "The cat sleeps.",
                "The dog sleeps."
            ]);

        Assert.Equal(2, data.VocabularyTokens.Count);
        Assert.Equal(2, data.Sentences.Count);
    }
}
