using HowAILearnsLanguage.Learning;
using HowAILearnsLanguage.Mathematics;

namespace HowAILearnsLanguage.Tests;

public class GradientDescentOptimizerTests
{
    [Fact]
    public void Should_move_parameters_in_the_opposite_direction_of_gradient()
    {
        var optimizer =
            new GradientDescentOptimizer(0.1);

        var parameters =
            new Vector([1.0, 2.0, 3.0]);

        var gradients =
            new Vector([0.5, -1.0, 2.0]);

        var updated =
            optimizer.Update(
                parameters,
                gradients);

        Assert.Equal(
            [0.95, 2.1, 2.8],
            updated.ToArray());
    }

    [Fact]
    public void Should_reject_non_positive_learning_rate()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => new GradientDescentOptimizer(0));

        Assert.Throws<ArgumentOutOfRangeException>(
            () => new GradientDescentOptimizer(-0.1));
    }

    [Fact]
    public void Should_reject_incompatible_dimensions()
    {
        var optimizer =
            new GradientDescentOptimizer(0.1);

        var parameters =
            new Vector([1.0, 2.0]);

        var gradients =
            new Vector([1.0, 2.0, 3.0]);

        Assert.Throws<ArgumentException>(
            () => optimizer.Update(
                parameters,
                gradients));
    }
}
