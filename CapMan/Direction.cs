using static CapMan.Direction;

namespace CapMan;

public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public static class DirectionExtensions
{
    public static Direction Opposite(this Direction d) => d switch
    {
        Up => Down,
        Down => Up,
        Left => Right,
        Right => Left,
        _ => throw new Exception($"Unknown direction {d}"),

    };

    public static bool IsOpposite(this Direction d0, Direction d1) => (d0, d1) switch
    {
        (Up, Down) => true,
        (Down, Up) => true,
        (Left, Right) => true,
        (Right, Left) => true,
        _ => false,
    };

    public static Direction[] Turns(this Direction currentDirection) => [.. Enum.GetValues<Direction>().Where(d => d != currentDirection)];

    public static (int X, int Y) SnapPosition(this Direction movingDirection, double x, double y) => movingDirection switch
    {
        Up => ((int)x, (int)Math.Ceiling(y)),
        Left => ((int)Math.Ceiling(x), (int)y),
        _ => ((int)x, (int)y),
    };

    public static (int X, int Y) Step(this Direction movingDirection, int x, int y) => movingDirection switch
    {
        Up => (x, y - 1),
        Down => (x, y + 1),
        Left => (x - 1, y),
        Right => (x + 1, y),
        _ => throw new Exception($"Unknown direction {movingDirection}"),
    };
}