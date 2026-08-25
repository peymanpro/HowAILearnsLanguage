namespace HowAILearnsLanguage.Mathematics;

public sealed class Vector
{
    private readonly double[] _values;

    public Vector(IEnumerable<double> values)
    {
        ArgumentNullException.ThrowIfNull(values);

        _values = values.ToArray();

        if (_values.Length == 0)
        {
            throw new ArgumentException(
                "A vector must contain at least one value.",
                nameof(values));
        }

        if (_values.Any(value => !double.IsFinite(value)))
        {
            throw new ArgumentException(
                "Vector values must be finite.",
                nameof(values));
        }
    }

    public int Dimension => _values.Length;

    public double this[int index] => _values[index];

    public double[] ToArray() => (double[])_values.Clone();

    public Vector Add(Vector other)
    {
        EnsureSameDimension(other);

        var result = new double[Dimension];

        for (var i = 0; i < Dimension; i++)
        {
            result[i] = _values[i] + other[i];
        }

        return new Vector(result);
    }

    public Vector Subtract(Vector other)
    {
        EnsureSameDimension(other);

        var result = new double[Dimension];

        for (var i = 0; i < Dimension; i++)
        {
            result[i] = _values[i] - other[i];
        }

        return new Vector(result);
    }

    public Vector Multiply(double scalar)
    {
        if (!double.IsFinite(scalar))
        {
            throw new ArgumentException(
                "Scalar must be finite.",
                nameof(scalar));
        }

        var result = new double[Dimension];

        for (var i = 0; i < Dimension; i++)
        {
            result[i] = _values[i] * scalar;
        }

        return new Vector(result);
    }

    public double Dot(Vector other)
    {
        EnsureSameDimension(other);

        var result = 0.0;

        for (var i = 0; i < Dimension; i++)
        {
            result += _values[i] * other[i];
        }

        return result;
    }

    public double Norm()
    {
        return Math.Sqrt(Dot(this));
    }

    public double CosineSimilarity(Vector other)
    {
        EnsureSameDimension(other);

        var leftNorm = Norm();
        var rightNorm = other.Norm();

        if (leftNorm == 0 || rightNorm == 0)
        {
            throw new InvalidOperationException(
                "Cosine similarity is undefined for a zero vector.");
        }

        var similarity = Dot(other) / (leftNorm * rightNorm);

        return Math.Clamp(similarity, -1.0, 1.0);
    }

    private void EnsureSameDimension(Vector other)
    {
        ArgumentNullException.ThrowIfNull(other);

        if (Dimension != other.Dimension)
        {
            throw new ArgumentException(
                $"Vector dimensions must match. " +
                $"Expected {Dimension}, received {other.Dimension}.");
        }
    }
}
