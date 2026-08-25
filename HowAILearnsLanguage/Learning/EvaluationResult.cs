namespace HowAILearnsLanguage.Learning;

public sealed record EvaluationResult(
    double AverageLoss,
    double Accuracy,
    int CorrectPredictions,
    int TotalExamples);
