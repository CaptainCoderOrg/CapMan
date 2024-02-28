namespace CapMan.Raylib;

using Raylib_cs;

public readonly record struct ProgressBar()
{
    public int X { get; init; } = 0;
    public int Y { get; init; } = 0;
    public int Width { get; init; } = 0;
    public int Height { get; init; } = 0;
    public Color Color { get; init; } = Color.White;
}

public static class ProgessBarExtensions
{
    public static void Render(this ProgressBar rectangle, double percent) =>
        Raylib.DrawRectangle(rectangle.X, rectangle.Y, (int)(percent * rectangle.Width), rectangle.Height, rectangle.Color);

    public static void Render(this ProgressBar bar, double value, double max) => Render(bar, Interpolator.Percent(value, max));
}