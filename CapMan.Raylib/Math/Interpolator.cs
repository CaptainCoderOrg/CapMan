namespace CapMan.Raylib;

public readonly record struct Interpolator()
{
    public double Min { get; init; } = 0;
    public double Max { get; init; } = 1;
    public double Percent(double value) => Percent(value, Min, Max);
    public static double Percent(double value, double min, double max) => (Math.Clamp(value, min, max) - min) / (max - min);
    public static double Percent(double value, double max) => Percent(value, 0, max);
}