using HowAILearnsLanguage.Domain;

namespace HowAILearnsLanguage.Tests;

public class VocabularyTests
{
    [Fact]
    public void Should_assign_incremental_ids()
    {
        var vocabulary = new Vocabulary();

        Assert.Equal(0, vocabulary.Add("cat"));
        Assert.Equal(1, vocabulary.Add("dog"));
        Assert.Equal(2, vocabulary.Add("milk"));
    }

    [Fact]
    public void Should_normalize_tokens()
    {
        var vocabulary = new Vocabulary();

        var firstId = vocabulary.Add(" Cat ");
        var secondId = vocabulary.Add("CAT");

        Assert.Equal(firstId, secondId);
        Assert.Equal(1, vocabulary.Count);
    }

    [Fact]
    public void Should_resolve_token_to_id_and_back()
    {
        var vocabulary = new Vocabulary();

        var id = vocabulary.Add("cat");

        Assert.Equal(id, vocabulary.GetId("cat"));
        Assert.Equal("cat", vocabulary.GetToken(id));
    }

    [Fact]
    public void Should_reject_unknown_token()
    {
        var vocabulary = new Vocabulary();

        Assert.Throws<KeyNotFoundException>(
            () => vocabulary.GetId("cat"));
    }

    [Fact]
    public void Should_reject_invalid_id()
    {
        var vocabulary = new Vocabulary();
        vocabulary.Add("cat");

        Assert.Throws<ArgumentOutOfRangeException>(
            () => vocabulary.GetToken(10));
    }

    [Fact]
    public void Should_not_add_duplicate_tokens()
    {
        var vocabulary = new Vocabulary();

        vocabulary.Add("cat");
        vocabulary.Add("cat");

        Assert.Equal(1, vocabulary.Count);
    }
}

