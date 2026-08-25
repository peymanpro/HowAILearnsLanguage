namespace HowAILearnsLanguage.Learning;

public interface ILossFunction
{
    double Calculate(
        Prediction prediction,
        int targetTokenId);
}
