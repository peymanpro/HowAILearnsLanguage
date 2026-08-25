using HowAILearnsLanguage.Mathematics;

namespace HowAILearnsLanguage.Tests;

public class VectorTests
{
    [Fact]
    public void Should_create_vector()
    {
        var vector = new Vector([1, 2, 3]);

        Assert.Equal(3, vector.Dimension);
        Assert.Equal([1, 2, 3], vector.ToArray());
    }

    [Fact]
    public void Should_add_vectors()
    {
        var left = new Vector([1, 2, 3]);
        var right = new Vector([4, 5, 6]);

        var result = left.Add(right);

        Assert.Equal([5, 7, 9], result.ToArray());
    }

    [Fact]
    public void Should_subtract_vectors()
    {
        var left = new Vector([1, 2, 3]);
        var right = new Vector([4, 5, 6]);

        var result = left.Subtract(right);

        Assert.Equal([-3, -3, -3], result.ToArray());
    }

    [Fact]
    public void Should_multiply_by_scalar()
    {
        var vector = new Vector([1, 2, 3]);

        var result = vector.Multiply(2);

        Assert.Equal([2, 4, 6], result.ToArray());
    }

    [Fact]
    public void Should_calculate_dot_product()
    {
        var left = new Vector([1, 2, 3]);
        var right = new Vector([4, 5, 6]);

        Assert.Equal(32, left.Dot(right));
    }

    [Fact]
    public void Should_calculate_norm()
    {
        var vector = new Vector([3, 4]);

        Assert.Equal(5, vector.Norm());
    }

    [Fact]
    public void Should_calculate_cosine_similarity()
    {
        var left = new Vector([1, 0]);
        var right = new Vector([1, 0]);

        Assert.Equal(1, left.CosineSimilarity(right));
    }

    [Fact]
    public void Should_reject_empty_vector()
    {
        Assert.Throws<ArgumentException>(
            () => new Vector([]));
    }

    [Fact]
    public void Should_reject_non_finite_values()
    {
        Assert.Throws<ArgumentException>(
            () => new Vector([1, double.NaN]));

        Assert.Throws<ArgumentException>(
            () => new Vector([1, double.PositiveInfinity]));
    }

    [Fact]
    public void Should_reject_incompatible_dimensions()
    {
        var left = new Vector([1, 2]);
        var right = new Vector([1, 2, 3]);

        Assert.Throws<ArgumentException>(
            () => left.Add(right));

        Assert.Throws<ArgumentException>(
            () => left.Dot(right));
    }

    [Fact]
    public void Should_reject_cosine_similarity_for_zero_vector()
    {
        var zero = new Vector([0, 0]);
        var vector = new Vector([1, 0]);

        Assert.Throws<InvalidOperationException>(
            () => zero.CosineSimilarity(vector));
    }
}
