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

}