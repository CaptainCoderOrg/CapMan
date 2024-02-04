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
            Up => WallTile.Vertical,                      // 0b0001
            Right => WallTile.Horizontal,                 // 0b0010
            Up | Right => WallTile.BottomLeft,            // 0b0011            
            Down => WallTile.Vertical,                    // 0b0100
            Down | Up => WallTile.Vertical,               // 0b0101
            Down | Up | Right => WallTile.TopLeft,        // 0b0111
            Left  => WallTile.TopLeft,                    // 0b1000
            Left | Up => WallTile.TopLeft,                // 0b1001
            Left | Right => WallTile.TopLeft,             // 0b1010
            Left | Right | Up => WallTile.TopLeft,        // 0b1011
            Left | Down => WallTile.TopLeft,              // 0b1100
            Left | Down | Up => WallTile.TopLeft,         // 0b1101
            Left | Down | Right => WallTile.TopLeft,      // 0b1110
            Left | Down | Right | Up => WallTile.TopLeft, // 0b1110

            _ => throw new ArgumentException($"Could not determine wall type from: {neighbors}"),
        };
    }
}