using HowAILearnsLanguage.Domain;
using HowAILearnsLanguage.Learning;

const string vocabularyPath =
    "data/vocabulary.json";

const string corpusPath =
    "data/corpus.json";

var trainingData =
    TrainingData.Load(
        vocabularyPath,
        corpusPath);

var vocabulary =
    new Vocabulary();

foreach (var token in trainingData.VocabularyTokens)
{
    vocabulary.Add(token);
}

var corpus =
    new TrainingCorpus();

foreach (var sentence in trainingData.Sentences)
{
    corpus.Add(sentence);
}

var experiment =
    new LanguageLearningExperiment(
        vocabulary,
        corpus);

var history =
    experiment.Run(
        embeddingDimension: 8,
        contextRadius: 1,
        epochs: 50,
        learningRate: 0.1,
        seed: 42);

Console.WriteLine(
    "HowAILearnsLanguage");

Console.WriteLine(
    "==================");

Console.WriteLine();

Console.WriteLine(
    $"Vocabulary: {vocabulary.Count}");

Console.WriteLine(
    $"Sentences:  {corpus.Count}");

Console.WriteLine();

Console.WriteLine(
    "Training");

Console.WriteLine(
    "--------");

foreach (var result in history.Where(
    result =>
        result.Epoch == 1 ||
        result.Epoch % 10 == 0 ||
        result.Epoch == history[^1].Epoch))
{
    Console.WriteLine(
        $"Epoch {result.Epoch,3}  " +
        $"Loss: {result.AverageLoss:F6}");
}

Console.WriteLine();

Console.WriteLine(
    $"Initial loss: {history[0].AverageLoss:F6}");

Console.WriteLine(
    $"Final loss:   {history[^1].AverageLoss:F6}");

Console.WriteLine();

Console.WriteLine(
    history[^1].AverageLoss <
    history[0].AverageLoss
        ? "Learning confirmed: loss decreased."
        : "Learning failed: loss did not decrease.");
