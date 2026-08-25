using HowAILearnsLanguage.Learning;

namespace HowAILearnsLanguage.Tests;

public class CrossEntropyLossTests
{
    [Fact]
    public void Loss_should_be_low_when_target_probability_is_high()
    {
        var prediction = new Prediction(
        [
            0.05,
            0.90,
            0.05
        ]);

        var loss = new CrossEntropyLoss();

        var result = loss.Calculate(
            prediction,
            targetTokenId: 1);

        Assert.Equal(
            -Math.Log(0.90),
            result,
            precision: 10);
    }

    [Fact]
    public void Loss_should_be_high_when_target_probability_is_low()
    {
        var prediction = new Prediction(
        [
            0.45,
            0.10,
            0.45
        ]);

        var loss = new CrossEntropyLoss();

        var result = loss.Calculate(
            prediction,
            targetTokenId: 1);

        Assert.Equal(
            -Math.Log(0.10),
            result,
            precision: 10);
    }

    [Fact]
    public void Perfect_prediction_should_produce_near_zero_loss()
    {
        var prediction = new Prediction(
        [
            0.0,
            1.0,
            0.0
        ]);

        var loss = new CrossEntropyLoss();

        var result = loss.Calculate(
            prediction,
            targetTokenId: 1);

        Assert.InRange(
            result,
            0.0,
            1e-10);
    }

    [Fact]
    public void Wrong_target_id_should_be_rejected()
    {
        var prediction = new Prediction(
        [
            0.5,
            0.5
        ]);

        var loss = new CrossEntropyLoss();

        Assert.Throws<ArgumentOutOfRangeException>(
            () => loss.Calculate(
                prediction,
                targetTokenId: 2));
    }
}
