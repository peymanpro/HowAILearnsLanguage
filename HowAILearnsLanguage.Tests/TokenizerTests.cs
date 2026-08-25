using HowAILearnsLanguage.Domain;

namespace HowAILearnsLanguage.Tests;

public class TokenizerTests
{
    [Fact]
    public void Should_extract_normalized_tokens()
    {
        var tokenizer = new Tokenizer();

        var tokens =
            tokenizer.Tokenize(
                "The cat drinks milk.");

        Assert.Equal(
            ["the", "cat", "drinks", "milk"],
            tokens);
    }

    [Fact]
    public void Should_ignore_punctuation()
    {
        var tokenizer = new Tokenizer();

        var tokens =
            tokenizer.Tokenize(
                "Cat, dog!");

        Assert.Equal(
            ["cat", "dog"],
            tokens);
    }
}
