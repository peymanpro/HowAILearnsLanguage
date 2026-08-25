using HowAILearnsLanguage.Mathematics;

namespace HowAILearnsLanguage.Learning;

public interface IOptimizer
{
    Vector Update(
        Vector parameters,
        Vector gradients);
}
