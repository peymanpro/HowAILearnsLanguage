namespace HowAILearnsLanguage.Learning;

public sealed record EmbeddingSimilarityComparison(
    string FirstToken,
    string SecondToken,
    double BeforeTraining,
    double AfterTraining);
