namespace CapMan.Raylib.Tests;

public class Interpolator_should_
{
    [Theory]
    [InlineData(0, 100, 10, .10)]
    [InlineData(0, 100, 50, .50)]
    [InlineData(0, 100, 150, 1.0)]
    [InlineData(-100, 0, -50, .50)]
    [InlineData(-100, 0, -100, 0.0)]
    [InlineData(-100, 0, -150, 0.0)]
    [InlineData(-100, 0, 0, 1.0)]
    [InlineData(-100, 0, 50, 1.0)]
    [InlineData(-100, 0, -90, 0.1)]
    public void calculate_percentage(double min, double max, double value, double expected)
    {
        Interpolator info = new() { Min = min, Max = max };
        info.Percent(value).ShouldBe(expected, 0.001);
    }
}