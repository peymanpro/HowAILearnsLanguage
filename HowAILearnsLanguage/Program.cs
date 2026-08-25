using System.Globalization;
using HowAILearnsLanguage.Domain;
using HowAILearnsLanguage.Learning;

const string vocabularyPath = "data/vocabulary.json";
const string corpusPath = "data/corpus.json";
const string evaluationPath = "data/evaluation.json";

var trainingData =
    TrainingData.Load(
        vocabularyPath,
        corpusPath);

var vocabulary = new Vocabulary();

foreach (var token in trainingData.VocabularyTokens)
{
    vocabulary.Add(token);
}

var trainingCorpus = new TrainingCorpus();

foreach (var sentence in trainingData.Sentences)
{
    trainingCorpus.Add(sentence);
}

var experiment =
    new LanguageLearningExperiment(
        vocabulary,
        trainingCorpus);

var learningResult =
    experiment.Run(
        embeddingDimension: 8,
        contextRadius: 1,
        epochs: 50,
        learningRate: 0.1,
        seed: 42);

var history = learningResult.History;

Console.WriteLine("HowAILearnsLanguage");
Console.WriteLine("==================");
Console.WriteLine();

Console.WriteLine(
    $"Vocabulary: {vocabulary.Count}");

Console.WriteLine(
    $"Sentences:  {trainingCorpus.Count}");

Console.WriteLine();

Console.WriteLine("Training");
Console.WriteLine("--------");

foreach (var result in history.Where(
             result =>
                 result.Epoch == 1 ||
                 result.Epoch % 10 == 0 ||
                 result.Epoch == history[^1].Epoch))
{
    Console.WriteLine(
        $"Epoch {result.Epoch,3}  " +
        $"Loss: {result.AverageLoss.ToString(
            "F6",
            CultureInfo.InvariantCulture)}");
}

Console.WriteLine();

Console.WriteLine(
    $"Initial loss: {history[0].AverageLoss.ToString(
        "F6",
        CultureInfo.InvariantCulture)}");

Console.WriteLine(
    $"Final loss:   {history[^1].AverageLoss.ToString(
        "F6",
        CultureInfo.InvariantCulture)}");

Console.WriteLine();

Console.WriteLine(
    history[^1].AverageLoss < history[0].AverageLoss
        ? "Learning confirmed: loss decreased."
        : "Learning failed: loss did not decrease.");

var evaluationSentences =
    TrainingData.LoadSentences(
        evaluationPath);

var evaluationCorpus =
    new TrainingCorpus();

foreach (var sentence in evaluationSentences)
{
    evaluationCorpus.Add(sentence);
}

var evaluationExamples =
    new TrainingExampleGenerator()
        .Generate(
            evaluationCorpus,
            vocabulary,
            contextRadius: 1);

var evaluator =
    new Evaluator(
        learningResult.Model,
        new CrossEntropyLoss());

var evaluation =
    evaluator.Evaluate(
        evaluationExamples);

Console.WriteLine();
Console.WriteLine("Evaluation");
Console.WriteLine("----------");
Console.WriteLine();

Console.WriteLine(
    $"Examples:  {evaluation.TotalExamples}");

Console.WriteLine(
    $"Correct:   {evaluation.CorrectPredictions}");

Console.WriteLine(
    $"Accuracy:  {evaluation.Accuracy.ToString(
        "P2",
        CultureInfo.InvariantCulture)}");

Console.WriteLine(
    $"Avg Loss:  {evaluation.AverageLoss.ToString(
        "F6",
        CultureInfo.InvariantCulture)}");

var comparisonEvaluator =
    new EmbeddingEvaluator(
        vocabulary);

var comparisons = new[]
{
    ("cat", "kitten"),
    ("dog", "puppy"),
    ("cat", "dog"),
    ("cat", "milk")
};

Console.WriteLine();
Console.WriteLine("Learned Embeddings");
Console.WriteLine("------------------");
Console.WriteLine();

foreach (var (firstToken, secondToken) in comparisons)
{
    var comparison =
        comparisonEvaluator.Compare(
            learningResult.InitialEmbeddings,
            learningResult.Model,
            firstToken,
            secondToken);

    Console.WriteLine(
        $"{comparison.FirstToken} <-> " +
        $"{comparison.SecondToken}");

    Console.WriteLine(
        $"  Before: {comparison.BeforeTraining.ToString(
            "F4",
            CultureInfo.InvariantCulture)}");

    Console.WriteLine(
        $"  After:  {comparison.AfterTraining.ToString(
            "F4",
            CultureInfo.InvariantCulture)}");

    Console.WriteLine();
}
