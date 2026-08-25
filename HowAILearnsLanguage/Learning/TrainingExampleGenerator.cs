using HowAILearnsLanguage.Domain;

namespace HowAILearnsLanguage.Learning;

public sealed class TrainingExampleGenerator
{
    public IReadOnlyList<TrainingExample> Generate(
        TrainingCorpus corpus,
        Vocabulary vocabulary,
        int contextRadius)
    {
        ArgumentNullException.ThrowIfNull(corpus);
        ArgumentNullException.ThrowIfNull(vocabulary);

        if (contextRadius <= 0)
        {
            throw new ArgumentOutOfRangeException(
                nameof(contextRadius));
        }

        var tokenizer = new Tokenizer();
        var examples = new List<TrainingExample>();

        foreach (var sentence in corpus.Sentences)
        {
            var tokens = tokenizer.Tokenize(sentence);

            var tokenIds = tokens
                .Select(vocabulary.GetId)
                .ToArray();

            for (var targetIndex = 0;
                 targetIndex < tokenIds.Length;
                 targetIndex++)
            {
                var start =
                    Math.Max(
                        0,
                        targetIndex - contextRadius);

                var end =
                    Math.Min(
                        tokenIds.Length - 1,
                        targetIndex + contextRadius);

                for (var contextIndex = start;
                     contextIndex <= end;
                     contextIndex++)
                {
                    if (contextIndex == targetIndex)
                    {
                        continue;
                    }

                    examples.Add(
                        new TrainingExample(
                            tokenIds[targetIndex],
                            tokenIds[contextIndex]));
                }
            }
        }

        return examples;
    }
}
