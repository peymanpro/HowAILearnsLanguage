using HowAILearnsLanguage.Domain;
using HowAILearnsLanguage.Mathematics;

namespace HowAILearnsLanguage.Learning;

public sealed class Backpropagator
{
    public LanguageGradients Calculate(
        LanguageModel model,
        TrainingExample example)
    {
        ArgumentNullException.ThrowIfNull(model);
        ArgumentNullException.ThrowIfNull(example);

        var prediction = model.Predict(example);

        var inputVector =
            model.GetInputEmbedding(
                example.TargetTokenId);

        var inputGradient =
            new Vector(
                new double[inputVector.Dimension]);

        var outputGradients =
            new Vector[model.VocabularySize];

        for (var tokenId = 0;
             tokenId < model.VocabularySize;
             tokenId++)
        {
            var outputVector =
                model.GetOutputEmbedding(tokenId);

            var probability =
                prediction.Probabilities[tokenId];

            var target =
                tokenId == example.ContextTokenId
                    ? 1.0
                    : 0.0;

            var delta =
                probability - target;

            var outputGradient =
                inputVector.Multiply(delta);

            outputGradients[tokenId] =
                outputGradient;

            inputGradient =
                inputGradient.Add(
                    outputVector.Multiply(delta));
        }

        return new LanguageGradients(
            example.TargetTokenId,
            example.ContextTokenId,
            inputGradient,
            outputGradients);
    }
}
