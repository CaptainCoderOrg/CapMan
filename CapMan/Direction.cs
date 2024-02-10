using static Direction;
public enum Direction
{
    Up,
    Down,
    Left,
    Right
}

public static class DirectionExtensions
{
    public static bool IsOpposite(this Direction d0, Direction d1) => (d0, d1) switch
    {
        (Up, Down) => true,
        (Down, Up) => true,
        (Left, Right) => true,
        (Right, Left) => true,
        _ => false,
    };
}