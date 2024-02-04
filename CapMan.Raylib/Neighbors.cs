using static Neighbors;
[Flags]
public enum Neighbors
{
    None     = 0b0000,
    Up       = 0b0001,
    Right    = 0b0010,
    Down     = 0b0100,
    Left     = 0b1000,
    DownLeft = Down | Left,   // 0b1100
    DownRight = Down | Right, // 0b0110
    UpLeft = Up | Left,       // 0b1001
    UpRight = Up | Right,     // 0b0011
    LeftRight = Left | Right,
    UpDown = Up | Down,
}

public static class NeighborExtensions
{
    public static Neighbors[] OrthogonalNeighbors { get; } = [ Up, Down, Left, Right ];
    public static WallTile ToWallTile(this Neighbors neighbors)
    {
        return neighbors switch
        {
            DownLeft => WallTile.TopRight,
            DownRight => WallTile.TopLeft,
            UpLeft => WallTile.BottomRight,
            UpRight => WallTile.BottomLeft,
            LeftRight => WallTile.Horizontal,
            UpDown => WallTile.Vertical,
            _ => throw new ArgumentException($"Could not determine wall type from: {neighbors}"),
        };
    }
}