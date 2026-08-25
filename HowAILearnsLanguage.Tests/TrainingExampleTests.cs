using HowAILearnsLanguage.Domain;

namespace HowAILearnsLanguage.Tests;

public class TrainingExampleTests
{
    [Fact]
    public void Should_store_target_and_context_token_ids()
    {
        var example = new TrainingExample(
            targetTokenId: 3,
            contextTokenId: 7);

        Assert.Equal(3, example.TargetTokenId);
        Assert.Equal(7, example.ContextTokenId);
    }

    [Fact]
    public void Should_reject_negative_target_id()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new TrainingExample(-1, 2));
    }

    [Fact]
    public void Should_reject_negative_context_id()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new TrainingExample(1, -1));
    }
}

